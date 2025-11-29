using Domain.Models;
using Domain.Models.Dto.Negocio;

namespace Infrastructure.Repository.InterfacesRepository
{
    public interface INegocioRepository
    {
        Task<Negocio?> ObtenerNegocioAsync(int idNegocio);
        Task<int> EditarNegocioAsync(Negocio negocio);
        Task<List<ProductoMasComprado>> ObtenerProductoMasComprado();
        Task<List<ProductoMasVendido>> ObtenerProductoMasVendido();
        Task<List<TopCliente>> ObtenerTopClientes();
        Task<List<TopProveedor>> ObtenerTopProveedores();
        Task<List<ViajesTransportista>> ObtenerViajesTransportista();
        Task<List<EmpleadoProductivo>> ObtenerEmpleadosProductivos();
    }
}
