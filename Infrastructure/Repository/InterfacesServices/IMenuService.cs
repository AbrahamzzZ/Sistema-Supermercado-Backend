using Domain.Models;
using Utilities.Shared;

namespace Infrastructure.Repository.InterfacesServices
{
    public interface IMenuService
    {
        Task<ApiResponse<List<Menu>>> ObtenerMenusAsync(int idUsuario);
    }
}
