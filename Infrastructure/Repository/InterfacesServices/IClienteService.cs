using Domain.Models;
using Domain.Models.Dto.Response.Cliente;
using Utilities.Shared;

namespace Infrastructure.Repository.InterfacesServices
{
    public interface IClienteService
    {
        Task<ApiResponse<List<ClienteResponse>>> ListarClientesAsync();
        Task<ApiResponse<Paginacion<ClienteResponse>>> ListarClientesPaginacionAsync(int pageNumber, int pageSize);
        Task<ApiResponse<ClienteResponse>> ObtenerClienteAsync(int idCliente);
        Task<ApiResponse<object>> RegistrarClienteAsync(Cliente cliente);
        Task<ApiResponse<object>> EditarClienteAsync(Cliente cliente);
        Task<ApiResponse<int>> EliminarClienteAsync(int id);
    }
}
