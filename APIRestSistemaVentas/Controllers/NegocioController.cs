﻿using DataBaseFirst.Models;
using DataBaseFirst.Models.Dto;
using DataBaseFirst.Services;
using Microsoft.AspNetCore.Mvc;
using Utilities.Shared;
using WebApiRest.Dto;

namespace APIRestSistemaVentas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NegocioController : ControllerBase
    {
        private readonly NegocioService _negocioService;

        public NegocioController(NegocioService negocioService)
        {
            _negocioService = negocioService;
        }

        // GET: api/negocio/1
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Negocio>>> GetNegocio(int id)
        {
            var response = await _negocioService.ObtenerNegocioAsync(id);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }

        // PUT: api/negocio/1
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> EditarNegocio(int id, [FromBody] Negocio negocio)
        {
            var response = await _negocioService.EditarNegocioAsync(negocio);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        // GET: api/negocio/producto-mas-comprado
        [HttpGet("producto-mas-comprado")]
        public async Task<ActionResult<List<ProductoMasComprado>>> ObtenerProductosMasComprados()
        {
            var response = await _negocioService.ObtenerProductoMasComprado();
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }

        // GET: api/negocio/producto-mas-vendido
        [HttpGet("producto-mas-vendido")]
        public async Task<ActionResult<List<ProductoMasVendido>>> ObtenerProductosMasVendidos()
        {
            var response = await _negocioService.ObtenerProductoMasVendido();
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }

        // GET: api/negocio/top-clientes
        [HttpGet("top-clientes")]
        public async Task<ActionResult<List<TopCliente>>> ObtenerTopClientes()
        {
            var response = await _negocioService.ObtenerTopClientes();
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }

        // GET: api/negocio/top-proveedores
        [HttpGet("top-proveedores")]
        public async Task<ActionResult<List<TopProveedor>>> ObtenerProveedorPreferido()
        {
            var response = await _negocioService.ObtenerTopProveedores();
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }

        // GET: api/negocio/viaje-transportista
        [HttpGet("viajes-transportista")]
        public async Task<ActionResult<List<ViajesTransportista>>> ObtenerTransportistaViajes()
        {
            var response = await _negocioService.ObtenerViajesTransportista();
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }

        // GET: api/negocio/empleados-productivos
        [HttpGet("empleados-productivos")]
        public async Task<ActionResult<List<EmpleadoProductivo>>> ObtenerEmpleadosProductivos()
        {
            var response = await _negocioService.ObtenerEmpleadosProductivos();
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }
    }
}
