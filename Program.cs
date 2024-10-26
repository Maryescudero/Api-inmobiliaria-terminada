//program.cs
using System.Text;
using inmobiliaria.Models;
using inmobiliaria.Repositorios;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using inmobiliaria.Servicio;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:5000", "https://localhost:5001", "http://*:5000", "https://*:5001");

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
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
    builder.WebHost.UseUrls("http://localhost:5000", "https://localhost:5001", "http://*:5000", "https://*:5001");
    app.UseSwagger();
    app.UseSwaggerUI();
}
//habilita el cors
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());


//se usa para habilitar la redirección HTTPS - comentada para pobrar desde el movil
app.UseHttpsRedirection();

app.UseRouting();    //nuevo

// Habilitar la redireccion HTTPS
app.UseHttpsRedirection();

// Habilitar autenticacion
app.UseAuthentication();

// Habilitar autorizacion
app.UseAuthorization();

// Habilitar el uso de archivos esaticos
app.UseStaticFiles();

// Mapear los controladores
//app.MapControllers();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers(); 
});

// Ejecutar la aplicación
app.Run();




/*var builder = WebApplication.CreateBuilder(args);

// Configurar URLs donde se ejecutará el proyecto
builder.WebHost.UseUrls("http://localhost:5000", "https://localhost:5001");

// Agregar servicios al contenedor
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Obtener la cadena de conexión desde appsettings.json
var connectionString = builder.Configuration.GetConnectionString("Mysql");

// Agregar el DbContext con la cadena de conexión
builder.Services.AddDbContext<DataContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Agregar servicios personalizados
builder.Services.AddScoped<RepositorioInquilino>();
builder.Services.AddScoped<RepositorioInmueble>();
builder.Services.AddScoped<RepositorioPropietario>();
builder.Services.AddScoped<RepositorioContrato>();
builder.Services.AddScoped<RepositorioPago>();
builder.Services.AddScoped<RepositorioTipoInmuebles>();
builder.Services.AddScoped<Auth>();
builder.Services.AddScoped<EmailSender>();
builder.Services.AddScoped<RepositorioUsuario>();

// Configurar autenticación JWT
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience))
{
    Console.WriteLine("Error: Clave, emisor o audiencia del token JWT no configurados correctamente");
}
else
{
    var key = Encoding.UTF8.GetBytes(jwtKey);

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtIssuer,
                ValidAudience = jwtAudience,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        });
}

// Configuración de autorización con roles
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Propietario", policy => policy.RequireRole("Propietario"));
});

var app = builder.Build();

// Configuración del pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Habilitar CORS
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());*/

