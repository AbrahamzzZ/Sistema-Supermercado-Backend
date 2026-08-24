using Domain.Models;
using Domain.Models.Dto.Response.Sucursal;
using Utilities.Shared;

namespace Infrastructure.Repository.InterfacesRepository
{
    public interface ISucursalRepository
    {
        Task<List<SucursalResponse>> ListarSucursalesAsync();
        Task<Paginacion<SucursalResponse>> ListarSucursalesPaginacionAsync(int pageNumber, int pageSize);
        Task<SucursalResponse?> ObtenerSucursalAsync(int idSucursal);
        Task<int> RegistrarSucursalAsync(Sucursal sucursal, int usuarioCreacion);
        Task<int> EditarSucursalAsync(Sucursal sucursal);
        Task<int> EliminarSucursalAsync(int idSucursal);
    }
}
