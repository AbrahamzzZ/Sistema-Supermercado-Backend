using Domain.Models;
using Infrastructure.Repository.InterfacesServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Utilities.Shared;
using Utilities.Security;

namespace APIRestSistemaVentas.Controllers
{
    [Authorize(Roles = Roles.Administrador)]
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
            Description = "Obtiene la lista de logs, del más reciente al más antiguo, utilizando paginación. " +
                          "Opcionalmente filtra por rango de fechas (desde/hasta, formato yyyy-MM-dd; 'hasta' incluye el día completo)."
        )]
        [SwaggerResponse(200, "Lista paginada obtenida correctamente")]
        [SwaggerResponse(400, "Rango de fechas inválido")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<ApiResponse<Paginacion<Log>>>> ListarLogsPaginacion(int pageNumber = 1, int pageSize = 10, string filtro = "", DateOnly? desde = null, DateOnly? hasta = null)
        {
            if (desde.HasValue && hasta.HasValue && desde.Value > hasta.Value)
                return BadRequest(new ApiResponse<Paginacion<Log>> { IsSuccess = false, Message = "La fecha 'desde' no puede ser posterior a la fecha 'hasta'." });

            var result = await _logService.ListarLogsPaginacionAsync(pageNumber, pageSize, filtro, desde, hasta);
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
        public async Task<ActionResult<Log>> ObtenerLog(int id)
        {
            var response = await _logService.ObtenerLogAsync(id);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }
    }
}
