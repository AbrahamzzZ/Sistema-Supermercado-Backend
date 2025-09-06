using DataBaseFirst.Models;
using Utilities.Shared;

namespace Infrastructure.Repository.InterfacesServices
{
    public interface IProveedorService
    {
        Task<ApiResponse<List<Proveedor>>> ListarProveedoresAsync();
        Task<ApiResponse<Paginacion<Proveedor>>> ListarProveedoresPaginacionAsync(int pageNumber, int pageSize);
        Task<ApiResponse<Proveedor>> ObtenerProveedorAsync(int idProveedor);
        Task<ApiResponse<object>> RegistrarProveedorAsync(Proveedor proveedor);
        Task<ApiResponse<object>> EditarProveedorAsync(Proveedor proveedor);
        Task<ApiResponse<int>> EliminarProveedorAsync(int idProveedor);
    }
}
