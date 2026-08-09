using Domain.Models;

namespace Infrastructure.Repository.InterfacesRepository
{
    public interface IMenuRepository
    {
        Task<List<Menu>> ObtenerMenusAsync(int idUsuario);
    }
}
