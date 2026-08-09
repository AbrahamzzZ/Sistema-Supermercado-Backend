using Domain.Models.Dto.Compra;
using Infrastructure.Repository.InterfacesServices;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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
        [SwaggerOperation(
            Summary = "Obtener número de documento de compra",
            Description = "Genera y retorna el siguiente número de documento disponible para registrar una compra."
        )]
        [SwaggerResponse(200, "Número de documento obtenido correctamente")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult> GetObtenerNumeroDocumento()
        {
            var response = await _compraService.ObtenerNumeroDocumentoAsync();
            return Ok(response);
        }

        // GET: api/compra/{numeroDocumento}
        [HttpGet("{numeroDocumento}")]
        [SwaggerOperation(
            Summary = "Obtener compra por número de documento",
            Description = "Obtiene la información de una compra registrada utilizando su número de documento."
        )]
        [SwaggerResponse(200, "Compra encontrada")]
        [SwaggerResponse(404, "Compra no encontrada")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<CompraRespuesta>> GetObtenerCompra(string numeroDocumento)
        {
            var compra = await _compraService.ObtenerCompraAsync(numeroDocumento);
            return Ok(compra);
        }

        // GET: api/compra/detalles/{idCompra}
        [HttpGet("detalles/{idCompra}")]
        [SwaggerOperation(
            Summary = "Obtener detalles de compra",
            Description = "Obtiene la lista de productos incluidos en una compra específica."
        )]
        [SwaggerResponse(200, "Detalles de compra obtenidos correctamente")]
        [SwaggerResponse(404, "Compra no encontrada")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<List<DetalleCompras>>> GetObtenerDetallesCompra(int idCompra)
        {
            var detalles = await _compraService.ObtenerDetallesCompraAsync(idCompra);
            return Ok(detalles);
        }

        // POST: api/compra
        [HttpPost]
        [SwaggerOperation(
            Summary = "Registrar compra",
            Description = "Registra una nueva compra en el sistema junto con sus detalles."
        )]
        [SwaggerResponse(200, "Compra registrada correctamente")]
        [SwaggerResponse(400, "Error en los datos enviados")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<IActionResult> PostRegistrarCompra([FromBody] Compras compraDto)
        {
            var response = await _compraService.RegistrarCompraAsync(compraDto);
            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
