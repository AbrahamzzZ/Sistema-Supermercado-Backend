using Domain.Models;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace APIRestSistemaVentas.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly MenuService _menuService;

        public MenuController(MenuService menuService)
        {
            _menuService = menuService;
        }

        // GET: api/menu/1
        [HttpGet("{idUsuario}")]
        [SwaggerOperation(
            Summary = "Obtener menús por usuario",
            Description = "Obtiene la lista de menús disponibles para un usuario según su rol."
        )]
        [SwaggerResponse(200, "Menús obtenidos correctamente")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<IEnumerable<Menu>>> GetMenus(int idUsuario)
        {
            var respuesta = await _menuService.ObtenerMenusAsync(idUsuario);
            return Ok(respuesta);
        }
    }
}
