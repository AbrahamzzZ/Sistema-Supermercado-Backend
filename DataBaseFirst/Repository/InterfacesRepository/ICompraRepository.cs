using DataBaseFirst.Models.Dto;

namespace DataBaseFirst.Repository.InterfacesRepository
{
    public interface ICompraRepository
    {
        Task<string> ObtenerNumeroDocumentoAsync();
        Task<CompraRespuesta?> ObtenerCompraAsync(string numeroDocumento);
        Task<List<DetalleComprasRepuesta>> ObtenerDetallesCompraAsync(int idCompra);
        Task<bool> RegistrarCompraAsync(Compras compraDto);
    }
}
