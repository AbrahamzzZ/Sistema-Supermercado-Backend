using Domain.Models;
using Utilities.Shared;

namespace Infrastructure.Repository.InterfacesRepository
{
    public interface ILogRepository
    {
        Task<Paginacion<Log>> ListarLogsPaginacionAsync(int pageNumber, int pageSize);
        Task<Log?> ObtenerLogAsync(int idLog);
        Task<int> RegistrarLogAsync(Log log);
    }
}
