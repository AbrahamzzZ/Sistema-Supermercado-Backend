using Domain.Models.Dto.Response.Compra;
using Utilities.Shared;

namespace Infrastructure.Repository.InterfacesServices
{
    public interface ICompraService
    {
        Task<ApiResponse<string>> ObtenerNumeroDocumentoAsync();
        Task<ApiResponse<CompraResponse>> ObtenerCompraAsync(string numeroDocumento);
        Task<ApiResponse<List<DetalleCompraReponse>>> ObtenerDetallesCompraAsync(int idCompra);
        Task<ApiResponse<object>> RegistrarCompraAsync(Compras compraDto);
    }
}
