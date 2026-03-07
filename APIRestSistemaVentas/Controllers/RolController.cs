using Domain.Models;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
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
        [SwaggerOperation(
            Summary = "Listar roles",
            Description = "Obtiene la lista de roles disponibles en el sistema."
        )]
        [SwaggerResponse(200, "Lista de roles obtenida correctamente")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<ApiResponse<List<Rol>>>> GetRoles()
        {
            var respuesta = await _rolService.ListarRolesAsync();
            return Ok(respuesta);
        }
    }
}
