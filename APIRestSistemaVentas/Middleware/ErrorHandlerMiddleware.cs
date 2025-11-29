using Domain.Models;
using Infrastructure.Repository.InterfacesServices;
using System.Security.Claims;
using System.Text.Json;
using Utilities.Shared;

namespace APIRestSistemaVentas.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
        }

        private async Task HandleException(HttpContext context, Exception ex)
        {
            var logService = context.RequestServices.GetRequiredService<ILogService>();

            string codigo = $"ERR-{DateTime.Now:yyyyMMddHHmmss}";

            var log = new Log
            {
                Codigo_Error = codigo,
                Mensaje_Error = ex.Message,
                Detalle_Error = ex.ToString(),
                Endpoint = context.Request.Path,
                Metodo = context.Request.Method,
                Nivel = "ERROR",
                Id_Usuario = ObtenerIdUsuario(context)
            };

            await logService.RegistrarLogAsync(log);

            var response = new ApiResponse<object>
            {
                IsSuccess = false,
                Message = "Ha ocurrido un error inesperado.",
                Data = new { errorCode = codigo }
            };

            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        private int? ObtenerIdUsuario(HttpContext context)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var claim = context.User.FindFirst(ClaimTypes.NameIdentifier);
                if (claim != null)
                    return int.Parse(claim.Value);
            }

            return null;
        }
    }
}
