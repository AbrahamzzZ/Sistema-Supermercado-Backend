using Domain.Models;
using Domain.Models.Dto.Response.Negocio;
using Utilities.Shared;

namespace Infrastructure.Repository.InterfacesServices
{
    public interface INegocioService
    {
        Task<ApiResponse<Negocio>> ObtenerNegocioAsync(int idNegocio);
        Task<ApiResponse<object>> EditarNegocioAsync(Negocio negocio);
        Task<ApiResponse<List<ProductoMasCompradoResponse>>> ObtenerProductoMasComprado();
        Task<ApiResponse<List<ProductoMasVendidoResponse>>> ObtenerProductoMasVendido();
        Task<ApiResponse<List<TopClienteResponse>>> ObtenerTopClientes();
        Task<ApiResponse<List<TopProveedorResponse>>> ObtenerTopProveedores();
        Task<ApiResponse<List<ViajesTransportistaResponse>>> ObtenerViajesTransportista();
        Task<ApiResponse<List<EmpleadoProductivoResponse>>> ObtenerEmpleadosProductivos();
    }
}
