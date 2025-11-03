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
    public class SucursalController : ControllerBase
    {
        private readonly SucursalService _sucursalService;

        public SucursalController(SucursalService sucursalService)
        {
            _sucursalService = sucursalService;
        }

        //Para pruebas unitarias, descomenta este constructor y comenta el constructor anterior.

        /*private readonly ISucursalService _sucursalService;

        public SucursalController(ISucursalService sucursalService)
        {
            _sucursalService = sucursalService;
        }*/

        // GET: api/sucursal
        [HttpGet]
        public async Task<ActionResult<ApiResponse<Sucursal>>> GetSucursales()
        {
            var sucursales = await _sucursalService.ListarSucursalesAsync();
            return Ok(sucursales);
        }

        [HttpGet("paginacion")]
        public async Task<ActionResult<ApiResponse<Paginacion<Sucursal>>>> GetSucursalesPaginacion(int pageNumber = 1, int pageSize = 10)
        {
            var result = await _sucursalService.ListarSucursalesPaginacionAsync(pageNumber, pageSize);
            return Ok(result);
        }

        // GET: api/sucursal/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Sucursal>> GetSucursal(int id)
        {
            var response = await _sucursalService.ObtenerSucursalAsync(id);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }

        // POST: api/sucursal
        [HttpPost]
        public async Task<ActionResult<ApiResponse<object>>> RegistrarSucursal([FromBody] Sucursal sucursal)
        {
            var response = await _sucursalService.RegistrarSucursalAsync(sucursal);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        // PUT: api/sucursal/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> EditarSucursal(int id, [FromBody] Sucursal sucursal)
        {
            var response = await _sucursalService.EditarSucursalAsync(sucursal);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        // DELETE: api/sucursal/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<int>>> EliminarSucursal(int id)
        {
            var response = await _sucursalService.EliminarSucursalAsync(id);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }
    }
}
