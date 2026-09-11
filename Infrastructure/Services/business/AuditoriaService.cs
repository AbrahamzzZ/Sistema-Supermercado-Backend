using Domain.Models;
using Infrastructure.Repository.InterfacesBusiness;
using Infrastructure.Repository.InterfacesRepository;
using Infrastructure.Repository.InterfacesServices;

namespace Infrastructure.Services.business
{
    public class AuditoriaService : IAuditoriaService
    {
        private readonly ILogRepository _logRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IRequestContext _requestContext;

        public AuditoriaService(ILogRepository logRepository, ICurrentUser currentUserService, IRequestContext requestContext)
        {
            _logRepository = logRepository;
            _currentUser = currentUserService;
            _requestContext = requestContext;
        }

        // Registra un log de operación exitosa
        public async Task RegistrarExitoAsync(string operacion, string descripcion)
        {
            var log = new Log
            {
                Codigo = $"OK-{DateTime.Now:yyyyMMddHHmmss}",
                Mensaje = $"Operación exitosa: {operacion}",
                Detalle = descripcion,
                Endpoint = _requestContext.GetEndpoint(),
                Metodo = _requestContext.GetMethod(),
                Nivel = "INFO",
                Id_Usuario = _currentUser.GetUserId(),
                Fecha = DateTime.Now
            };

            try
            {
                await _logRepository.RegistrarLogAsync(log);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AUDIT] No se registró log de éxito: {ex.Message}");
            }
        }

        // Registra un log de operación fallida (validación, reglas de negocio)
        public async Task RegistrarFalloAsync(string operacion, string razon)
        {
            var log = new Log
            {
                Codigo = $"FAIL-{DateTime.Now:yyyyMMddHHmmss}",
                Mensaje = $"Operación fallida: {operacion}",
                Detalle = razon,
                Endpoint = _requestContext.GetEndpoint(),
                Metodo = _requestContext.GetMethod(),
                Nivel = "WARNING",
                Id_Usuario = _currentUser.GetUserId(),
                Fecha = DateTime.Now
            };

            try
            {
                await _logRepository.RegistrarLogAsync(log);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AUDIT] No se registró log de fallo: {ex.Message}");
            }
        }

        // Registra un log de error del sistema (excepciones)
        public async Task RegistrarErrorAsync(string operacion, Exception ex)
        {
            var log = new Log
            {
                Codigo = $"ERR-{DateTime.Now:yyyyMMddHHmmss}",
                Mensaje = $"Error del sistema: {operacion}",
                Detalle = ex.ToString(),
                Endpoint = _requestContext.GetEndpoint(),
                Metodo = _requestContext.GetMethod(),
                Nivel = "ERROR",
                Id_Usuario = _currentUser.GetUserId(),
                Fecha = DateTime.Now
            };

            try
            {
                await _logRepository.RegistrarLogAsync(log);
            }
            catch (Exception logEx)
            {
                Console.WriteLine($"[AUDIT] No se registró log de error: {logEx.Message}");
            }
        }
    }
}
