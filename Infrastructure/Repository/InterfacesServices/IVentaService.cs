using Domain.Models.Dto.Response.Venta;
using Utilities.Shared;

namespace Infrastructure.Repository.InterfacesServices
{
    public interface IVentaService
    {
        Task<ApiResponse<string>> ObtenerNumeroDocumentoAsync();
        Task<ApiResponse<VentaResponse>> ObtenerVentaAsync(string numeroDocumento);
        Task<ApiResponse<List<DetalleVentaReponse>>> ObtenerDetallesVentaAsync(int idCompra);
        Task<ApiResponse<VentaRegistradaResponse>> RegistrarVentaAsync(Ventas ventaDto);
    }
}
