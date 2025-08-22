using DataBaseFirst.Models.Dto;
using Utilities.Shared;

namespace DataBaseFirst.Repository.InterfacesServices
{
    public interface IVentaService
    {
        Task<ApiResponse<string>> ObtenerNumeroDocumentoAsync();
        Task<ApiResponse<VentaRespuesta>> ObtenerVentaAsync(string numeroDocumento);
        Task<List<DetalleVentasRepuesta>> ObtenerDetallesVentaAsync(int idCompra);
        Task<ApiResponse<object>> RegistrarVentaAsync(Ventas ventaDto);
    }
}
