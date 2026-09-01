using Domain.Models;
using Domain.Models.Dto.Response.Producto;
using Utilities.Shared;

namespace Infrastructure.Repository.InterfacesRepository
{
    public interface IProductoRepository
    {
        Task<List<ProductoCategoriaResponse>> ListarProductosAsync();
        Task<Paginacion<ProductoCategoriaResponse>> ListarProductosPaginacionAsync(int pageNumber, int pageSize, string filtro);
        Task<ProductoResponse?> ObtenerProductoAsync(int idProducto);
        Task<int> RegistrarProductoAsync(Producto producto, int usuarioCreacion);
        Task<int> EditarProductoAsync(Producto producto, int usuarioModificacion);
        Task<int> EliminarProductoAsync(int idProducto);
    }
}
