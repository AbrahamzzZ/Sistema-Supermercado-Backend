using Domain.Models.Dto.Venta;
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
    public class VentaController : ControllerBase
    {
        private readonly VentaService _ventaService;

        public VentaController(VentaService ventaService)
        {
            _ventaService = ventaService;
        }

        //Para pruebas unitarias, descomenta este constructor y comenta el constructor anterior.

        /*private readonly IVentaService _ventaService;

        public VentaController(IVentaService ventaService)
        {
            _ventaService = ventaService;
        }*/

        // GET: api/venta/numero-documento
        [HttpGet("numero-documento")]
        [SwaggerOperation(
            Summary = "Obtener número de documento de venta",
            Description = "Genera o retorna el siguiente número de documento disponible para registrar una venta."
        )]
        [SwaggerResponse(200, "Número de documento obtenido correctamente")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult> GetObtenerNumeroDocumento()
        {
            var response = await _ventaService.ObtenerNumeroDocumentoAsync();
            return Ok(response);
        }

        // GET: api/venta/{numeroDocumento}
        [HttpGet("{numeroDocumento}")]
        [SwaggerOperation(
            Summary = "Obtener venta por número de documento",
            Description = "Obtiene la información de una venta registrada utilizando su número de documento."
        )]
        [SwaggerResponse(200, "Venta encontrada")]
        [SwaggerResponse(404, "Venta no encontrada")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<VentaRespuesta>> GetObtenerVenta(string numeroDocumento)
        {
            var venta = await _ventaService.ObtenerVentaAsync(numeroDocumento);
            return Ok(venta);
        }

        // GET: api/venta/detalles/{idVenta}
        [HttpGet("detalles/{idVenta}")]
        [SwaggerOperation(
            Summary = "Obtener detalles de venta",
            Description = "Obtiene la lista de productos incluidos en una venta específica."
        )]
        [SwaggerResponse(200, "Detalles de venta obtenidos correctamente")]
        [SwaggerResponse(404, "Venta no encontrada")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<List<DetalleVentas>>> GetObtenerDetallesVenta(int idVenta)
        {
            var detalles = await _ventaService.ObtenerDetallesVentaAsync(idVenta);
            return Ok(detalles);
        }

        // POST: api/venta
        [HttpPost]
        [SwaggerOperation(
            Summary = "Registrar venta",
            Description = "Registra una nueva venta en el sistema junto con sus detalles."
        )]
        [SwaggerResponse(200, "Venta registrada correctamente")]
        [SwaggerResponse(400, "Error en los datos enviados")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<IActionResult> PostRegistrarVenta([FromBody] Ventas ventaDto)
        {
            var response = await _ventaService.RegistrarVentaAsync(ventaDto);
            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
