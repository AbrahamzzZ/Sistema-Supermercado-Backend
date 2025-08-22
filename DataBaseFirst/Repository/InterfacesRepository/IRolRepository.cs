using DataBaseFirst.Models;

namespace DataBaseFirst.Repository.InterfacesRepository
{
    public interface IRolRepository
    {
        Task<List<Rol>> ListarRolesAsync();
    }
}
