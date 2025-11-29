using Domain.Models;
using Utilities.Shared;

namespace Infrastructure.Repository.InterfacesRepository
{
    public interface IClienteRepository
    {
        Task<List<Cliente>> ListarClientesAsync();
        Task<Paginacion<Cliente>> ListarClientesPaginacionAsync(int pageNumber, int pageSize);
        Task<Cliente?> ObtenerClienteAsync(int idCliente);
        Task<int> RegistrarClienteAsync(Cliente cliente);
        Task<int> EditarClienteAsync(Cliente cliente);
        Task<int> EliminarClienteAsync(int id);
    }
}
