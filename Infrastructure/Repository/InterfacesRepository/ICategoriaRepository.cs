using Domain.Models;
using Utilities.Shared;

namespace Infrastructure.Repository.InterfacesRepository
{
    public interface ICategoriaRepository
    {
        Task<List<Categorium>> ListarCategoriasAsync();
        Task<Paginacion<Categorium>> ListarCategoriasPaginacionAsync(int pageNumber, int pageSize);
        Task<Categorium?> ObtenerCategoriaAsync(int idCategoria);
        Task<int> RegistrarCategoriaAsync(Categorium categoria);
        Task<int> EditarCategoriaAsync(Categorium categoria);
        Task<int> EliminarCategoriaAsync(int id);
    }
}
