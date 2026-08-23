using Domain.Models;
using Domain.Models.Dto.Response.Cliente;
using Utilities.Shared;

namespace Infrastructure.Repository.InterfacesRepository
{
    public interface IClienteRepository
    {
        Task<List<ClienteResponse>> ListarClientesAsync();
        Task<Paginacion<Cliente>> ListarClientesPaginacionAsync(int pageNumber, int pageSize);
        Task<Cliente?> ObtenerClienteAsync(int idCliente);
        Task<int> RegistrarClienteAsync(Cliente cliente, int UsuarioCreacion);
        Task<int> EditarClienteAsync(Cliente cliente);
        Task<int> EliminarClienteAsync(int id);
    }
}
