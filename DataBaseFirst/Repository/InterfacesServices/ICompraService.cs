using DataBaseFirst.Models.Dto;
using Utilities.Shared;

namespace DataBaseFirst.Repository.InterfacesServices
{
    public interface ICompraService
    {
        Task<ApiResponse<string>> ObtenerNumeroDocumentoAsync();
        Task<ApiResponse<CompraRespuesta>> ObtenerCompraAsync(string numeroDocumento);
        Task<List<DetalleComprasRepuesta>> ObtenerDetallesCompraAsync(int idCompra);
        Task<ApiResponse<object>> RegistrarCompraAsync(Compras compraDto);
    }
}
