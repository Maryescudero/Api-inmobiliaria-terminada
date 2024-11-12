//program.cs
using System.Text;
using inmobiliaria.Models;
using inmobiliaria.Repositorios;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using inmobiliaria.Servicio;



var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://*:5000");

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configura CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Obtener la cadena de conexión de appsettings.json
var connectionString = builder.Configuration.GetConnectionString("Mysql");

// Agregar el DbContext utilizando la cadena de conexión
#pragma warning disable CS8604 // Posible argumento de referencia nulo
builder.Services.AddDbContext<DataContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
#pragma warning restore CS8604 // Posible argumento de referencia nulo

builder.Services.AddScoped<RepositorioInquilino>();
builder.Services.AddScoped<RepositorioInmueble>();
builder.Services.AddScoped<RepositorioPropietario>();
builder.Services.AddScoped<RepositorioContrato>();
builder.Services.AddScoped<RepositorioPago>();
builder.Services.AddScoped<RepositorioTipoInmuebles>();
builder.Services.AddScoped<Auth>();
builder.Services.AddScoped<EmailSender>();
builder.Services.AddScoped<RepositorioUsuario>();
builder.Services.AddSwaggerGen();

var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience))
{
    Console.WriteLine("Error: Clave, emisor o audiencia del token JWT no configurados correctamente");
}
else
{
    builder.Services.AddAuthentication()
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        var key = Encoding.ASCII.GetBytes(jwtKey);

        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false, //este en falso para las pruebas
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });
}

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Propietario", policy => policy.RequireRole("Propietario"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    builder.WebHost.UseUrls("http://*:5000");
    app.UseSwagger();
    app.UseSwaggerUI();
}


/*// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}*/

// Habilitar CORS antes de la redirección HTTPS y la autenticación
app.UseCors("AllowAll");

//app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();

// Mapear los controladores
app.MapControllers();

// Ejecutar la aplicación
app.Run();




