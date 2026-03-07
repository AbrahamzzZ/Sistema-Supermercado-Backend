using Domain.Models;
using Domain.Models.Dto;
using Infrastructure.Helpers;
using Infrastructure.Repository.InterfacesServices;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Utilities.Shared;

namespace APIRestSistemaVentas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;
        private readonly MenuService _menuService;
        private readonly Token _token;

        public UsuarioController(UsuarioService usuarioService, Token token, MenuService menuService)
        {
            _usuarioService = usuarioService;
            _token = token;
            _menuService = menuService;
        }

        // GET: api/usuario
        [Authorize]
        [HttpGet]
        [SwaggerOperation(
            Summary = "Listar usuarios",
            Description = "Obtiene la lista completa de usuarios registrados en el sistema."
        )]
        [SwaggerResponse(200, "Lista de usuarios obtenida correctamente")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            var usuarios = await _usuarioService.ListarUsuariosAsync();
            return Ok(usuarios);
        }

        // GET: api/usuario/paginacion
        [Authorize]
        [HttpGet("paginacion")]
        [SwaggerOperation(
            Summary = "Listar usuarios con paginación",
            Description = "Obtiene la lista de usuarios utilizando paginación."
        )]
        [SwaggerResponse(200, "Lista paginada obtenida correctamente")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<ApiResponse<Paginacion<UsuarioRol>>>> GetUsuariosPaginacion(int pageNumber = 1, int pageSize = 10)
        {
            var result = await _usuarioService.ListarUsuariosPaginacionAsync(pageNumber, pageSize);
            return Ok(result);
        }

        // GET: api/usuario/5
        [Authorize]
        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Obtener usuario por ID",
            Description = "Obtiene la información de un usuario específico mediante su identificador."
        )]
        [SwaggerResponse(200, "Usuario encontrado")]
        [SwaggerResponse(404, "Usuario no encontrado")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<ApiResponse<UsuarioRol>>> GetUsuario(int id)
        {
            var response = await _usuarioService.ObtenerUsuarioAsync(id);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }

        // POST: api/usuario/login
        [HttpPost("login")]
        [SwaggerOperation(
            Summary = "Iniciar sesión",
            Description = "Valida las credenciales del usuario y genera un token JWT si son correctas."
        )]
        [SwaggerResponse(200, "Login exitoso, retorna el token")]
        [SwaggerResponse(401, "Credenciales incorrectas")]

        public async Task<ActionResult<ApiResponse<UsuarioRol>>> IniciarSesion([FromBody] Login login)
        {
            var response = await _usuarioService.IniciarSesionAsync(login);

            if (!response.IsSuccess)
                return Unauthorized(response);

            List<Menu> menus = await _menuService.ObtenerMenusAsync(response.Data!.Id_Usuario);
            string tokenGenerado = _token.GenerarToken(response.Data, menus);

            return Ok(new ApiResponse<object> { IsSuccess = true, Message = Mensajes.MESSAGE_TOKEN, Data = new { token = tokenGenerado }} );
        }

        // POST: api/usuario
        [Authorize]
        [HttpPost]
        [SwaggerOperation(
            Summary = "Registrar usuario",
            Description = "Registra un nuevo usuario en el sistema."
        )]
        [SwaggerResponse(200, "Usuario registrado correctamente")]
        [SwaggerResponse(400, "Error en los datos enviados")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<ApiResponse<object>>>  RegistrarUsuario([FromBody] Usuario usuario)
        {
            var response = await _usuarioService.RegistrarUsuarioAsync(usuario);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        // PUT: api/usuario/5
        [Authorize]
        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Actualizar usuario",
            Description = "Actualiza la información de un usuario existente."
        )]
        [SwaggerResponse(200, "Usuario actualizado correctamente")]
        [SwaggerResponse(400, "Error en los datos enviados")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<ApiResponse<object>>> EditarUsuario(int id, [FromBody] Usuario usuario)
        {
            var response = await _usuarioService.EditarUsuarioAsync(usuario);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        // DELETE: api/usuario/5
        [Authorize]
        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Eliminar usuario",
            Description = "Elimina un usuario del sistema mediante su identificador."
        )]
        [SwaggerResponse(200, "Usuario eliminado correctamente")]
        [SwaggerResponse(404, "Usuario no encontrado")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<ApiResponse<int>>> EliminarUsuario(int id)
        {
            var response = await _usuarioService.EliminarUsuarioAsync(id);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }
    }
}
