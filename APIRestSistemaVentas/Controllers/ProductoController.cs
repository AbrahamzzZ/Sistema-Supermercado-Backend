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
    public class ProductoController : ControllerBase
    {

        /*private readonly ProductoService _productoService;

        public ProductoController(ProductoService productoService)
        {
            _productoService = productoService;
        }*/

        //Para pruebas unitarias, descomenta este constructor y comenta el constructor anterior.

        private readonly IProductoService _productoService;

        public ProductoController(IProductoService productoService)
        {
            _productoService = productoService;
        }

        // GET: api/producto
        [HttpGet]
        [SwaggerOperation(
            Summary = "Listar productos",
            Description = "Obtiene la lista completa de productos registrados en el sistema."
        )]
        [SwaggerResponse(200, "Lista de productos obtenida correctamente")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<ApiResponse<ProductoCategoria>>> GetProductos()
        {
            var productos = await _productoService.ListarProductosAsync();
            return Ok(productos);
        }

        // GET: api/producto/paginacion
        [HttpGet("paginacion")]
        [SwaggerOperation(
            Summary = "Listar productos con paginación",
            Description = "Obtiene la lista de productos utilizando paginación."
        )]
        [SwaggerResponse(200, "Lista paginada obtenida correctamente")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<ApiResponse<Paginacion<ProductoCategoria>>>> GetProductosPaginacion(int pageNumber = 1, int pageSize = 10)
        {
            var result = await _productoService.ListarProductosPaginacionAsync(pageNumber, pageSize);
            return Ok(result);
        }

        // GET: api/producto/5
        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Obtener producto por ID",
            Description = "Obtiene la información de un producto específico."
        )]
        [SwaggerResponse(200, "Producto encontrado")]
        [SwaggerResponse(404, "Producto no encontrado")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<Producto>> GetProducto(int id)
        {
            var response = await _productoService.ObtenerProductoAsync(id);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }

        // POST: api/producto
        [HttpPost]
        [SwaggerOperation(
            Summary = "Registrar producto",
            Description = "Registra un nuevo producto en el sistema."
        )]
        [SwaggerResponse(200, "Producto registrado correctamente")]
        [SwaggerResponse(400, "Error en los datos enviados")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<ApiResponse<object>>> RegistrarUsuario([FromBody] Producto producto)
        {
            var response = await _productoService.RegistrarProductoAsync(producto);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        // PUT: api/producto/5
        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Actualizar producto",
            Description = "Actualiza la información de un producto existente."
        )]
        [SwaggerResponse(200, "Producto actualizado correctamente")]
        [SwaggerResponse(400, "Error en los datos enviados")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<ApiResponse<object>>>  EditarProducto(int id, [FromBody] Producto producto)
        {
            var response = await _productoService.EditarProductoAsync(producto);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        // DELETE: api/producto/5
        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Eliminar producto",
            Description = "Elimina un producto del sistema."
        )]
        [SwaggerResponse(200, "Producto eliminado correctamente")]
        [SwaggerResponse(404, "Producto no encontrado")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<ApiResponse<int>>> EliminarProducto(int id)
        {
            var response = await _productoService.EliminarProductoAsync(id);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }
    }
}
