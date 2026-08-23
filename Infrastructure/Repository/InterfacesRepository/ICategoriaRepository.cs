using Domain.Models;
using Domain.Models.Dto.Response.Categoria;
using Utilities.Shared;

namespace Infrastructure.Repository.InterfacesRepository
{
    public interface ICategoriaRepository
    {
        Task<List<CategoriaResponse>> ListarCategoriasAsync();
        Task<Paginacion<Categorium>> ListarCategoriasPaginacionAsync(int pageNumber, int pageSize);
        Task<Categorium?> ObtenerCategoriaAsync(int idCategoria);
        Task<int> RegistrarCategoriaAsync(Categorium categoria, int UsuarioCreacion);
        Task<int> EditarCategoriaAsync(Categorium categoria);
        Task<int> EliminarCategoriaAsync(int id);
    }
}
