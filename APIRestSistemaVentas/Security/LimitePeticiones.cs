using System.Security.Claims;
using System.Text.Json;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using Utilities.Shared;

namespace APIRestSistemaVentas.Security
{
    public static class LimitePeticiones
    {
        public const string Politica = "api";
        private const string RutaLogin = "/api/usuario/login";

        public static IServiceCollection AddLimitePeticiones(this IServiceCollection services, IConfiguration configuration)
        {
            var seccion = configuration.GetSection("LimitePeticiones");
            var porUsuario = seccion.GetValue("PorUsuarioPorMinuto", 100);
            var anonimo = seccion.GetValue("AnonimoPorMinuto", 30);
            var login = seccion.GetValue("LoginPorMinuto", 5);

            services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
                options.OnRejected = ResponderLimiteExcedidoAsync;

                options.AddPolicy(Politica, httpContext =>
                {
                    var particion = ObtenerParticion(httpContext);

                    if (particion.StartsWith("login:", StringComparison.Ordinal))
                    {
                        return RateLimitPartition.GetFixedWindowLimiter(particion, _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = login,
                            Window = TimeSpan.FromMinutes(1),
                            QueueLimit = 0
                        });
                    }

                    if (particion.StartsWith("usuario:", StringComparison.Ordinal))
                    {
                        var recuperadasCada10s = Math.Max(1, porUsuario / 6);
                        return RateLimitPartition.GetTokenBucketLimiter(particion, _ => new TokenBucketRateLimiterOptions
                        {
                            TokenLimit = porUsuario,
                            TokensPerPeriod = recuperadasCada10s,
                            ReplenishmentPeriod = TimeSpan.FromSeconds(10),
                            QueueLimit = Math.Min(10, recuperadasCada10s),
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            AutoReplenishment = true
                        });
                    }

                    return RateLimitPartition.GetSlidingWindowLimiter(particion, _ => new SlidingWindowRateLimiterOptions
                    {
                        PermitLimit = anonimo,
                        Window = TimeSpan.FromMinutes(1),
                        SegmentsPerWindow = 6,
                        QueueLimit = 0
                    });
                });
            });

            return services;
        }

        public static string ObtenerParticion(HttpContext httpContext)
        {
            var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "desconocida";

            if (string.Equals(httpContext.Request.Path.Value?.TrimEnd('/'), RutaLogin, StringComparison.OrdinalIgnoreCase))
                return $"login:{ip}";

            var idUsuario = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (httpContext.User.Identity?.IsAuthenticated == true && !string.IsNullOrEmpty(idUsuario))
                return $"usuario:{idUsuario}";

            return $"ip:{ip}";
        }

        private static async ValueTask ResponderLimiteExcedidoAsync(OnRejectedContext context, CancellationToken cancellationToken)
        {
            var segundos = 60;
            if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                segundos = Math.Max(1, (int)Math.Ceiling(retryAfter.TotalSeconds));

            var response = context.HttpContext.Response;
            response.Headers.RetryAfter = segundos.ToString();
            response.ContentType = "application/json";

            var cuerpo = new ApiResponse<object>
            {
                IsSuccess = false,
                Message = $"Demasiadas solicitudes. Espere {segundos} segundos e intente nuevamente."
            };
            await response.WriteAsync(JsonSerializer.Serialize(cuerpo, new JsonSerializerOptions(JsonSerializerDefaults.Web)), cancellationToken);
        }
    }
}
