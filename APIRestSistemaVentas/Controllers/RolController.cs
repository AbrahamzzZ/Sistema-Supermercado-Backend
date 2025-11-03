using DataBaseFirst.Models;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Shared;

namespace APIRestSistemaVentas.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RolController : ControllerBase
    {
        private readonly RolService _rolService;

        public RolController(RolService rolService)
        {
            _rolService = rolService;
        }

        // GET: api/rol
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<Rol>>>> GetRoles()
        {
            var respuesta = await _rolService.ListarRolesAsync();
            return Ok(respuesta);
        }
    }
}
