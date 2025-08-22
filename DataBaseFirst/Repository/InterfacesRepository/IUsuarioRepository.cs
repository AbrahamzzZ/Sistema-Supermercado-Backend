using DataBaseFirst.Models;
using DataBaseFirst.Models.Dto;
using Utilities.Shared;

namespace DataBaseFirst.Repository.InterfacesRepository
{
    public interface IUsuarioRepository
    {
        Task<List<UsuarioRol>> ListarUsuariosAsync();
        Task<Paginacion<UsuarioRol>> ListarUsuariosPaginacionAsync(int pageNumber, int pageSize);
        Task<UsuarioRol?> ObtenerUsuarioAsync(int idUsuario);
        Task<UsuarioRol?> IniciarSesionAsync(Login login);
        Task<int> RegistrarUsuarioAsync(Usuario usuario);
        Task<int> EditarUsuarioAsync(Usuario usuario);
        Task<int> EliminarUsuarioAsync(int idUsuario);
    }
}
