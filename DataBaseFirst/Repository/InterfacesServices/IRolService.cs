using DataBaseFirst.Models;
using Utilities.Shared;

namespace DataBaseFirst.Repository.InterfacesServices
{
    public interface IRolService
    {
        Task<ApiResponse<List<Rol>>> ListarRolesAsync();
    }
}
