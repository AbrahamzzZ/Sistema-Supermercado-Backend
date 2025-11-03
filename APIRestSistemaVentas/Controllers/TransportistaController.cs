using DataBaseFirst.Models;
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
        public async Task<ActionResult<ApiResponse<Transportistum>>> GetTransportistas()
        {
            var transportistas = await _transportistaService.ListarTransportistasAsync();
            return Ok(transportistas);
        }

        [HttpGet("paginacion")]
        public async Task<ActionResult<ApiResponse<Paginacion<Transportistum>>>> GetTransportistasPaginacion(int pageNumber = 1, int pageSize = 10)
        {
            var result = await _transportistaService.ListarTransportistasPaginacionAsync(pageNumber, pageSize);
            return Ok(result);
        }

        // GET: api/transportista/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Transportistum>> GetTransportista(int id)
        {
            var response = await _transportistaService.ObtenerTransportistaAsync(id);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }

        // POST: api/transportista
        [HttpPost]
        public async Task<ActionResult<ApiResponse<object>>> RegistrarProveedor([FromBody] Transportistum transportista)
        {
            var response = await _transportistaService.RegistrarTransportistaAsync(transportista);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        // PUT: api/transportista/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> EditarTransportista(int id, [FromBody] Transportistum transportista)
        {
            var response = await _transportistaService.EditarTransportistaAsync(transportista);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        // DELETE: api/transportista/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<int>>> EliminarTransportista(int id)
        {
            var response = await _transportistaService.EliminarTransportistaAsync(id);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }
    }
}
