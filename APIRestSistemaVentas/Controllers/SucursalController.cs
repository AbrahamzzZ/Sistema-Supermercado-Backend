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
    public class SucursalController : ControllerBase
    {
        private readonly SucursalService _sucursalService;

        public SucursalController(SucursalService sucursalService)
        {
            _sucursalService = sucursalService;
        }

        //Para pruebas unitarias, descomenta este constructor y comenta el constructor anterior.

        /*private readonly ISucursalService _sucursalService;

        public SucursalController(ISucursalService sucursalService)
        {
            _sucursalService = sucursalService;
        }*/

        // GET: api/sucursal
        [HttpGet]
        [SwaggerOperation(
            Summary = "Listar sucursales",
            Description = "Obtiene la lista completa de sucursales registradas en el sistema."
        )]
        [SwaggerResponse(200, "Lista de sucursales obtenida correctamente")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<ApiResponse<Sucursal>>> GetSucursales()
        {
            var sucursales = await _sucursalService.ListarSucursalesAsync();
            return Ok(sucursales);
        }

        // GET: api/sucursal/paginacion
        [HttpGet("paginacion")]
        [SwaggerOperation(
            Summary = "Listar sucursales con paginación",
            Description = "Obtiene la lista de sucursales utilizando paginación."
        )]
        [SwaggerResponse(200, "Lista paginada obtenida correctamente")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<ApiResponse<Paginacion<Sucursal>>>> GetSucursalesPaginacion(int pageNumber = 1, int pageSize = 10)
        {
            var result = await _sucursalService.ListarSucursalesPaginacionAsync(pageNumber, pageSize);
            return Ok(result);
        }

        // GET: api/sucursal/5
        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Obtener sucursal por ID",
            Description = "Obtiene la información de una sucursal específica mediante su identificador."
        )]
        [SwaggerResponse(200, "Sucursal encontrada")]
        [SwaggerResponse(404, "Sucursal no encontrada")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<Sucursal>> GetSucursal(int id)
        {
            var response = await _sucursalService.ObtenerSucursalAsync(id);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }

        // POST: api/sucursal
        [HttpPost]
        [SwaggerOperation(
            Summary = "Registrar sucursal",
            Description = "Registra una nueva sucursal en el sistema."
        )]
        [SwaggerResponse(200, "Sucursal registrada correctamente")]
        [SwaggerResponse(400, "Error en los datos enviados")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<ApiResponse<object>>> RegistrarSucursal([FromBody] Sucursal sucursal)
        {
            var response = await _sucursalService.RegistrarSucursalAsync(sucursal);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        // PUT: api/sucursal/5
        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Actualizar sucursal",
            Description = "Actualiza la información de una sucursal existente."
        )]
        [SwaggerResponse(200, "Sucursal actualizada correctamente")]
        [SwaggerResponse(400, "Error en los datos enviados")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<ApiResponse<object>>> EditarSucursal(int id, [FromBody] Sucursal sucursal)
        {
            var response = await _sucursalService.EditarSucursalAsync(sucursal);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        // DELETE: api/sucursal/5
        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Eliminar sucursal",
            Description = "Elimina una sucursal del sistema mediante su identificador."
        )]
        [SwaggerResponse(200, "Sucursal eliminada correctamente")]
        [SwaggerResponse(404, "Sucursal no encontrada")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<ApiResponse<int>>> EliminarSucursal(int id)
        {
            var response = await _sucursalService.EliminarSucursalAsync(id);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }
    }
}
