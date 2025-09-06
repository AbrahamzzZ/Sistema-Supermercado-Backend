using DataBaseFirst.Models;
using Utilities.Shared;

namespace Infrastructure.Repository.InterfacesServices
{
    public interface IClienteService
    {
        Task<ApiResponse<List<Cliente>>> ListarClientesAsync();
        Task<ApiResponse<Paginacion<Cliente>>> ListarClientesPaginacionAsync(int pageNumber, int pageSize);
        Task<ApiResponse<Cliente>> ObtenerClienteAsync(int idCliente);
        Task<ApiResponse<object>> RegistrarClienteAsync(Cliente cliente);
        Task<ApiResponse<object>> EditarClienteAsync(Cliente cliente);
        Task<ApiResponse<int>> EliminarClienteAsync(int id);
    }
}
