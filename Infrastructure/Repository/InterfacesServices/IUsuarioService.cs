using Domain.Models;
using Domain.Models.Dto;
using Utilities.Shared;

namespace Infrastructure.Repository.InterfacesServices
{
    public interface IUsuarioService
    {
        Task<ApiResponse<List<UsuarioRol>>> ListarUsuariosAsync();
        Task<ApiResponse<Paginacion<UsuarioRol>>> ListarUsuariosPaginacionAsync(int pageNumber, int pageSize);
        Task<ApiResponse<UsuarioRol>> ObtenerUsuarioAsync(int idUsuario);
        Task<ApiResponse<UsuarioRol>> IniciarSesionAsync(Login login);
        Task<ApiResponse<object>> RegistrarUsuarioAsync(Usuario usuario);
        Task<ApiResponse<object>> EditarUsuarioAsync(Usuario usuario);
        Task<ApiResponse<int>> EliminarUsuarioAsync(int idUsuario);
    }
}
