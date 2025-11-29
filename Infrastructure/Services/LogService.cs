using Domain.Models;
using Infrastructure.Repository;
using Infrastructure.Repository.InterfacesRepository;
using Infrastructure.Repository.InterfacesServices;
using Utilities.Shared;

namespace Infrastructure.Services
{
    public class LogService : ILogService
    {
        private readonly ILogRepository _logRepository;

        public LogService(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public async Task<ApiResponse<Paginacion<Log>>> ListarLogsPaginacionAsync(int pageNumber, int pageSize)
        {
            var pagedResult = await _logRepository.ListarLogsPaginacionAsync(pageNumber, pageSize);

            if (pagedResult.Items == null || pagedResult.Items.Count == 0)
            {
                return new ApiResponse<Paginacion<Log>> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY, Data = pagedResult };
            }

            return new ApiResponse<Paginacion<Log>> { IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = pagedResult };
        }

        public async Task<ApiResponse<Log>> ObtenerLogAsync(int idLog)
        {
            var log = await _logRepository.ObtenerLogAsync(idLog);

            if (log == null)
            {
                return new ApiResponse<Log> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };
            }

            return new ApiResponse<Log> { IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = log };
        }

        public async Task<ApiResponse<object>> RegistrarLogAsync(Log log)
        {
       

            var result = await _logRepository.RegistrarLogAsync(log);
            if (result > 0)
                return new ApiResponse<object> { IsSuccess = true, Message = Mensajes.MESSAGE_REGISTER };

            return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_REGISTER_FAILLED };
        }
    }
}
