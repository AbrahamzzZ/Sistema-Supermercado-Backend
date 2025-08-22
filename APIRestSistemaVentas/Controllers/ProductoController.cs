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
    public class ProductoController : ControllerBase
    {

        private readonly ProductoService _productoService;

        public ProductoController(ProductoService productoService)
        {
            _productoService = productoService;
        }

        // GET: api/producto
        [HttpGet]
        public async Task<ActionResult<ApiResponse<ProductoCategoria>>> GetProductos()
        {
            var productos = await _productoService.ListarProductosAsync();
            return Ok(productos);
        }

        [HttpGet("paginacion")]
        public async Task<ActionResult<ApiResponse<Paginacion<ProductoCategoria>>>> GetProductosPaginacion(int pageNumber = 1, int pageSize = 10)
        {
            var result = await _productoService.ListarProductosPaginacionAsync(pageNumber, pageSize);
            return Ok(result);
        }

        // GET: api/producto/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProducto(int id)
        {
            var response = await _productoService.ObtenerProductoAsync(id);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }

        // POST: api/producto
        [HttpPost]
        public async Task<ActionResult<ApiResponse<object>>> RegistrarUsuario([FromBody] Producto producto)
        {
            var response = await _productoService.RegistrarProductoAsync(producto);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        // PUT: api/producto/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<object>>>  EditarProducto(int id, [FromBody] Producto producto)
        {
            var response = await _productoService.EditarProductoAsync(producto);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        // DELETE: api/producto/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<int>>> EliminarProducto(int id)
        {
            var response = await _productoService.EliminarProductoAsync(id);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }
    }
}
