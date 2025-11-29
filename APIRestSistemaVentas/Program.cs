using APIRestSistemaVentas.Middleware;
using Domain.Contexts;
using Infrastructure.Extensions;
using Infrastructure.Helpers;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Utilities.IA;

var builder = WebApplication.CreateBuilder(args);

// Configuración de clave JWT
var jwtSettings = builder.Configuration.GetSection("Jwt");
var claveSecreta = jwtSettings.GetValue<string>("Key");

// Agregar DbContext con la cadena de conexión del appsettings.json
builder.Services.AddDbContext<SistemaSupermercadoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CadenaSQL"))
);

// Inyección de dependencias separada en métodos de extensión
builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddValidators();
builder.Services.AddSingleton<Token>();
builder.Services.AddHttpClient<OllamaClient>(client =>
{
    client.BaseAddress = new Uri("http://localhost:11434");
    client.Timeout = Timeout.InfiniteTimeSpan; 
});

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

// Configuración de Rate Limiting
builder.Services.AddRateLimiter(option =>
{
    option.RejectionStatusCode = 
    StatusCodes.Status429TooManyRequests;

    option.AddFixedWindowLimiter("fijo", opt =>
    {
        opt.PermitLimit = 15;
        opt.Window = TimeSpan.FromSeconds(30);
        opt.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 2;
    });
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
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "API Supermercado", Version = "v1" });

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

app.MapControllers().RequireRateLimiting("fijo");

app.Run();
