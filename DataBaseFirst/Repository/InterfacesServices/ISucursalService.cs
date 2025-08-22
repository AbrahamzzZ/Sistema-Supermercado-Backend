using DataBaseFirst.Models;
using Utilities.Shared;

namespace DataBaseFirst.Repository.InterfacesServices
{
    public interface ISucursalService
    {
        Task<ApiResponse<List<Sucursal>>> ListarSucursalesAsync();
        Task<ApiResponse<Paginacion<Sucursal>>> ListarSucursalesPaginacionAsync(int pageNumber, int pageSize);
        Task<ApiResponse<Sucursal>> ObtenerSucursalAsync(int idSucursal);
        Task<ApiResponse<object>> RegistrarSucursalAsync(Sucursal sucursal);
        Task<ApiResponse<object>> EditarSucursalAsync(Sucursal sucursal);
        Task<ApiResponse<int>> EliminarSucursalAsync(int idSucursal);
    }
}
