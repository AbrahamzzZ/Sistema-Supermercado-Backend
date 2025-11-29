using Domain.Models.Dto.Compra;

namespace Infrastructure.Repository.InterfacesRepository
{
    public interface ICompraRepository
    {
        Task<string> ObtenerNumeroDocumentoAsync();
        Task<CompraRespuesta?> ObtenerCompraAsync(string numeroDocumento);
        Task<List<DetalleComprasRepuesta>> ObtenerDetallesCompraAsync(int idCompra);
        Task<bool> RegistrarCompraAsync(Compras compraDto);
    }
}
