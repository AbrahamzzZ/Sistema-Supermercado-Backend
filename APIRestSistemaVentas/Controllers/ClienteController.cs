using Domain.Models;
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
    public class ClienteController : ControllerBase
    {
        private readonly ClienteService _clienteService;

        public ClienteController(ClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        //Para pruebas unitarias, descomenta este constructor y comenta el constructor anterior.

        /*private readonly IClienteService _clienteService;

        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }*/

        // GET: api/cliente
        [HttpGet]
        public async Task<ActionResult<ApiResponse<Cliente>>> GetClientes()
        {
            var clientes = await _clienteService.ListarClientesAsync();
            return Ok(clientes);
        }

        [HttpGet("paginacion")]
        public async Task<ActionResult<ApiResponse<Paginacion<Cliente>>>> GetClientesPaginacion(int pageNumber = 1, int pageSize = 10)
        {
            var result = await _clienteService.ListarClientesPaginacionAsync(pageNumber, pageSize);
            return Ok(result);
        }

        // GET: api/cliente/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetCliente(int id)
        {
            var response = await _clienteService.ObtenerClienteAsync(id);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }

        // POST: api/cliente
        [HttpPost]
        public async Task<ActionResult<ApiResponse<object>>> RegistrarCliente([FromBody] Cliente cliente)
        {
            var response = await _clienteService.RegistrarClienteAsync(cliente);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        // PUT: api/cliente/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> EditarCliente(int id, [FromBody] Cliente cliente)
        {
            var response = await _clienteService.EditarClienteAsync(cliente);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        // DELETE: api/cliente/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<int>>> EliminarCliente(int id)
        {
            var response = await _clienteService.EliminarClienteAsync(id);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }
    }
}
