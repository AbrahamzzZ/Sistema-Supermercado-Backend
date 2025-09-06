using DataBaseFirst.Models;
using DataBaseFirst.Repository.InterfacesServices;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Utilities.Shared;

namespace APIRestSistemaVentas.Controllers
{
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
        public async Task<ActionResult<ApiResponse<Proveedor>>> GetProveedores()
        {
            var proveedores = await _proveedorService.ListarProveedoresAsync();
            return Ok(proveedores);
        }

        // GET: api/proveedor/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Proveedor>> GetProveedor(int id)
        {
            var response = await _proveedorService.ObtenerProveedorAsync(id);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }

        [HttpGet("paginacion")]
        public async Task<ActionResult<ApiResponse<Paginacion<Proveedor>>>> GetProveedoresPaginacion(int pageNumber = 1, int pageSize = 10)
        {
            var result = await _proveedorService.ListarProveedoresPaginacionAsync(pageNumber, pageSize);
            return Ok(result);
        }

        // POST: api/proveedor
        [HttpPost]
        public async Task<ActionResult<ApiResponse<object>>> RegistrarProveedor([FromBody] Proveedor proveedor)
        {
            var response = await _proveedorService.RegistrarProveedorAsync(proveedor);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        // PUT: api/proveedor/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> EditarProveedor(int id, [FromBody] Proveedor proveedor)
        {
            var response = await _proveedorService.EditarProveedorAsync(proveedor);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        // DELETE: api/proveedor/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<int>>> EliminarProveedor(int id)
        {
            var response = await _proveedorService.EliminarProveedorAsync(id);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }
    }
}
