using DataBaseFirst.Models;
using DataBaseFirst.Models.Dto;
using DataBaseFirst.Repository;
using DataBaseFirst.Repository.InterfacesRepository;
using DataBaseFirst.Repository.InterfacesServices;
using Microsoft.Data.SqlClient;
using System.Text.RegularExpressions;
using Utilities.Shared;

namespace DataBaseFirst.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly UsuarioRepository _usuarioRepository;

        public UsuarioService(UsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        //Para pruebas unitarias, descomenta este constructor y comenta el constructor anterior.

        /*readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }*/

        public async Task<ApiResponse<List<UsuarioRol>>> ListarUsuariosAsync()
        {
            var listaUsuarios = await _usuarioRepository.ListarUsuariosAsync();

            if (listaUsuarios == null || listaUsuarios.Count == 0)
                return new ApiResponse<List<UsuarioRol>> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY, Data = listaUsuarios };

            return new ApiResponse<List<UsuarioRol>> { IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = listaUsuarios };
        }

        public async Task<ApiResponse<Paginacion<UsuarioRol>>> ListarUsuariosPaginacionAsync(int pageNumber, int pageSize)
        {
            var pagedResult = await _usuarioRepository.ListarUsuariosPaginacionAsync(pageNumber, pageSize);

            if (pagedResult.Items == null || pagedResult.Items.Count == 0)
            {
                return new ApiResponse<Paginacion<UsuarioRol>> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY, Data = pagedResult };
            }

            return new ApiResponse<Paginacion<UsuarioRol>> { IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = pagedResult };
        }

        public async Task<ApiResponse<UsuarioRol>> ObtenerUsuarioAsync(int idUsuario)
        {
            var usuario = await _usuarioRepository.ObtenerUsuarioAsync(idUsuario);

            if (usuario == null)
            {
                return new ApiResponse<UsuarioRol> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };
            }

            return new ApiResponse<UsuarioRol> { IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = usuario };
        }

        public async Task<ApiResponse<UsuarioRol>> IniciarSesionAsync(Login login)
        {
            if (login == null || string.IsNullOrWhiteSpace(login.Correo_Electronico) || string.IsNullOrWhiteSpace(login.Clave))
            {
                return new ApiResponse<UsuarioRol> { IsSuccess = false, Message = "Correo y clave son obligatorios." };
            }

            var usuario = await _usuarioRepository.IniciarSesionAsync(login);

            if (usuario == null)
            {
                return new ApiResponse<UsuarioRol> { IsSuccess = false, Message = "Credenciales inválidas." };
            }

            if (usuario.Estado == false)
            {
                return new ApiResponse<UsuarioRol>
                { IsSuccess = false, Message = "Usuario inactivo. Contacte con el administrador." };
            }

            return new ApiResponse<UsuarioRol> { IsSuccess = true, Message = "Inicio de sesión exitoso.", Data = usuario};
        }

        public async Task<ApiResponse<object>> RegistrarUsuarioAsync(Usuario usuario)
        {
            if (usuario == null)
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_VALIDATE };

            if (string.IsNullOrWhiteSpace(usuario.Nombre_Completo) || string.IsNullOrWhiteSpace(usuario.Correo_Electronico) || string.IsNullOrWhiteSpace(usuario.Clave))
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_EMPTY };

            var regex = new Regex("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$");
            if (!regex.IsMatch(usuario.Nombre_Completo))
                return new ApiResponse<object> { IsSuccess = false, Message = "El nombre completo solo puede contener letras y espacios." };

            var regexClave = new Regex(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[^A-Za-z\d]).{10,}$");
            if (!regexClave.IsMatch(usuario.Clave!))
                return new ApiResponse<object> { IsSuccess = false, Message = "La clave debe tener al menos 10 caracteres, incluyendo letras, números y caracteres especiales." };

            var regexCorreo = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            if (!regexCorreo.IsMatch(usuario.Correo_Electronico))
                return new ApiResponse<object> { IsSuccess = false, Message = "El correo electrónico no tiene un formato válido" };

            var categorias = await _usuarioRepository.ListarUsuariosAsync();
            if (categorias.Any(c => c.Codigo == usuario.Codigo))
                return new ApiResponse<object> { IsSuccess = false, Message = "El código ya existe" };

            if (categorias.Any(c => c.Nombre_Completo == usuario.Nombre_Completo))
                return new ApiResponse<object> { IsSuccess = false, Message = "Ese nombre ya existe" };

            var result = await _usuarioRepository.RegistrarUsuarioAsync(usuario);
            if (result > 0)
                return new ApiResponse<object> { IsSuccess = true, Message = Mensajes.MESSAGE_REGISTER };

            return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_REGISTER_FAILLED };
        }

        public async Task<ApiResponse<object>> EditarUsuarioAsync(Usuario usuario)
        {
            if (usuario == null)
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_VALIDATE };

            if (string.IsNullOrWhiteSpace(usuario.Nombre_Completo) || string.IsNullOrWhiteSpace(usuario.Correo_Electronico))
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_EMPTY };

            var usuarioActual = await _usuarioRepository.ObtenerUsuarioAsync(usuario.Id_Usuario);
            if (usuarioActual == null)
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };

            if (usuarioActual.Nombre_Rol == "Administrador" && usuarioActual.Estado == true)
            {
                var totalAdmins = (await _usuarioRepository.ListarUsuariosAsync())
                    .Count(u => u.Nombre_Rol == "Administrador" && u.Estado == true);

                if (totalAdmins <= 1 && 
                    (usuario.Estado == false || usuario.Id_Rol != usuarioActual.Id_Rol))
                {
                    return new ApiResponse<object> { IsSuccess = false, Message = "No se puede modificar el rol al único administrador activo." };
                }
            }

            var regex = new Regex("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$");
            if (!regex.IsMatch(usuario.Nombre_Completo))
                return new ApiResponse<object> { IsSuccess = false, Message = "El nombre completo solo puede contener letras y espacios." };

            var regexCorreo = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            if (!regexCorreo.IsMatch(usuario.Correo_Electronico))
                return new ApiResponse<object> { IsSuccess = false, Message = "El correo electrónico no tiene un formato válido" };

            var usuarios = await _usuarioRepository.ListarUsuariosAsync();
            if (usuarios.Any(c => c.Nombre_Completo == usuario.Nombre_Completo && c.Id_Usuario != usuario.Id_Usuario))
            {
                return new ApiResponse<object> { IsSuccess = false, Message = "El nombre ya existe." };
            }

            var result = await _usuarioRepository.EditarUsuarioAsync(usuario);
            if (result > 0)
                return new ApiResponse<object> { IsSuccess = true, Message = Mensajes.MESSAGE_UPDATE };

            return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_UPDATE_FAILLED };
        }

        public async Task<ApiResponse<int>> EliminarUsuarioAsync(int id)
        {
            try
            {
                var existe = await _usuarioRepository.ObtenerUsuarioAsync(id);
                if (existe == null)
                {
                    return new ApiResponse<int> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };
                }

                if (existe.Nombre_Rol == "Administrador" && existe.Estado == true)
                {
                    var totalAdmins = (await _usuarioRepository.ListarUsuariosAsync())
                        .Count(u => u.Nombre_Rol == "Administrador" && u.Estado == true);

                    if (totalAdmins <= 1)
                    {
                        return new ApiResponse<int> { IsSuccess = false, Message = "No se puede eliminar al único administrador activo." };
                    }
                }

                var result = await _usuarioRepository.EliminarUsuarioAsync(id);

                if (result > 0)
                    return new ApiResponse<int> { IsSuccess = true, Message = Mensajes.MESSAGE_DELETE };

                return new ApiResponse<int> { IsSuccess = false, Message = Mensajes.MESSAGE_DELETE_FAILLED };
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                return new ApiResponse<int> { IsSuccess = false, Message = "No se puede eliminar el usuario porque tiene compras o ventas asociadas." };
            }
        }
    }
}
