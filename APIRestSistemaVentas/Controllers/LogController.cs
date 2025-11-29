using Domain.Models;
using Infrastructure.Repository.InterfacesServices;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("paginacion")]
        public async Task<ActionResult<ApiResponse<Paginacion<Log>>>> GetLogsPaginacion(int pageNumber = 1, int pageSize = 10)
        {
            var result = await _logService.ListarLogsPaginacionAsync(pageNumber, pageSize);
            return Ok(result);
        }

        // GET: api/log/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Categorium>> GetLog(int id)
        {
            var response = await _logService.ObtenerLogAsync(id);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }
    }
}
