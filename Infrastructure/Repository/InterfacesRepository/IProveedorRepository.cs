using DataBaseFirst.Models;
using Utilities.Shared;

namespace Infrastructure.Repository.InterfacesRepository
{
    public interface IProveedorRepository
    {
        Task<List<Proveedor>> ListarProveedoresAsync();
        Task<Paginacion<Proveedor>> ListarProveedoresPaginacionAsync(int pageNumber, int pageSize);
        Task<Proveedor?> ObtenerProveedorAsync(int idProveedor);
        Task<int> RegistrarProveedorAsync(Proveedor proveedor);
        Task<int> EditarProveedorAsync(Proveedor proveedor);
        Task<int> EliminarProveedorAsync(int idProveedor);
    }
}
