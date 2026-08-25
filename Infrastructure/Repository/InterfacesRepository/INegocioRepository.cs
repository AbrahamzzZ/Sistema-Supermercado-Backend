using Domain.Models;
using Domain.Models.Dto.Response.Negocio;
using Domain.Models.Dto.Response.Negocio.IA;


namespace Infrastructure.Repository.InterfacesRepository
{
    public interface INegocioRepository
    {
        Task<Negocio?> ObtenerNegocioAsync(int idNegocio);
        Task<int> EditarNegocioAsync(Negocio negocio, int usuarioModificacion);
        Task<List<ProductoMasCompradoResponse>> ObtenerProductoMasComprado();
        Task<List<ProductoMasVendidoResponse>> ObtenerProductoMasVendido();
        Task<List<ProductoMasCompradoAnalisisIA>> ObtenerAnalisisProductosComprados();
        Task<List<ProductoMasVendidoAnalisisIA>> ObtenerAnalisisProductosVendidos();
        Task<List<TopClienteResponse>> ObtenerTopClientes();
        Task<List<TopProveedorResponse>> ObtenerTopProveedores();
        Task<List<ViajesTransportistaResponse>> ObtenerViajesTransportista();
        Task<List<EmpleadoProductivoResponse>> ObtenerEmpleadosProductivos();
    }
}
