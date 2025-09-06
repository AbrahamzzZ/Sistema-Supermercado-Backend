using DataBaseFirst.Models;
using DataBaseFirst.Models.Dto;
using Utilities.Shared;

namespace Infrastructure.Repository.InterfacesServices
{
    public interface IOfertaService
    {
        Task<ApiResponse<List<OfertaProducto>>> ListarOfertasAsync();
        Task<ApiResponse<Paginacion<OfertaProducto>>> ListarOfertasPaginacionAsync(int pageNumber, int pageSize);
        Task<ApiResponse<Ofertum>> ObtenerOfertaAsync(int idOferta);
        Task<ApiResponse<object>> RegistrarOfertaAsync(Ofertum oferta);
        Task<ApiResponse<object>> EditarOfertaAsync(Ofertum oferta);
        Task<ApiResponse<int>> EliminarOfertaAsync(int idOferta);
    }
}
