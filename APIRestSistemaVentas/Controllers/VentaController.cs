using DataBaseFirst.Models.Dto;
using Infrastructure.Repository.InterfacesServices;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult> GetObtenerNumeroDocumento()
        {
            var response = await _ventaService.ObtenerNumeroDocumentoAsync();
            return Ok(response);
        }

        // GET: api/venta/{numeroDocumento}
        [HttpGet("{numeroDocumento}")]
        public async Task<ActionResult<VentaRespuesta>> GetObtenerVenta(string numeroDocumento)
        {
            var venta = await _ventaService.ObtenerVentaAsync(numeroDocumento);
            return Ok(venta);
        }

        // GET: api/venta/detalles/{idVenta}
        [HttpGet("detalles/{idVenta}")]
        public async Task<ActionResult<List<DetalleVentas>>> GetObtenerDetallesVenta(int idVenta)
        {
            var detalles = await _ventaService.ObtenerDetallesVentaAsync(idVenta);
            return Ok(detalles);
        }

        // POST: api/venta
        [HttpPost]
        public async Task<IActionResult> PostRegistrarVenta([FromBody] Ventas ventaDto)
        {
            var response = await _ventaService.RegistrarVentaAsync(ventaDto);
            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
