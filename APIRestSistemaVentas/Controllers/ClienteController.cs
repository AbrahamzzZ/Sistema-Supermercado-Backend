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
    public class ClienteController : ControllerBase
    {
        /*private readonly ClienteService _clienteService;

        public ClienteController(ClienteService clienteService)
        {
            _clienteService = clienteService;
        }*/

        //Para pruebas unitarias, descomenta este constructor y comenta el constructor anterior.

        private readonly IClienteService _clienteService;

        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        // GET: api/cliente
        [HttpGet]
        [SwaggerOperation(
            Summary = "Listar clientes",
            Description = "Obtiene la lista completa de clientes registrados en el sistema."
        )]
        [SwaggerResponse(200, "Lista de clientes obtenida correctamente")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<ApiResponse<Cliente>>> GetClientes()
        {
            var clientes = await _clienteService.ListarClientesAsync();
            return Ok(clientes);
        }

        // GET: api/cliente/paginacion
        [HttpGet("paginacion")]
        [SwaggerOperation(
            Summary = "Listar clientes con paginación",
            Description = "Obtiene la lista de clientes utilizando paginación."
        )]
        [SwaggerResponse(200, "Lista paginada obtenida correctamente")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<ApiResponse<Paginacion<Cliente>>>> GetClientesPaginacion(int pageNumber = 1, int pageSize = 10)
        {
            var result = await _clienteService.ListarClientesPaginacionAsync(pageNumber, pageSize);
            return Ok(result);
        }

        // GET: api/cliente/5
        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Obtener cliente por ID",
            Description = "Obtiene la información de un cliente específico mediante su identificador."
        )]
        [SwaggerResponse(200, "Cliente encontrado")]
        [SwaggerResponse(404, "Cliente no encontrado")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<Cliente>> GetCliente(int id)
        {
            var response = await _clienteService.ObtenerClienteAsync(id);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }

        // POST: api/cliente
        [HttpPost]
        [SwaggerOperation(
            Summary = "Registrar cliente",
            Description = "Registra un nuevo cliente en el sistema."
        )]
        [SwaggerResponse(200, "Cliente registrado correctamente")]
        [SwaggerResponse(400, "Error en los datos enviados")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<ApiResponse<object>>> RegistrarCliente([FromBody] Cliente cliente)
        {
            var response = await _clienteService.RegistrarClienteAsync(cliente);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        // PUT: api/cliente/5
        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Actualizar cliente",
            Description = "Actualiza la información de un cliente existente."
        )]
        [SwaggerResponse(200, "Cliente actualizado correctamente")]
        [SwaggerResponse(400, "Error en los datos enviados")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<ApiResponse<object>>> EditarCliente(int id, [FromBody] Cliente cliente)
        {
            var response = await _clienteService.EditarClienteAsync(cliente);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        // DELETE: api/cliente/5
        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Eliminar cliente",
            Description = "Elimina un cliente del sistema mediante su identificador."
        )]
        [SwaggerResponse(200, "Cliente eliminado correctamente")]
        [SwaggerResponse(404, "Cliente no encontrado")]
        [SwaggerResponse(401, "No autorizado")]
        public async Task<ActionResult<ApiResponse<int>>> EliminarCliente(int id)
        {
            var response = await _clienteService.EliminarClienteAsync(id);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }
    }
}
