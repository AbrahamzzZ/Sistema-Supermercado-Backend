using Domain.Models;
using Utilities.Shared;

namespace Infrastructure.Repository.InterfacesServices
{
    public interface ILogService
    {
        Task<ApiResponse<Paginacion<Log>>> ListarLogsPaginacionAsync(int pageNumber, int pageSize);
        Task<ApiResponse<Log>> ObtenerLogAsync(int idLog);
        Task<ApiResponse<object>> RegistrarLogAsync(Log log);
    }
}
