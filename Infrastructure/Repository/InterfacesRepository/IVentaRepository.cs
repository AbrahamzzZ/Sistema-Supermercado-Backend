using Domain.Models.Dto.Response.Venta;

namespace Infrastructure.Repository.InterfacesRepository
{
    public interface IVentaRepository
    {
        Task<string> ObtenerNumeroDocumentoAsync();
        Task<VentaResponse?> ObtenerVentaAsync(string numeroDocumento);
        Task<List<DetalleVentaReponse>> ObtenerDetallesVentaAsync(int idCompra);
        Task<bool> RegistrarVentaAsync(Ventas ventaDto);
    }
}
