using DataBaseFirst.Models;
using Utilities.Shared;

namespace DataBaseFirst.Repository.InterfacesServices
{
    public interface ITransportistaService
    {
        Task<ApiResponse<List<Transportistum>>> ListarTransportistasAsync();
        Task<ApiResponse<Paginacion<Transportistum>>> ListarTransportistasPaginacionAsync(int pageNumber, int pageSize);
        Task<ApiResponse<Transportistum>> ObtenerTransportistaAsync(int idTranportista);
        Task<ApiResponse<object>> RegistrarTransportistaAsync(Transportistum transportista);
        Task<ApiResponse<object>> EditarTransportistaAsync(Transportistum transportista);
        Task<ApiResponse<int>> EliminarTransportistaAsync(int idTransportista);
    }
}
