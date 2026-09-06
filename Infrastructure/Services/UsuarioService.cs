using Domain.Models;
using Domain.Models.Dto.Request;
using Domain.Models.Dto.Response.Usuario;
using FluentValidation;
using Infrastructure.Repository;
using Infrastructure.Repository.InterfacesBusiness;
using Infrastructure.Repository.InterfacesRepository;
using Infrastructure.Repository.InterfacesServices;
using Infrastructure.Services.business;
using Microsoft.Data.SqlClient;
using Utilities.Shared;

namespace Infrastructure.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly UsuarioRepository _usuarioRepository;
        private readonly IValidator<Usuario> _validator;
        private readonly ICurrentUser _currentUserService;
        private readonly IAuditoriaService _auditoriaService;

        public UsuarioService(UsuarioRepository usuarioRepository, IValidator<Usuario> validator, ICurrentUser currentUserService, IAuditoriaService auditoriaService)
        {
            _usuarioRepository = usuarioRepository;
            _validator = validator;
            _currentUserService = currentUserService;
            _auditoriaService = auditoriaService;
        }

        //Para pruebas unitarias, descomenta este constructor y comenta el constructor anterior.

        /*readonly IUsuarioRepository _usuarioRepository;
        private readonly IValidator<Usuario> _validator;
        public UsuarioService(IUsuarioRepository usuarioRepository, IValidator<Usuario> validator)
        {
            _usuarioRepository = usuarioRepository;
            _validator = validator;
        }*/

        public async Task<ApiResponse<List<UsuarioRolResponse>>> ListarUsuariosAsync()
        {
            try
            {
                var listaUsuarios = await _usuarioRepository.ListarUsuariosAsync();

                if (listaUsuarios == null || listaUsuarios.Count == 0)
                {
                    await _auditoriaService.RegistrarFalloAsync("Listar Usuarios", "No hay usuarios registrados");

                    return new ApiResponse<List<UsuarioRolResponse>>{ IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY, Data = listaUsuarios };
                }

                await _auditoriaService.RegistrarExitoAsync("Listar Usuarios", $"Se obtuvieron {listaUsuarios.Count} usuarios");

                return new ApiResponse<List<UsuarioRolResponse>>{ IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = listaUsuarios };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Listar Usuarios", ex);
                throw;
            }
        }

        public async Task<ApiResponse<Paginacion<UsuarioRolResponse>>> ListarUsuariosPaginacionAsync(int pageNumber, int pageSize, string filtro = "")
        {
            try
            {
                var pagedResult = await _usuarioRepository.ListarUsuariosPaginacionAsync(pageNumber, pageSize, filtro);

                if (pagedResult.Items == null || pagedResult.Items.Count == 0)
                {
                    await _auditoriaService.RegistrarFalloAsync( "Listar Usuarios Paginación", $"No hay resultados para el filtro: {filtro}");

                    return new ApiResponse<Paginacion<UsuarioRolResponse>>{ IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY, Data = pagedResult };
                }

                await _auditoriaService.RegistrarExitoAsync("Listar Usuarios Paginación", $"Página {pageNumber}, {pagedResult.Items.Count} usuarios. Filtro: {filtro}");

                return new ApiResponse<Paginacion<UsuarioRolResponse>>{ IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = pagedResult };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Listar Usuarios Paginación", ex);
                throw;
            }
        }

        public async Task<ApiResponse<UsuarioRolResponse>> ObtenerUsuarioAsync(int idUsuario)
        {
            try
            {
                var usuario = await _usuarioRepository.ObtenerUsuarioAsync(idUsuario);

                if (usuario == null)
                {
                    await _auditoriaService.RegistrarFalloAsync("Obtener Usuario", $"Usuario con ID {idUsuario} no encontrado");

                    return new ApiResponse<UsuarioRolResponse>{ IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };
                }

                await _auditoriaService.RegistrarExitoAsync("Obtener Usuario", $"Usuario {usuario.Codigo} obtenido correctamente");

                return new ApiResponse<UsuarioRolResponse>{ IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = usuario };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Obtener Usuario", ex);
                throw;
            }
        }

        public async Task<ApiResponse<UsuarioRolResponse>> IniciarSesionAsync(LoginRequest login)
        {
            try
            {
                if (login == null || string.IsNullOrWhiteSpace(login.Correo_Electronico) || string.IsNullOrWhiteSpace(login.Clave))
                {
                    await _auditoriaService.RegistrarFalloAsync("Iniciar Sesión", "Correo o clave vacíos");

                    return new ApiResponse<UsuarioRolResponse>{ IsSuccess = false, Message = "Correo y clave son obligatorios." };
                }

                var usuario = await _usuarioRepository.IniciarSesionAsync(login);

                if (usuario == null)
                {
                    await _auditoriaService.RegistrarFalloAsync("Iniciar Sesión", $"Credenciales inválidas para: {login.Correo_Electronico}");

                    return new ApiResponse<UsuarioRolResponse>{ IsSuccess = false, Message = "Credenciales inválidas." };
                }

                if (usuario.Estado == false)
                {
                    await _auditoriaService.RegistrarFalloAsync("Iniciar Sesión", $"Usuario inactivo: {usuario.Codigo}");

                    return new ApiResponse<UsuarioRolResponse>{ IsSuccess = false, Message = "Usuario inactivo. Contacte con el administrador." };
                }

                await _auditoriaService.RegistrarExitoAsync("Iniciar Sesión", $"Inicio de sesión exitoso: {usuario.Codigo}");

                return new ApiResponse<UsuarioRolResponse>{ IsSuccess = true, Message = "Inicio de sesión exitoso.", Data = usuario };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Iniciar Sesión", ex);
                throw;
            }
        }

        public async Task<ApiResponse<object>> RegistrarUsuarioAsync(Usuario usuario)
        {
            try
            {
                if (usuario == null)
                {
                    await _auditoriaService.RegistrarFalloAsync("Registrar Usuario", "Usuario nulo");

                    return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_NULL };
                }

                var validationResult = await _validator.ValidateAsync(usuario);

                if (!validationResult.IsValid)
                {
                    var errores = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage));
                    await _auditoriaService.RegistrarFalloAsync("Registrar Usuario", $"Validación fallida: {errores}");

                    return new ApiResponse<object>{ IsSuccess = false, Message = errores };
                }

                var usuarios = await _usuarioRepository.ListarUsuariosAsync();

                if (usuarios.Any(c => c.Codigo == usuario.Codigo))
                {
                    await _auditoriaService.RegistrarFalloAsync("Registrar Usuario", $"Código {usuario.Codigo} ya existe");

                    return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_CODE_EXITS };
                }

                if (usuarios.Any(c => c.Nombre_Completo == usuario.Nombre_Completo))
                {
                    await _auditoriaService.RegistrarFalloAsync("Registrar Usuario", $"Nombre {usuario.Nombre_Completo} ya existe");

                    return new ApiResponse<object>{ IsSuccess = false, Message = "Ese nombre ya existe" };
                }

                var idUsuario = _currentUserService.GetUserId();
                var result = await _usuarioRepository.RegistrarUsuarioAsync(usuario, idUsuario);

                if (result > 0)
                {
                    await _auditoriaService.RegistrarExitoAsync("Registrar Usuario", $"Usuario {usuario.Codigo} registrado exitosamente");

                    return new ApiResponse<object>{ IsSuccess = true, Message = Mensajes.MESSAGE_REGISTER };
                }

                await _auditoriaService.RegistrarFalloAsync("Registrar Usuario", $"No se pudo guardar el usuario {usuario.Codigo}");

                return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_REGISTER_FAILLED };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Registrar Usuario", ex);
                throw;
            }
        }

        public async Task<ApiResponse<object>> EditarUsuarioAsync(Usuario usuario)
        {
            try
            {
                if (usuario == null)
                {
                    await _auditoriaService.RegistrarFalloAsync("Editar Usuario", "Usuario nulo");

                    return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_NULL };
                }

                var validationResult = await _validator.ValidateAsync(usuario);
                if (!validationResult.IsValid)
                {
                    var errores = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage));
                    await _auditoriaService.RegistrarFalloAsync("Editar Usuario", $"Validación fallida: {errores}");

                    return new ApiResponse<object>{ IsSuccess = false, Message = errores };
                }

                var usuarioActual = await _usuarioRepository.ObtenerUsuarioAsync(usuario.Id_Usuario);
                if (usuarioActual == null)
                {
                    await _auditoriaService.RegistrarFalloAsync("Editar Usuario", $"Usuario con ID {usuario.Id_Usuario} no encontrado");

                    return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };
                }

                if (usuarioActual.Nombre_Rol == "Administrador" && usuarioActual.Estado == true)
                {
                    var totalAdmins = (await _usuarioRepository.ListarUsuariosAsync()).Count(u => u.Nombre_Rol == "Administrador" && u.Estado == true);

                    if (totalAdmins <= 1 && (usuario.Estado == false || usuario.Id_Rol != usuarioActual.Id_Rol))
                    {
                        await _auditoriaService.RegistrarFalloAsync("Editar Usuario", $"Intento de modificar al único administrador activo: {usuarioActual.Codigo}");

                        return new ApiResponse<object>{ IsSuccess = false, Message = "No se puede modificar el rol al único administrador activo." };
                    }
                }

                var usuarios = await _usuarioRepository.ListarUsuariosAsync();
                if (usuarios.Any(c => c.Nombre_Completo == usuario.Nombre_Completo && c.Id_Usuario != usuario.Id_Usuario))
                {
                    await _auditoriaService.RegistrarFalloAsync("Editar Usuario", $"Nombre {usuario.Nombre_Completo} ya existe en otro usuario");

                    return new ApiResponse<object>{ IsSuccess = false, Message = "El nombre ya existe." };
                }

                var idUsuario = _currentUserService.GetUserId();
                var result = await _usuarioRepository.EditarUsuarioAsync(usuario, idUsuario);

                if (result > 0)
                {
                    await _auditoriaService.RegistrarExitoAsync("Editar Usuario", $"Usuario {usuario.Codigo} actualizado exitosamente");
                    
                    return new ApiResponse<object>{ IsSuccess = true, Message = Mensajes.MESSAGE_UPDATE };
                }

                await _auditoriaService.RegistrarFalloAsync("Editar Usuario", $"No se pudo actualizar el usuario {usuario.Codigo}");

                return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_UPDATE_FAILLED };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Editar Usuario", ex);
                throw;
            }
        }

        public async Task<ApiResponse<int>> EliminarUsuarioAsync(int id)
        {
            try
            {
                var existe = await _usuarioRepository.ObtenerUsuarioAsync(id);
                if (existe == null)
                {
                    await _auditoriaService.RegistrarFalloAsync("Eliminar Usuario", $"Usuario con ID {id} no encontrado");

                    return new ApiResponse<int>{ IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };
                }

                if (existe.Nombre_Rol == "Administrador" && existe.Estado == true)
                {
                    var totalAdmins = (await _usuarioRepository.ListarUsuariosAsync()).Count(u => u.Nombre_Rol == "Administrador" && u.Estado == true);

                    if (totalAdmins <= 1)
                    {
                        await _auditoriaService.RegistrarFalloAsync("Eliminar Usuario", $"Intento de eliminar al único administrador activo: {existe.Codigo}");

                        return new ApiResponse<int>{ IsSuccess = false, Message = "No se puede eliminar al único administrador activo." };
                    }
                }

                var result = await _usuarioRepository.EliminarUsuarioAsync(id);

                if (result > 0)
                {
                    await _auditoriaService.RegistrarExitoAsync("Eliminar Usuario", $"Usuario {existe.Codigo} eliminado exitosamente");

                    return new ApiResponse<int>{ IsSuccess = true, Message = Mensajes.MESSAGE_DELETE };
                }

                await _auditoriaService.RegistrarFalloAsync("Eliminar Usuario", $"No se pudo eliminar el usuario {existe.Codigo}");

                return new ApiResponse<int>{ IsSuccess = false, Message = Mensajes.MESSAGE_DELETE_FAILLED };
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                await _auditoriaService.RegistrarErrorAsync("Eliminar Usuario", new Exception("No se puede eliminar: usuario tiene compras o ventas asociadas"));

                return new ApiResponse<int>{ IsSuccess = false, Message = "No se puede eliminar el usuario porque tiene compras o ventas asociadas." };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Eliminar Usuario", ex);
                throw;
            }
        }
    }
}
