using APIRestSistemaVentas.Middleware;
using APIRestSistemaVentas.Security;
using Domain.Contexts;
using Infrastructure.Extensions;
using Infrastructure.Helpers;
using Infrastructure.Repository.InterfacesBusiness;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Clave JWT
var jwtSettings = builder.Configuration.GetSection("Jwt");
var claveSecreta = jwtSettings.GetValue<string>("Key");
if (string.IsNullOrWhiteSpace(claveSecreta))
    throw new InvalidOperationException("Falta la configuración 'Jwt:Key'. En local: dotnet user-secrets set \"Jwt:Key\" \"<clave>\". En Docker: variable JWT_KEY en el archivo .env.");

// Base de datos
var connectionString = builder.Configuration.GetConnectionString("CadenaSQL");
if (string.IsNullOrWhiteSpace(connectionString))
    throw new InvalidOperationException("Falta la cadena de conexión 'ConnectionStrings:CadenaSQL'. En local: appsettings.Development.json. En Docker: variable DB_CONNECTION_STRING en el archivo .env.");

builder.Services.AddDbContext<SistemaSupermercadoContext>(options =>
{
    options.UseSqlServer(connectionString);
});

// Inyección de dependencias separada en métodos de extensión
builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddValidators();
builder.Services.AddSingleton<IToken, Token>();
builder.Services.AddOllamaClient(builder.Configuration);

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

    // Un token válido no basta: el usuario debe seguir activo y con el mismo rol (efecto en ~1 minuto)
    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = async context =>
        {
            var principal = context.Principal;
            int.TryParse(principal?.FindFirstValue(ClaimTypes.NameIdentifier), out var idUsuario);
            var rol = principal?.FindFirstValue(ClaimTypes.Role);

            var verificacion = context.HttpContext.RequestServices.GetRequiredService<IVerificacionUsuario>();
            if (!await verificacion.EsSesionValidaAsync(idUsuario, rol))
                context.Fail("El usuario está inactivo o sus permisos cambiaron. Inicie sesión nuevamente.");
        }
    };
});

builder.Services.AddMemoryCache();

// Límite de peticiones por usuario / IP (ver Seguridad/LimitePeticiones.cs)
builder.Services.AddLimitePeticiones(builder.Configuration);

// Agregar servicios para controladores
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

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
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "API Supermercado", Version = "v1" });
    options.EnableAnnotations();

    // Configuración de seguridad JWT
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Ingrese el token JWT en el campo de texto: **Bearer {su token}**"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Configurar middleware y pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("NuevaPolitica");

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseRateLimiter();

app.MapControllers().RequireRateLimiting(LimitePeticiones.Politica);

app.Run();
