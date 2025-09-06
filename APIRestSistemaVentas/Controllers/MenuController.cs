using DataBaseFirst.Models;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace APIRestSistemaVentas.Controllers
{
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
        public async Task<ActionResult<IEnumerable<Menu>>> GetMenus(int idUsuario)
        {
            var respuesta = await _menuService.ObtenerMenusAsync(idUsuario);
            return Ok(respuesta);
        }
    }
}
