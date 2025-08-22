using DataBaseFirst.Models;
using DataBaseFirst.Models.Dto;
using DataBaseFirst.Repository.InterfacesServices;
using DataBaseFirst.Services;
using Microsoft.AspNetCore.Mvc;
using Utilities.Shared;

namespace APIRestSistemaVentas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfertaController : ControllerBase
    {
        private readonly OfertaService _ofertaService;

        public OfertaController(OfertaService ofertaService)
        {
            _ofertaService = ofertaService;
        }

        // GET: api/oferta
        [HttpGet]
        public async Task<ActionResult<ApiResponse<OfertaProducto>>> GetOfertas()
        {
            var ofertas = await _ofertaService.ListarOfertasAsync();
            return Ok(ofertas);
        }

        [HttpGet("paginacion")]
        public async Task<ActionResult<ApiResponse<Paginacion<OfertaProducto>>>> GetOfertasPaginacion(int pageNumber = 1, int pageSize = 10)
        {
            var result = await _ofertaService.ListarOfertasPaginacionAsync(pageNumber, pageSize);
            return Ok(result);
        }

        // GET: api/oferta/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ofertum>> GetOferta(int id)
        {
            var response = await _ofertaService.ObtenerOfertaAsync(id);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }

        // POST: api/oferta
        [HttpPost]
        public async Task<ActionResult<ApiResponse<object>>> RegistrarOferta([FromBody] Ofertum oferta)
        {
            var response = await _ofertaService.RegistrarOfertaAsync(oferta);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        // PUT: api/oferta/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> EditarOferta(int id, [FromBody] Ofertum oferta)
        {
            var response = await _ofertaService.EditarOfertaAsync(oferta);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        // DELETE: api/oferta/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<int>>> EliminarOferta(int id)
        {
            var response = await _ofertaService.EliminarOfertaAsync(id);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }
    }
}
