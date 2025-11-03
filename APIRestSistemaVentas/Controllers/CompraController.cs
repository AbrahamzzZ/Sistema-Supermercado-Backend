using DataBaseFirst.Models.Dto;
using Infrastructure.Repository.InterfacesServices;
using DataBaseFirst.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace APIRestSistemaVentas.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CompraController : ControllerBase
    {
        private readonly CompraService _compraService;

        public CompraController(CompraService compraService)
        {
            _compraService = compraService;
        }

        //Para pruebas unitarias, descomenta este constructor y comenta el constructor anterior.

        /*private readonly ICompraService _compraService;

        public CompraController(ICompraService compraService)
        {
            _compraService = compraService;
        }*/

        // GET: api/compra/numero-documento
        [HttpGet("numero-documento")]
        public async Task<ActionResult> GetObtenerNumeroDocumento()
        {
            var response = await _compraService.ObtenerNumeroDocumentoAsync();
            return Ok(response);
        }

        // GET: api/compra/{numeroDocumento}
        [HttpGet("{numeroDocumento}")]
        public async Task<ActionResult<CompraRespuesta>> GetObtenerCompra(string numeroDocumento)
        {
            var compra = await _compraService.ObtenerCompraAsync(numeroDocumento);
            return Ok(compra);
        }

        // GET: api/compra/detalles/{idCompra}
        [HttpGet("detalles/{idCompra}")]
        public async Task<ActionResult<List<DetalleCompras>>> GetObtenerDetallesCompra(int idCompra)
        {
            var detalles = await _compraService.ObtenerDetallesCompraAsync(idCompra);
            return Ok(detalles);
        }

        // POST: api/compra
        [HttpPost]
        public async Task<IActionResult> PostRegistrarCompra([FromBody] Compras compraDto)
        {
            var response = await _compraService.RegistrarCompraAsync(compraDto);
            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
