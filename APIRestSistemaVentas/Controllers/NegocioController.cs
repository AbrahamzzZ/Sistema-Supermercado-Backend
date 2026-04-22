using Domain.Models;
using Domain.Models.Dto.Negocio;
using Infrastructure.Repository.InterfacesServices;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Utilities.IA;
using Utilities.Shared;

namespace APIRestSistemaVentas.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NegocioController : ControllerBase
    {
        /*private readonly NegocioService _negocioService;

        public NegocioController(NegocioService negocioService)
        {
            _negocioService = negocioService;
        }*/

        //Para pruebas unitarias, descomenta este constructor y comenta el constructor anterior.

        private readonly INegocioService _negocioService;

        public NegocioController(INegocioService negocioService)
        {
            _negocioService = negocioService;
        }

        // GET: api/negocio/1
        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Obtener información del negocio",
            Description = "Obtiene la información general del negocio."
        )]
        [SwaggerResponse(200, "Información del negocio obtenida")]
        [SwaggerResponse(404, "Negocio no encontrado")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<ApiResponse<Negocio>>> GetNegocio(int id)
        {
            var response = await _negocioService.ObtenerNegocioAsync(id);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }

        // PUT: api/negocio/1
        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Actualizar información del negocio",
            Description = "Actualiza los datos del negocio registrados en el sistema."
        )]
        [SwaggerResponse(200, "Negocio actualizado correctamente")]
        [SwaggerResponse(400, "Error en los datos enviados")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<ApiResponse<object>>> EditarNegocio(int id, [FromBody] Negocio negocio)
        {
            var response = await _negocioService.EditarNegocioAsync(negocio);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        // GET: api/negocio/producto-mas-comprado
        [HttpGet("producto-mas-comprado")]
        [SwaggerOperation(
            Summary = "Productos más comprados",
            Description = "Obtiene los productos que han sido comprados con mayor frecuencia."
        )]
        [SwaggerResponse(200, "Listado obtenido correctamente")]
        [SwaggerResponse(404, "Información no encontrada")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<List<ProductoMasComprado>>> ObtenerProductosMasComprados()
        {
            var response = await _negocioService.ObtenerProductoMasComprado();
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }

        // GET: api/negocio/producto-mas-vendido
        [HttpGet("producto-mas-vendido")]
        [SwaggerOperation(
            Summary = "Productos más vendidos",
            Description = "Obtiene los productos con mayor volumen de ventas."
        )]
        [SwaggerResponse(200, "Listado obtenido correctamente")]
        [SwaggerResponse(404, "Información no encontrada")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<List<ProductoMasVendido>>> ObtenerProductosMasVendidos()
        {
            var response = await _negocioService.ObtenerProductoMasVendido();
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }

        // POST: api/negocio/analisis-ia
        /*[HttpPost("analisis-ia")]
        [SwaggerOperation(
            Summary = "Análisis con IA",
            Description = "Genera un análisis del negocio utilizando inteligencia artificial basado en el prompt enviado."
        )]
        [SwaggerResponse(200, "Análisis generado correctamente")]
        [SwaggerResponse(400, "Error en la solicitud")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult> AnalisisIA([FromBody] AnalisisIARequest request)
        {
            var response = await _negocioService.AnalisisIA(request.Prompt);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }*/

        // GET: api/negocio/top-clientes
        [HttpGet("top-clientes")]
        [SwaggerOperation(
            Summary = "Top clientes",
            Description = "Obtiene los clientes que generan mayores compras."
        )]
        [SwaggerResponse(200, "Listado obtenido correctamente")]
        [SwaggerResponse(404, "Información no encontrada")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<List<TopCliente>>> ObtenerTopClientes()
        {
            var response = await _negocioService.ObtenerTopClientes();
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }

        // GET: api/negocio/top-proveedores
        [HttpGet("top-proveedores")]
        [SwaggerOperation(
            Summary = "Top proveedores",
            Description = "Obtiene los proveedores más utilizados en compras."
        )]
        [SwaggerResponse(200, "Listado obtenido correctamente")]
        [SwaggerResponse(404, "Información no encontrada")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<List<TopProveedor>>> ObtenerProveedorPreferido()
        {
            var response = await _negocioService.ObtenerTopProveedores();
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }

        // GET: api/negocio/viaje-transportista
        [HttpGet("viajes-transportista")]
        [SwaggerOperation(
            Summary = "Viajes por transportista",
            Description = "Obtiene estadísticas de viajes realizados por transportistas."
        )]
        [SwaggerResponse(200, "Listado obtenido correctamente")]
        [SwaggerResponse(404, "Información no encontrada")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<List<ViajesTransportista>>> ObtenerTransportistaViajes()
        {
            var response = await _negocioService.ObtenerViajesTransportista();
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }

        // GET: api/negocio/empleados-productivos
        [HttpGet("empleados-productivos")]
        [SwaggerOperation(
            Summary = "Empleados más productivos",
            Description = "Obtiene los empleados con mayor productividad en ventas."
        )]
        [SwaggerResponse(200, "Listado obtenido correctamente")]
        [SwaggerResponse(404, "Información no encontrada")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<List<EmpleadoProductivo>>> ObtenerEmpleadosProductivos()
        {
            var response = await _negocioService.ObtenerEmpleadosProductivos();
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }
    }
}
