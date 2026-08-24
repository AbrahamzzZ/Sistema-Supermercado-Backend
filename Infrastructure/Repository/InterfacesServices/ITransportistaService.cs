using Domain.Models;
using Domain.Models.Dto.Response.Transportista;
using Utilities.Shared;

namespace Infrastructure.Repository.InterfacesServices
{
    public interface ITransportistaService
    {
        Task<ApiResponse<List<TransportistaResponse>>> ListarTransportistasAsync();
        Task<ApiResponse<Paginacion<TransportistaResponse>>> ListarTransportistasPaginacionAsync(int pageNumber, int pageSize);
        Task<ApiResponse<TransportistaResponse>> ObtenerTransportistaAsync(int idTranportista);
        Task<ApiResponse<object>> RegistrarTransportistaAsync(Transportistum transportista);
        Task<ApiResponse<object>> EditarTransportistaAsync(Transportistum transportista);
        Task<ApiResponse<int>> EliminarTransportistaAsync(int idTransportista);
    }
}
