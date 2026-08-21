using Domain.Models;
using Domain.Models.Dto.Response.Producto;
using Utilities.Shared;

namespace Infrastructure.Repository.InterfacesServices
{
    public interface IProductoService
    {
        Task<ApiResponse<List<ProductoCategoriaResponse>>> ListarProductosAsync();
        Task<ApiResponse<Paginacion<ProductoCategoriaResponse>>> ListarProductosPaginacionAsync(int pageNumber, int pageSize);
        Task<ApiResponse<ProductoResponse>> ObtenerProductoAsync(int idProducto);
        Task<ApiResponse<object>> RegistrarProductoAsync(Producto producto);
        Task<ApiResponse<object>> EditarProductoAsync(Producto producto);
        Task<ApiResponse<int>> EliminarProductoAsync(int idProducto);
    }
}
