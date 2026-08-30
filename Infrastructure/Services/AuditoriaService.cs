using Domain.Models;
using Infrastructure.Repository.InterfacesRepository;
using Infrastructure.Repository.InterfacesServices;
using Utilities.Shared;

namespace Infrastructure.Services
{
    public class AuditoriaService : IAuditoriaService
    {
        private readonly ILogRepository _logRepository;

        public AuditoriaService(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        // Registra un log de operación exitosa
        public async Task RegistrarExitoAsync(string operacion, string descripcion, int? idUsuario, string endpoint, string metodo)
        {
            var log = new Log
            {
                Codigo_Error = $"OK-{DateTime.Now:yyyyMMddHHmmss}",
                Mensaje_Error = $"Operación exitosa: {operacion}",
                Detalle_Error = descripcion,
                Endpoint = endpoint,
                Metodo = metodo,
                Nivel = "INFO",
                Id_Usuario = idUsuario,
                Fecha = DateTime.Now
            };

            await _logRepository.RegistrarLogAsync(log);
        }

        // Registra un log de operación fallida (validación, reglas de negocio)
        public async Task RegistrarFalloAsync(string operacion, string razon, int? idUsuario, string endpoint, string metodo)
        {
            var log = new Log
            {
                Codigo_Error = $"FAIL-{DateTime.Now:yyyyMMddHHmmss}",
                Mensaje_Error = $"Operación fallida: {operacion}",
                Detalle_Error = razon,
                Endpoint = endpoint,
                Metodo = metodo,
                Nivel = "WARNING",
                Id_Usuario = idUsuario,
                Fecha = DateTime.Now
            };

            await _logRepository.RegistrarLogAsync(log);
        }

        // Registra un log de error del sistema (excepciones)
        public async Task RegistrarErrorAsync(string operacion, Exception ex, int? idUsuario, string endpoint, string metodo)
        {
            var log = new Log
            {
                Codigo_Error = $"ERR-{DateTime.Now:yyyyMMddHHmmss}",
                Mensaje_Error = $"Error del sistema: {operacion}",
                Detalle_Error = ex.ToString(),
                Endpoint = endpoint,
                Metodo = metodo,
                Nivel = "ERROR",
                Id_Usuario = idUsuario,
                Fecha = DateTime.Now
            };

            await _logRepository.RegistrarLogAsync(log);
        }
    }
}
