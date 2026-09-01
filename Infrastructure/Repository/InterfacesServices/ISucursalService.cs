using Domain.Models;
using Domain.Models.Dto.Response.Sucursal;
using Utilities.Shared;

namespace Infrastructure.Repository.InterfacesServices
{
    public interface ISucursalService
    {
        Task<ApiResponse<List<SucursalResponse>>> ListarSucursalesAsync();
        Task<ApiResponse<Paginacion<SucursalResponse>>> ListarSucursalesPaginacionAsync(int pageNumber, int pageSize, string filtro);
        Task<ApiResponse<SucursalResponse>> ObtenerSucursalAsync(int idSucursal);
        Task<ApiResponse<object>> RegistrarSucursalAsync(Sucursal sucursal);
        Task<ApiResponse<object>> EditarSucursalAsync(Sucursal sucursal);
        Task<ApiResponse<int>> EliminarSucursalAsync(int idSucursal);
    }
}
