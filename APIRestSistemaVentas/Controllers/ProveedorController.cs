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
    public class ProveedorController : ControllerBase
    {
        private readonly ProveedorService _proveedorService;

        public ProveedorController(ProveedorService proveedorService)
        {
            _proveedorService = proveedorService;
        }

        //Para pruebas unitarias, descomenta este constructor y comenta el constructor anterior.

        /*private readonly IProveedorService _proveedorService;

        public ProveedorController(IProveedorService proveedorService)
        {
            _proveedorService = proveedorService;
        }*/

        // GET: api/proveedor
        [HttpGet]
        [SwaggerOperation(
            Summary = "Listar proveedores",
            Description = "Obtiene la lista completa de proveedores registrados en el sistema."
        )]
        [SwaggerResponse(200, "Lista de proveedores obtenida correctamente")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<ApiResponse<Proveedor>>> GetProveedores()
        {
            var proveedores = await _proveedorService.ListarProveedoresAsync();
            return Ok(proveedores);
        }

        // GET: api/proveedor/paginacion
        [HttpGet("paginacion")]
        [SwaggerOperation(
            Summary = "Listar proveedores con paginación",
            Description = "Obtiene la lista de proveedores utilizando paginación."
        )]
        [SwaggerResponse(200, "Lista paginada obtenida correctamente")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<ApiResponse<Paginacion<Proveedor>>>> GetProveedoresPaginacion(int pageNumber = 1, int pageSize = 10)
        {
            var result = await _proveedorService.ListarProveedoresPaginacionAsync(pageNumber, pageSize);
            return Ok(result);
        }

        // GET: api/proveedor/5
        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Obtener proveedor por ID",
            Description = "Obtiene la información de un proveedor específico mediante su identificador."
        )]
        [SwaggerResponse(200, "Proveedor encontrado")]
        [SwaggerResponse(404, "Proveedor no encontrado")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<Proveedor>> GetProveedor(int id)
        {
            var response = await _proveedorService.ObtenerProveedorAsync(id);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }

        // POST: api/proveedor
        [HttpPost]
        [SwaggerOperation(
            Summary = "Registrar proveedor",
            Description = "Registra un nuevo proveedor en el sistema."
        )]
        [SwaggerResponse(200, "Proveedor registrado correctamente")]
        [SwaggerResponse(400, "Error en los datos enviados")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<ApiResponse<object>>> RegistrarProveedor([FromBody] Proveedor proveedor)
        {
            var response = await _proveedorService.RegistrarProveedorAsync(proveedor);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        // PUT: api/proveedor/5
        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Actualizar proveedor",
            Description = "Actualiza la información de un proveedor existente."
        )]
        [SwaggerResponse(200, "Proveedir actualizado correctamente")]
        [SwaggerResponse(400, "Error en los datos enviados")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<ApiResponse<object>>> EditarProveedor(int id, [FromBody] Proveedor proveedor)
        {
            var response = await _proveedorService.EditarProveedorAsync(proveedor);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        // DELETE: api/proveedor/5
        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Eliminar proveedor",
            Description = "Elimina un proveedor del sistema mediante su identificador."
        )]
        [SwaggerResponse(200, "Proveedor eliminado correctamente")]
        [SwaggerResponse(404, "Proveedor no encontrado")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<ApiResponse<int>>> EliminarProveedor(int id)
        {
            var response = await _proveedorService.EliminarProveedorAsync(id);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }
    }
}
