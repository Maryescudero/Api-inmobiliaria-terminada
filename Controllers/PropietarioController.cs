//propietariocontroller.cs
using inmobiliaria.Repositorios;
using inmobiliaria.Servicio;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using System.Linq;


namespace inmobiliaria.Models
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PropietarioController : ControllerBase
    {
        private readonly RepositorioPropietario repositorioPropietario;
        private readonly IWebHostEnvironment hostingEnvironment;
        public PropietarioController(RepositorioPropietario repo, IWebHostEnvironment env)
        {
            repositorioPropietario = repo;
            hostingEnvironment = env;
        }

        private int GetPropietarioId()
        {
            var userIdClaim = User.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new InvalidOperationException("User ID not found in claims.");
            }
            return userId;
        }

        [HttpGet]
        public ActionResult<Propietario> Get()
        {
            try
            {
                var userId = GetPropietarioId();
                var propietario = repositorioPropietario.BuscarPorId(userId);
                if (propietario == null) return NotFound();

                propietario.password = null;
                return propietario;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }


        //http://localhost:5000/api/propietario/actualizar  (actualizo datos)
        [HttpPut("actualizar")]
        public ActionResult<Propietario> ActualizarPropietario([FromBody] Propietario propietario)
        {
            try
            {
                var id = GetPropietarioId();
                var propietarioExistente = repositorioPropietario.BuscarPorId(id);
                if (propietarioExistente == null) return NotFound();

                propietarioExistente.nombre = propietario.nombre ?? propietarioExistente.nombre;
                propietarioExistente.apellido = propietario.apellido ?? propietarioExistente.apellido;
                propietarioExistente.dni = propietario.dni ?? propietarioExistente.dni;
                propietarioExistente.telefono = propietario.telefono ?? propietarioExistente.telefono;
                propietarioExistente.email = propietario.email ?? propietarioExistente.email;

                if (!repositorioPropietario.Actualizar(propietarioExistente))
                {
                    return StatusCode(500, "Error al actualizar el propietario");
                }

                propietarioExistente.password = null;
                return Ok(propietarioExistente);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        //http://localhost:5000/api/propietario/actualizar/avatar (v como for: nuevoAvatarFile/ tipo file/ descargar imagen )
        [HttpPost("actualizar/avatar")]
        public ActionResult<Propietario> ActualizarAvatar([FromForm] IFormFile nuevoAvatarFile)
        {
            try
            {
                var id = GetPropietarioId();
                var propietarioExistente = repositorioPropietario.BuscarPorId(id);
                if (propietarioExistente == null) return NotFound();

                if (nuevoAvatarFile == null || !ImagenValida(nuevoAvatarFile))
                {
                    return BadRequest("Imagen no válida");
                }

                string folderPath = Path.Combine(hostingEnvironment.WebRootPath, "img", "uploads");
                Directory.CreateDirectory(folderPath);
                var filePath = Path.Combine(folderPath, nuevoAvatarFile.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    nuevoAvatarFile.CopyTo(stream);
                }

                propietarioExistente.avatarUrl = Path.GetFileName(filePath);

                if (!repositorioPropietario.Actualizar(propietarioExistente))
                {
                    return StatusCode(500, "Error al actualizar el avatar del propietario");
                }

                return Ok(propietarioExistente);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        private bool ImagenValida(IFormFile file)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            return allowedExtensions.Contains(extension);
        }

//http://localhost:5000/api/propietario/actualizar/pass  (funciona ya me la hassea)
        [HttpPatch("actualizar/pass")]
        public IActionResult CambiarPass([FromForm] string pass)
        {
            try
            {
                var id = GetPropietarioId();
                var usuarioExistente = repositorioPropietario.BuscarPorId(id);

                if (usuarioExistente == null) return NotFound("Usuario no encontrado.");
                if (string.IsNullOrEmpty(pass) || pass.Length < 8) return BadRequest("La contraseña debe tener al menos 8 caracteres.");

                if (!repositorioPropietario.CambiarPass(usuarioExistente.id, pass))
                {
                    return StatusCode(500, "Error al actualizar la contraseña.");
                }

                usuarioExistente.password = null;
                return Ok("La contraseña se cambió correctamente, vuelva a iniciar sesión.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }


        [HttpPost("crear")]
        public IActionResult CrearPropietario([FromForm] Propietario propietario)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(propietario.email) || string.IsNullOrWhiteSpace(propietario.password))
                {
                    return BadRequest(new { mensaje = "Email y contraseña son obligatorios" });
                }

                if (repositorioPropietario.Crear(propietario))
                {
                    return Ok(new { mensaje = "Propietario creado exitosamente" });
                }

                return BadRequest(new { mensaje = "No se pudo crear el propietario" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = "Error al crear el propietario", error = ex.Message });
            }
        }
    }
}
