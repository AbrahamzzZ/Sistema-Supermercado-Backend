using Domain.Models;
using Domain.Models.Dto.Response.Categoria;
using Utilities.Shared;

namespace Infrastructure.Repository.InterfacesRepository
{
    public interface ICategoriaRepository
    {
        Task<List<CategoriaResponse>> ListarCategoriasAsync();
        Task<Paginacion<CategoriaResponse>> ListarCategoriasPaginacionAsync(int pageNumber, int pageSize, string filtro);
        Task<CategoriaResponse?> ObtenerCategoriaAsync(int idCategoria);
        Task<int> RegistrarCategoriaAsync(Categorium categoria, int usuarioCreacion);
        Task<int> EditarCategoriaAsync(Categorium categoria, int usuarioModificacion);
        Task<int> EliminarCategoriaAsync(int id);
    }
}
