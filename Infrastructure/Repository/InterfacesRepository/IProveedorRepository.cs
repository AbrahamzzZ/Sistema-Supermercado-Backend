using Domain.Models;
using Domain.Models.Dto.Response.Provedor;
using Utilities.Shared;

namespace Infrastructure.Repository.InterfacesRepository
{
    public interface IProveedorRepository
    {
        Task<List<ProveedorResponse>> ListarProveedoresAsync();
        Task<Paginacion<ProveedorResponse>> ListarProveedoresPaginacionAsync(int pageNumber, int pageSize);
        Task<ProveedorResponse?> ObtenerProveedorAsync(int idProveedor);
        Task<int> RegistrarProveedorAsync(Proveedor proveedor, int usuarioCreacion);
        Task<int> EditarProveedorAsync(Proveedor proveedor);
        Task<int> EliminarProveedorAsync(int idProveedor);
    }
}
