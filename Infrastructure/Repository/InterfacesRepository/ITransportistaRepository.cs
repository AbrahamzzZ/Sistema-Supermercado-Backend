using Domain.Models;
using Utilities.Shared;

namespace Infrastructure.Repository.InterfacesRepository
{
    public interface ITransportistaRepository
    {
        Task<List<Transportistum>> ListarTransportistasAsync();
        Task<Paginacion<Transportistum>> ListarTransportistasPaginacionAsync(int pageNumber, int pageSize);
        Task<Transportistum?> ObtenerTransportistaAsync(int idTranportista);
        Task<int> RegistrarTransportistaAsync(Transportistum transportista);
        Task<int> EditarTransportistaAsync(Transportistum transportista);
        Task<int> EliminarTransportistaAsync(int idTransportista);
    }
}
