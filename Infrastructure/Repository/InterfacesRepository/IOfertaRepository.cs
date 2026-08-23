using Domain.Models;
using Domain.Models.Dto.Response.Oferta;
using Utilities.Shared;

namespace Infrastructure.Repository.InterfacesRepository
{
    public interface IOfertaRepository
    {
        Task<List<OfertaProductoResponse>> ListarOfertasAsync();
        Task<Paginacion<OfertaProductoResponse>> ListarOfertasPaginacionAsync(int pageNumber, int pageSize);
        Task<Ofertum?> ObtenerOfertaAsync(int idOferta);
        Task<int> RegistrarOfertaAsync(Ofertum oferta, int usuarioCreacion);
        Task<int> EditarOfertaAsync(Ofertum oferta);
        Task<int> EliminarOfertaAsync(int idOferta);
    }
}
