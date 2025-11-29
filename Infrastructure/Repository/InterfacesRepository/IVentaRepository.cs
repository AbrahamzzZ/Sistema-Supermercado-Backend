using Domain.Models.Dto.Venta;

namespace Infrastructure.Repository.InterfacesRepository
{
    public interface IVentaRepository
    {
        Task<string> ObtenerNumeroDocumentoAsync();
        Task<VentaRespuesta?> ObtenerVentaAsync(string numeroDocumento);
        Task<List<DetalleVentasRepuesta>> ObtenerDetallesVentaAsync(int idCompra);
        Task<bool> RegistrarVentaAsync(Ventas ventaDto);
    }
}
