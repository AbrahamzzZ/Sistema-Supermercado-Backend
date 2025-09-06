using DataBaseFirst.Models;
using DataBaseFirst.Models.Dto;
using Utilities.Shared;

namespace Infrastructure.Repository.InterfacesServices
{
    public interface IProductoService
    {
        Task<ApiResponse<List<ProductoCategoria>>> ListarProductosAsync();
        Task<ApiResponse<Paginacion<ProductoCategoria>>> ListarProductosPaginacionAsync(int pageNumber, int pageSize);
        Task<ApiResponse<ProductoRespuesta>> ObtenerProductoAsync(int idProducto);
        Task<ApiResponse<object>> RegistrarProductoAsync(Producto producto);
        Task<ApiResponse<object>> EditarProductoAsync(Producto producto);
        Task<ApiResponse<int>> EliminarProductoAsync(int idProducto);
    }
}
