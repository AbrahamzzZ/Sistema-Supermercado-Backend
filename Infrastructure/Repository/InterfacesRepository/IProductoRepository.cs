using Domain.Models;
using Domain.Models.Dto.Response.Producto;
using Utilities.Shared;

namespace Infrastructure.Repository.InterfacesRepository
{
    public interface IProductoRepository
    {
        Task<List<ProductoCategoriaResponse>> ListarProductosAsync();
        Task<Paginacion<ProductoCategoriaResponse>> ListarProductosPaginacionAsync(int pageNumber, int pageSize);
        Task<ProductoResponse?> ObtenerProductoAsync(int idProducto);
        Task<int> RegistrarProductoAsync(Producto producto);
        Task<int> EditarProductoAsync(Producto producto);
        Task<int> EliminarProductoAsync(int idProducto);
    }
}
