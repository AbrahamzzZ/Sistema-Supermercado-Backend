using Domain.Models;
using Domain.Models.Dto.Response.Transportista;
using Utilities.Shared;

namespace Infrastructure.Repository.InterfacesRepository
{
    public interface ITransportistaRepository
    {
        Task<List<TransportistaResponse>> ListarTransportistasAsync();
        Task<Paginacion<Transportistum>> ListarTransportistasPaginacionAsync(int pageNumber, int pageSize);
        Task<Transportistum?> ObtenerTransportistaAsync(int idTranportista);
        Task<int> RegistrarTransportistaAsync(Transportistum transportista, int usuarioCreacion);
        Task<int> EditarTransportistaAsync(Transportistum transportista);
        Task<int> EliminarTransportistaAsync(int idTransportista);
    }
}
