using DataBaseFirst.Models.Dto;

namespace DataBaseFirst.Repository.InterfacesRepository
{
    public interface IVentaRepository
    {
        Task<string> ObtenerNumeroDocumentoAsync();
        Task<VentaRespuesta?> ObtenerVentaAsync(string numeroDocumento);
        Task<List<DetalleVentasRepuesta>> ObtenerDetallesVentaAsync(int idCompra);
        Task<bool> RegistrarVentaAsync(Ventas ventaDto);
    }
}
