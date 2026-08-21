using Domain.Models.Dto.Venta;
using Utilities.Shared;

namespace Infrastructure.Repository.InterfacesServices
{
    public interface IVentaService
    {
        Task<ApiResponse<string>> ObtenerNumeroDocumentoAsync();
        Task<ApiResponse<VentaResponse>> ObtenerVentaAsync(string numeroDocumento);
        Task<ApiResponse<List<DetalleVentaReponse>>> ObtenerDetallesVentaAsync(int idCompra);
        Task<ApiResponse<object>> RegistrarVentaAsync(Ventas ventaDto);
    }
}
