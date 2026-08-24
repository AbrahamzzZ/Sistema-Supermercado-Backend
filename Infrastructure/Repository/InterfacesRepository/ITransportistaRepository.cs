using Domain.Models;
using Domain.Models.Dto.Response.Transportista;
using Utilities.Shared;

namespace Infrastructure.Repository.InterfacesRepository
{
    public interface ITransportistaRepository
    {
        Task<List<TransportistaResponse>> ListarTransportistasAsync();
        Task<Paginacion<TransportistaResponse>> ListarTransportistasPaginacionAsync(int pageNumber, int pageSize);
        Task<TransportistaResponse?> ObtenerTransportistaAsync(int idTranportista);
        Task<int> RegistrarTransportistaAsync(Transportistum transportista, int usuarioCreacion);
        Task<int> EditarTransportistaAsync(Transportistum transportista);
        Task<int> EliminarTransportistaAsync(int idTransportista);
    }
}
