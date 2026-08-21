using Domain.Models;
using Domain.Models.Dto.Response.Oferta;
using Utilities.Shared;

namespace Infrastructure.Repository.InterfacesServices
{
    public interface IOfertaService
    {
        Task<ApiResponse<List<OfertaProductoResponse>>> ListarOfertasAsync();
        Task<ApiResponse<Paginacion<OfertaProductoResponse>>> ListarOfertasPaginacionAsync(int pageNumber, int pageSize);
        Task<ApiResponse<Ofertum>> ObtenerOfertaAsync(int idOferta);
        Task<ApiResponse<object>> RegistrarOfertaAsync(Ofertum oferta);
        Task<ApiResponse<object>> EditarOfertaAsync(Ofertum oferta);
        Task<ApiResponse<int>> EliminarOfertaAsync(int idOferta);
    }
}
