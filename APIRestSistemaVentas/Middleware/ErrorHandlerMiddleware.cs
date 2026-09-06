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
            var auditoriaService = context.RequestServices.GetRequiredService<IAuditoriaService>();

            await auditoriaService.RegistrarErrorAsync(
                context.Request.Path.ToString(),
                ex
            );

            var response = new ApiResponse<object>
            {
                IsSuccess = false,
                Message = "Ha ocurrido un error inesperado.",
                Data = new { errorCode = $"ERR-{DateTime.Now:yyyyMMddHHmmss}" }
            };

            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
