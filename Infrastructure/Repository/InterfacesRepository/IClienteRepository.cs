using Domain.Models;
using Domain.Models.Dto.Response.Cliente;
using Utilities.Shared;

namespace Infrastructure.Repository.InterfacesRepository
{
    public interface IClienteRepository
    {
        Task<List<ClienteResponse>> ListarClientesAsync();
        Task<Paginacion<ClienteResponse>> ListarClientesPaginacionAsync(int pageNumber, int pageSize);
        Task<ClienteResponse?> ObtenerClienteAsync(int idCliente);
        Task<int> RegistrarClienteAsync(Cliente cliente, int usuarioCreacion);
        Task<int> EditarClienteAsync(Cliente cliente, int usuarioModificacion);
        Task<int> EliminarClienteAsync(int id);
    }
}
