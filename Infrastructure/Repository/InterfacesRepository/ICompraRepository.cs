using Domain.Models.Dto.Response.Compra;

namespace Infrastructure.Repository.InterfacesRepository
{
    public interface ICompraRepository
    {
        Task<string> ObtenerNumeroDocumentoAsync();
        Task<CompraResponse?> ObtenerCompraAsync(string numeroDocumento);
        Task<List<DetalleCompraReponse>> ObtenerDetallesCompraAsync(int idCompra);
        Task<bool> RegistrarCompraAsync(Compras compraDto);
    }
}
