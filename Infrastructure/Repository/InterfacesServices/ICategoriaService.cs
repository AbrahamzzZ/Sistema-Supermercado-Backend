using Domain.Models;
using Domain.Models.Dto.Response.Categoria;
using Utilities.Shared;

namespace Infrastructure.Repository.InterfacesServices
{
    public interface ICategoriaService
    {
        Task<ApiResponse<List<CategoriaResponse>>> ListarCategoriasAsync();
        Task<ApiResponse<Paginacion<CategoriaResponse>>> ListarCategoriasPaginacionAsync(int pageNumber, int pageSize);
        Task<ApiResponse<CategoriaResponse>> ObtenerCategoriaAsync(int idCategoria);
        Task<ApiResponse<object>> RegistrarCategoriaAsync(Categorium categoria);
        Task<ApiResponse<object>> EditarCategoriaAsync(Categorium categoria);
        Task<ApiResponse<int>> EliminarCategoriaAsync(int id);
    }
}
