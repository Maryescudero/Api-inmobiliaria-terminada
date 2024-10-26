//contratoController.cs
using inmobiliaria.Models;
using inmobiliaria.Repositorios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace inmobiliaria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "Propietario")]
    public class ContratoController : ControllerBase
    {
        private readonly RepositorioContrato repositorioContrato;

        public ContratoController(RepositorioContrato repo)
        {
            this.repositorioContrato = repo;
        }

        private int GetPropietarioId()
        {
#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
            var userIdClaim = User.FindFirst("id").Value;
#pragma warning restore CS8602 // Desreferencia de una referencia posiblemente NULL.
            int userId = int.Parse(userIdClaim);
            return userId;
        }


        [HttpDelete("borrar/{id}")]
        public ActionResult<Contrato> Delete(int id)
        {
            var propietarioId = GetPropietarioId();
            if (propietarioId == -1)
            {
                return BadRequest("No se pudo obtener el ID del propietario.");
            }

            var contrato = repositorioContrato.BuscarPorId(id);
            if (contrato == null || contrato.inmueble?.PropietarioId != propietarioId)
            {
                return NotFound();
            }

            var exito = repositorioContrato.EliminadoLogico(id);
            if (!exito)
            {
                return StatusCode(500, "Error al eliminar");
            }

            return NoContent();
        }

//http://localhost:5000/api/contrato(todos los inmuebles del logeado)
        [HttpGet]
        public ActionResult<List<Contrato>> Get()
        {
            var propietarioId = GetPropietarioId();
            if (propietarioId == -1)
            {
                return BadRequest("No se pudo obtener el ID del propietario.");
            }

            var contrato = repositorioContrato.ObtenerActivosPorPropietario(propietarioId);
            if (contrato == null)
            {
                return NotFound();
            }

            return contrato;
        }

//http://localhost:5000/api/contrato/22
        [HttpGet("{id}")]
        public ActionResult<Contrato> Get(int id)
        {
            var propietarioId = GetPropietarioId();
            if (propietarioId == -1)
            {
                return BadRequest("No se pudo obtener el ID del propietario.");
            }

            var contrato = repositorioContrato.BuscarPorId(id);
            if (contrato == null || contrato.inmueble?.PropietarioId != propietarioId)
            {
                return NotFound();
            }

            return contrato;
        }

        [HttpPost("guardar")]
        public ActionResult<Contrato> Post(Contrato contrato)
        {
            //las fechas tienen que venir desde el formulario asi:
            //como si fuera un string
            //"fecha_inicio": "2024-05-01"
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            repositorioContrato.Crear(contrato);
            return Ok(contrato);
        }

//http://localhost:5000/api/contrato/actualizar/17(o actualiza la fecha)
        [HttpPut("actualizar/{id}")]
        public ActionResult<Contrato> Put(int id, Contrato contrato)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState + ": El modelo no coincide con el esperado");
            }

            var contratoExistente = repositorioContrato.BuscarPorId(id);
            if (contratoExistente == null || contratoExistente.inmueble?.PropietarioId != GetPropietarioId())
            {
                return NotFound();
            }

            if (contratoExistente.fecha_inicio == DateOnly.FromDateTime(DateTime.Today) || contrato.fecha_inicio != default(DateOnly))
            {
                contratoExistente.fecha_inicio = contrato.fecha_inicio != default(DateOnly) ? contrato.fecha_inicio : contratoExistente.fecha_inicio;
                contratoExistente.fecha_correcta = contrato.fecha_correcta != default(DateOnly) ? contrato.fecha_correcta : contratoExistente.fecha_correcta;
                contratoExistente.monto = contrato.monto > 0 ? contrato.monto : contratoExistente.monto;

                repositorioContrato.Actualizar(contratoExistente);
                return Ok(contratoExistente);
            }
            else
            {
                return BadRequest("No se puede actualizar el contrato porque la fecha de inicio no es hoy.");
            }
        }

//http://localhost:5000/api/contrato/alquilados
        [HttpGet("alquilados")]
        public ActionResult<List<Contrato>> GetAlquilados()
        {
            var userId = GetPropietarioId();
            if (userId == 0)
            {
                return BadRequest("Usuario inválido.");
            }

            var inmuebles = repositorioContrato.InmueblesAlquilados(userId);

            if (inmuebles == null)
            {
                return NotFound("No se encontraron inmuebles alquilados.");
            }
            else if (inmuebles.Count == 0)
            {
                return NotFound("No tiene inmuebles alquilados.");
            }

            return inmuebles;
        }

    }
}
