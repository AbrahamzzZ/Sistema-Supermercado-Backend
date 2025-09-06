using DataBaseFirst.Models;
using Utilities.Shared;

namespace Infrastructure.Repository.InterfacesServices
{
    public interface ICategoriaService
    {
        Task<ApiResponse<List<Categorium>>> ListarCategoriasAsync();
        Task<ApiResponse<Paginacion<Categorium>>> ListarCategoriasPaginacionAsync(int pageNumber, int pageSize);
        Task<ApiResponse<Categorium>> ObtenerCategoriaAsync(int idCategoria);
        Task<ApiResponse<object>> RegistrarCategoriaAsync(Categorium categoria);
        Task<ApiResponse<object>> EditarCategoriaAsync(Categorium categoria);
        Task<ApiResponse<int>> EliminarCategoriaAsync(int id);
    }
}
