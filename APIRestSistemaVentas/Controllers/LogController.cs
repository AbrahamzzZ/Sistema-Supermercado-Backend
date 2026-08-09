using Domain.Models;
using Infrastructure.Repository.InterfacesServices;
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
    public class LogController : ControllerBase
    {
        private readonly ILogService _logService;

        public LogController(ILogService logService)
        {
            _logService = logService;
        }

        // GET: api/log/paginacion
        [HttpGet("paginacion")]
        [SwaggerOperation(
            Summary = "Listar logs con paginación",
            Description = "Obtiene la lista de logs utilizando paginación."
        )]
        [SwaggerResponse(200, "Lista paginada obtenida correctamente")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<ApiResponse<Paginacion<Log>>>> GetLogsPaginacion(int pageNumber = 1, int pageSize = 10)
        {
            var result = await _logService.ListarLogsPaginacionAsync(pageNumber, pageSize);
            return Ok(result);
        }

        // GET: api/log/5
        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Obtener log por ID",
            Description = "Obtiene la información de un log específico mediante su identificador."
        )]
        [SwaggerResponse(200, "Log encontrado")]
        [SwaggerResponse(404, "Log no encontrado")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<Log>> GetLog(int id)
        {
            var response = await _logService.ObtenerLogAsync(id);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }
    }
}
