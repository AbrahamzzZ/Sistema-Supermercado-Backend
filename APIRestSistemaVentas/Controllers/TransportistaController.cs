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
    public class TransportistaController : ControllerBase
    {
        private readonly TransportistaService _transportistaService;

        public TransportistaController(TransportistaService transportistaService)
        {
            _transportistaService = transportistaService;
        }

        //Para pruebas unitarias, descomenta este constructor y comenta el constructor anterior.

        /*private readonly ITransportistaService _transportistaService;

        public TransportistaController(ITransportistaService transportistaService)
        {
            _transportistaService = transportistaService;
        }*/

        // GET: api/transportista
        [HttpGet]
        [SwaggerOperation(
            Summary = "Listar transportista",
            Description = "Obtiene la lista completa de transportistas registrados en el sistema."
        )]
        [SwaggerResponse(200, "Lista de transportistas obtenida correctamente")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<ApiResponse<Transportistum>>> GetTransportistas()
        {
            var transportistas = await _transportistaService.ListarTransportistasAsync();
            return Ok(transportistas);
        }

        // GET: api/transportista/paginacion
        [HttpGet("paginacion")]
        [SwaggerOperation(
            Summary = "Listar transportistas con paginación",
            Description = "Obtiene la lista de transportistas utilizando paginación."
        )]
        [SwaggerResponse(200, "Lista paginada obtenida correctamente")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<ApiResponse<Paginacion<Transportistum>>>> GetTransportistasPaginacion(int pageNumber = 1, int pageSize = 10)
        {
            var result = await _transportistaService.ListarTransportistasPaginacionAsync(pageNumber, pageSize);
            return Ok(result);
        }

        // GET: api/transportista/5
        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Obtener transportista por ID",
            Description = "Obtiene la información de un transportista específico mediante su identificador."
        )]
        [SwaggerResponse(200, "Transportista encontrado")]
        [SwaggerResponse(404, "Transportista no encontrado")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<Transportistum>> GetTransportista(int id)
        {
            var response = await _transportistaService.ObtenerTransportistaAsync(id);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }

        // POST: api/transportista
        [HttpPost]
        [SwaggerOperation(
            Summary = "Registrar transportista",
            Description = "Registra un nuevo transportista en el sistema."
        )]
        [SwaggerResponse(200, "Transportista registrado correctamente")]
        [SwaggerResponse(400, "Error en los datos enviados")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<ApiResponse<object>>> RegistrarProveedor([FromBody] Transportistum transportista)
        {
            var response = await _transportistaService.RegistrarTransportistaAsync(transportista);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        // PUT: api/transportista/5
        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Actualizar transportista",
            Description = "Actualiza la información de un transportista existente."
        )]
        [SwaggerResponse(200, "Transportista actualizado correctamente")]
        [SwaggerResponse(400, "Error en los datos enviados")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<ApiResponse<object>>> EditarTransportista(int id, [FromBody] Transportistum transportista)
        {
            var response = await _transportistaService.EditarTransportistaAsync(transportista);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        // DELETE: api/transportista/5
        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Eliminar transportista",
            Description = "Elimina un transportista del sistema mediante su identificador."
        )]
        [SwaggerResponse(200, "Transportistae eliminado correctamente")]
        [SwaggerResponse(404, "Transportista no encontrado")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<ApiResponse<int>>> EliminarTransportista(int id)
        {
            var response = await _transportistaService.EliminarTransportistaAsync(id);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }
    }
}
