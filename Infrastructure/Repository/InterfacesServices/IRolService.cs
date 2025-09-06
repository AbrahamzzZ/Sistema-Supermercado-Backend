using DataBaseFirst.Models;
using Utilities.Shared;

namespace Infrastructure.Repository.InterfacesServices
{
    public interface IRolService
    {
        Task<ApiResponse<List<Rol>>> ListarRolesAsync();
    }
}
