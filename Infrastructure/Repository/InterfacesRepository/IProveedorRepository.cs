using Domain.Models;
using Domain.Models.Dto.Response.Provedor;
using Utilities.Shared;

namespace Infrastructure.Repository.InterfacesRepository
{
    public interface IProveedorRepository
    {
        Task<List<ProveedorResponse>> ListarProveedoresAsync();
        Task<Paginacion<Proveedor>> ListarProveedoresPaginacionAsync(int pageNumber, int pageSize);
        Task<Proveedor?> ObtenerProveedorAsync(int idProveedor);
        Task<int> RegistrarProveedorAsync(Proveedor proveedor, int usuarioCreacion);
        Task<int> EditarProveedorAsync(Proveedor proveedor);
        Task<int> EliminarProveedorAsync(int idProveedor);
    }
}
