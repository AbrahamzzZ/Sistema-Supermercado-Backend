using DataBaseFirst.Models;
using DataBaseFirst.Models.Dto;
using DataBaseFirst.Repository.InterfacesServices;
using DataBaseFirst.Services;
using Microsoft.AspNetCore.Mvc;
using Utilities;
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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            var usuarios = await _usuarioService.ListarUsuariosAsync();
            return Ok(usuarios);
        }

        [HttpGet("paginacion")]
        public async Task<ActionResult<ApiResponse<Paginacion<UsuarioRol>>>> GetUsuariosPaginacion(int pageNumber = 1, int pageSize = 10)
        {
            var result = await _usuarioService.ListarUsuariosPaginacionAsync(pageNumber, pageSize);
            return Ok(result);
        }

        // GET: api/usuario/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<UsuarioRol>>> GetUsuario(int id)
        {
            var response = await _usuarioService.ObtenerUsuarioAsync(id);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }

        // POST: api/usuario/login
        [HttpPost("login")]
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
        [HttpPost]
        public async Task<ActionResult<ApiResponse<object>>>  RegistrarUsuario([FromBody] Usuario usuario)
        {
            var response = await _usuarioService.RegistrarUsuarioAsync(usuario);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        // PUT: api/usuario/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> EditarUsuario(int id, [FromBody] Usuario usuario)
        {
            var response = await _usuarioService.EditarUsuarioAsync(usuario);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        // DELETE: api/usuario/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<int>>> EliminarUsuario(int id)
        {
            var response = await _usuarioService.EliminarUsuarioAsync(id);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }
    }
}
