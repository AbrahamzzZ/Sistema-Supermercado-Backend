using Domain.Models;
using Utilities.Shared;

namespace Infrastructure.Repository.InterfacesRepository
{
    public interface ISucursalRepository
    {
        Task<List<Sucursal>> ListarSucursalesAsync();
        Task<Paginacion<Sucursal>> ListarSucursalesPaginacionAsync(int pageNumber, int pageSize);
        Task<Sucursal?> ObtenerSucursalAsync(int idSucursal);
        Task<int> RegistrarSucursalAsync(Sucursal sucursal);
        Task<int> EditarSucursalAsync(Sucursal sucursal);
        Task<int> EliminarSucursalAsync(int idSucursal);
    }
}
