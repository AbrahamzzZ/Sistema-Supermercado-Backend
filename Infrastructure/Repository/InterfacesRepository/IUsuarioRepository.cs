using Domain.Models;
using Domain.Models.Dto.Request;
using Domain.Models.Dto.Response.Usuario;
using Utilities.Shared;

namespace Infrastructure.Repository.InterfacesRepository
{
    public interface IUsuarioRepository
    {
        Task<List<UsuarioRolResponse>> ListarUsuariosAsync();
        Task<Paginacion<UsuarioRolResponse>> ListarUsuariosPaginacionAsync(int pageNumber, int pageSize);
        Task<UsuarioRolResponse?> ObtenerUsuarioAsync(int idUsuario);
        Task<UsuarioRolResponse?> IniciarSesionAsync(LoginRequest login);
        Task<int> RegistrarUsuarioAsync(Usuario usuario);
        Task<int> EditarUsuarioAsync(Usuario usuario);
        Task<int> EliminarUsuarioAsync(int idUsuario);
    }
}
