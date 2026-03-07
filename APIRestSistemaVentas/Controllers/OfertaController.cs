using Domain.Models;
using Domain.Models.Dto;
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
    public class OfertaController : ControllerBase
    {
        private readonly OfertaService _ofertaService;

        public OfertaController(OfertaService ofertaService)
        {
            _ofertaService = ofertaService;
        }

        //Para pruebas unitarias, descomenta este constructor y comenta el constructor anterior.

        /*private readonly IOfertaService _ofertaService;

        public OfertaController(IOfertaService ofertaService)
        {
            _ofertaService = ofertaService;
        }*/

        // GET: api/oferta
        [HttpGet]
        [SwaggerOperation(
            Summary = "Listar ofertas",
            Description = "Obtiene la lista completa de ofertas de productos registradas en el sistema."
        )]
        [SwaggerResponse(200, "Lista de ofertas obtenida correctamente")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<ApiResponse<OfertaProducto>>> GetOfertas()
        {
            var ofertas = await _ofertaService.ListarOfertasAsync();
            return Ok(ofertas);
        }

        // GET: api/oferta/paginacion
        [HttpGet("paginacion")]
        [SwaggerOperation(
            Summary = "Listar ofertas con paginación",
            Description = "Obtiene la lista de ofertas utilizando paginación."
        )]
        [SwaggerResponse(200, "Lista paginada obtenida correctamente")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<ApiResponse<Paginacion<OfertaProducto>>>> GetOfertasPaginacion(int pageNumber = 1, int pageSize = 10)
        {
            var result = await _ofertaService.ListarOfertasPaginacionAsync(pageNumber, pageSize);
            return Ok(result);
        }

        // GET: api/oferta/5
        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Obtener oferta por ID",
            Description = "Obtiene la información de una oferta específica."
        )]
        [SwaggerResponse(200, "Oferta encontrada")]
        [SwaggerResponse(404, "Oferta no encontrada")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<Ofertum>> GetOferta(int id)
        {
            var response = await _ofertaService.ObtenerOfertaAsync(id);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }

        // POST: api/oferta
        [HttpPost]
        [SwaggerOperation(
            Summary = "Registrar oferta",
            Description = "Registra una nueva oferta para un producto."
        )]
        [SwaggerResponse(200, "Oferta registrada correctamente")]
        [SwaggerResponse(400, "Error en los datos enviados")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<ApiResponse<object>>> RegistrarOferta([FromBody] Ofertum oferta)
        {
            var response = await _ofertaService.RegistrarOfertaAsync(oferta);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        // PUT: api/oferta/5
        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Actualizar oferta",
            Description = "Actualiza la información de una oferta existente."
        )]
        [SwaggerResponse(200, "Oferta actualizada correctamente")]
        [SwaggerResponse(400, "Error en los datos enviados")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<ApiResponse<object>>> EditarOferta(int id, [FromBody] Ofertum oferta)
        {
            var response = await _ofertaService.EditarOfertaAsync(oferta);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        // DELETE: api/oferta/5
        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Eliminar oferta",
            Description = "Elimina una oferta registrada en el sistema."
        )]
        [SwaggerResponse(200, "Oferta eliminada correctamente")]
        [SwaggerResponse(404, "Oferta no encontrada")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<ApiResponse<int>>> EliminarOferta(int id)
        {
            var response = await _ofertaService.EliminarOfertaAsync(id);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }
    }
}
