using Domain.Models;
using Domain.Models.Dto.Negocio;
using Utilities.Shared;

namespace Infrastructure.Repository.InterfacesServices
{
    public interface INegocioService
    {
        Task<ApiResponse<Negocio>> ObtenerNegocioAsync(int idNegocio);
        Task<ApiResponse<object>> EditarNegocioAsync(Negocio negocio);
        Task<ApiResponse<List<ProductoMasComprado>>> ObtenerProductoMasComprado();
        Task<ApiResponse<List<ProductoMasVendido>>> ObtenerProductoMasVendido();
        Task<ApiResponse<List<TopCliente>>> ObtenerTopClientes();
        Task<ApiResponse<List<TopProveedor>>> ObtenerTopProveedores();
        Task<ApiResponse<List<ViajesTransportista>>> ObtenerViajesTransportista();
        Task<ApiResponse<List<EmpleadoProductivo>>> ObtenerEmpleadosProductivos();
    }
}
