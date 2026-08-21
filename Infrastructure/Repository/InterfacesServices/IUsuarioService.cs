using Domain.Models;
using Domain.Models.Dto.Request;
using Domain.Models.Dto.Response.Usuario;
using Utilities.Shared;

namespace Infrastructure.Repository.InterfacesServices
{
    public interface IUsuarioService
    {
        Task<ApiResponse<List<UsuarioRolResponse>>> ListarUsuariosAsync();
        Task<ApiResponse<Paginacion<UsuarioRolResponse>>> ListarUsuariosPaginacionAsync(int pageNumber, int pageSize);
        Task<ApiResponse<UsuarioRolResponse>> ObtenerUsuarioAsync(int idUsuario);
        Task<ApiResponse<UsuarioRolResponse>> IniciarSesionAsync(LoginRequest login);
        Task<ApiResponse<object>> RegistrarUsuarioAsync(Usuario usuario);
        Task<ApiResponse<object>> EditarUsuarioAsync(Usuario usuario);
        Task<ApiResponse<int>> EliminarUsuarioAsync(int idUsuario);
    }
}
