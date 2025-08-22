using DataBaseFirst.Models;
using DataBaseFirst.Models.Dto;
using Utilities.Shared;

namespace DataBaseFirst.Repository.InterfacesRepository
{
    public interface IOfertaRepository
    {
        Task<List<OfertaProducto>> ListarOfertasAsync();
        Task<Paginacion<OfertaProducto>> ListarOfertasPaginacionAsync(int pageNumber, int pageSize);
        Task<Ofertum?> ObtenerOfertaAsync(int idOferta);
        Task<int> RegistrarOfertaAsync(Ofertum oferta);
        Task<int> EditarOfertaAsync(Ofertum oferta);
        Task<int> EliminarOfertaAsync(int idOferta);
    }
}
