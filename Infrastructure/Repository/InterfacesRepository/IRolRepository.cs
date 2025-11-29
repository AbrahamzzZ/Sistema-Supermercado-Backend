using Domain.Models;

namespace Infrastructure.Repository.InterfacesRepository
{
    public interface IRolRepository
    {
        Task<List<Rol>> ListarRolesAsync();
    }
}
