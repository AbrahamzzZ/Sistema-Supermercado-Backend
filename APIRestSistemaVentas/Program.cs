using DataBaseFirst.Contexts;
using DataBaseFirst.Helpers;
using DataBaseFirst.Repository;
using DataBaseFirst.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configuración de clave JWT
var jwtSettings = builder.Configuration.GetSection("Jwt");
var claveSecreta = jwtSettings.GetValue<string>("Key");

// Agregar DbContext con la cadena de conexión del appsettings.json
builder.Services.AddDbContext<SistemaSupermercadoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CadenaSQL"))
);

// Registrar el servicio para inyección de dependencias
builder.Services.AddScoped<CategoriaRepository>();
builder.Services.AddScoped<CategoriaService>();

builder.Services.AddScoped<ClienteRepository>();
builder.Services.AddScoped<ClienteService>();

builder.Services.AddScoped<ProveedorRepository>();
builder.Services.AddScoped<ProveedorService>();

builder.Services.AddScoped<MenuService>();

builder.Services.AddScoped<RolRepository>();
builder.Services.AddScoped<RolService>();

builder.Services.AddScoped<TransportistaRepository>();
builder.Services.AddScoped<TransportistaService>();

builder.Services.AddScoped<UsuarioRepository>();
builder.Services.AddScoped<UsuarioService>();

builder.Services.AddScoped<NegocioRepository>();
builder.Services.AddScoped<NegocioService>();

builder.Services.AddScoped<ProductoRepository>();
builder.Services.AddScoped<ProductoService>();

builder.Services.AddScoped<OfertaRepository>();
builder.Services.AddScoped<OfertaService>();

builder.Services.AddScoped<SucursalRepository>();
builder.Services.AddScoped<SucursalService>();

builder.Services.AddScoped<CompraRepository>();
builder.Services.AddScoped<CompraService>();

builder.Services.AddScoped<VentaRepository>();
builder.Services.AddScoped<VentaService>(); 

builder.Services.AddSingleton<Token>();

// JWT Auth
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(claveSecreta))
    };
});

// Agregar servicios para controladores
builder.Services.AddControllers();

// Creacion de una nueva politica
builder.Services.AddCors(options =>
{
    options.AddPolicy("NuevaPolitica", app =>
    {
        app.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

// Agregar Swagger para documentación API  https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configurar middleware y pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("NuevaPolitica");

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
