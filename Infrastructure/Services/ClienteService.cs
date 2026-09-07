using Domain.Models;
using Infrastructure.Repository.InterfacesRepository;
using FluentValidation;
using Infrastructure.Repository;
using Infrastructure.Repository.InterfacesServices;
using Microsoft.Data.SqlClient;
using Utilities.Shared;
using Domain.Models.Dto.Response.Cliente;
using Infrastructure.Repository.InterfacesBusiness;

namespace Infrastructure.Services
{
    public class ClienteService : IClienteService
    {
        private readonly ClienteRepository _clienteRepository;
        private readonly IValidator<Cliente> _validator;
        private readonly ICurrentUser _currentUserService;
        private readonly IAuditoriaService _auditoriaService;


        public ClienteService(ClienteRepository clienteRepository, IValidator<Cliente> validator, ICurrentUser currentUserService, IAuditoriaService auditoriaService)
        {
            _clienteRepository = clienteRepository;
            _validator = validator;
            _currentUserService = currentUserService;
            _auditoriaService = auditoriaService;
        }


        //Para pruebas unitarias, descomenta este constructor y comenta el constructor anterior.

        /*readonly IClienteRepository _clienteRepository;
        private readonly IValidator<Cliente> _validator;
        public ClienteService(IClienteRepository clienteRepository, IValidator<Cliente> validator)
        {
            _clienteRepository = clienteRepository;
            _validator = validator;
        }*/

        public async Task<ApiResponse<List<ClienteResponse>>> ListarClientesAsync()
        {
            try
            {
                var listaClientes = await _clienteRepository.ListarClientesAsync();

                if (listaClientes == null || listaClientes.Count == 0)
                {
                    await _auditoriaService.RegistrarFalloAsync("Listar Clientes", "No hay clientes registrados");

                    return new ApiResponse<List<ClienteResponse>>{ IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY, Data = listaClientes };
                }

                await _auditoriaService.RegistrarExitoAsync("Listar Clientes", $"Se obtuvieron {listaClientes.Count} clientes");

                return new ApiResponse<List<ClienteResponse>>{ IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = listaClientes };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Listar Clientes", ex);
                throw;
            }
        }

        public async Task<ApiResponse<Paginacion<ClienteResponse>>> ListarClientesPaginacionAsync(int pageNumber, int pageSize, string filtro = "")
        {
            try
            {
                var pagedResult = await _clienteRepository.ListarClientesPaginacionAsync(pageNumber, pageSize, filtro);

                if (pagedResult.Items == null || pagedResult.Items.Count == 0)
                {
                    await _auditoriaService.RegistrarFalloAsync("Listar Clientes Paginación", $"No hay resultados para el filtro: {filtro}");

                    return new ApiResponse<Paginacion<ClienteResponse>>{ IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY, Data = pagedResult };
                }

                await _auditoriaService.RegistrarExitoAsync("Listar Clientes Paginación", $"Página {pageNumber}, {pagedResult.Items.Count} clientes. Filtro: {filtro}");

                return new ApiResponse<Paginacion<ClienteResponse>>{ IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = pagedResult };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Listar Clientes Paginación", ex);
                throw;
            }
        }

        public async Task<ApiResponse<ClienteResponse>> ObtenerClienteAsync(int idCliente)
        {
            try
            {
                var cliente = await _clienteRepository.ObtenerClienteAsync(idCliente);

                if (cliente == null)
                {
                    await _auditoriaService.RegistrarFalloAsync( "Obtener Cliente", $"Cliente con ID {idCliente} no encontrado");

                    return new ApiResponse<ClienteResponse>{ IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };
                }

                await _auditoriaService.RegistrarExitoAsync("Obtener Cliente", $"Cliente {cliente.Codigo} obtenido correctamente");

                return new ApiResponse<ClienteResponse>{ IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = cliente };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Obtener Cliente", ex);
                throw;
            }
        }

        public async Task<ApiResponse<object>> RegistrarClienteAsync(Cliente cliente)
        {
            if (cliente == null)
                return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_NULL};
            
            var validationResult = await _validator.ValidateAsync(cliente);

            if (!validationResult.IsValid)
                return new ApiResponse<object> { IsSuccess = false, Message = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage)) };

            var clientes = await _clienteRepository.ListarClientesAsync();
            if (clientes.Any(c => c.Codigo == cliente.Codigo))
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_CODE_EXITS };

            if (clientes.Any(c => c.Cedula == cliente.Cedula))
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_CEDULA_EXITS };

            if (clientes.Any(c => c.Telefono == cliente.Telefono))
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_PHONE_EXITS };

            var idUsuario = _currentUserService.GetUserId();

            var result = await _clienteRepository.RegistrarClienteAsync(cliente, idUsuario);
            if (result > 0)
                return new ApiResponse<object> { IsSuccess = true, Message = Mensajes.MESSAGE_REGISTER };

            return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_REGISTER_FAILLED };
        }

        public async Task<ApiResponse<object>> EditarClienteAsync(Cliente cliente)
        {
            try
            {
                if (cliente == null)
                {
                    await _auditoriaService.RegistrarFalloAsync("Editar Cliente", "Cliente nulo");

                    return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_NULL };
                }

                var validationResult = await _validator.ValidateAsync(cliente);
                if (!validationResult.IsValid)
                {
                    var errores = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage));
                    await _auditoriaService.RegistrarFalloAsync( "Editar Cliente", $"Validación fallida: {errores}");

                    return new ApiResponse<object>{ IsSuccess = false, Message = errores };
                }

                var clienteExistente = await _clienteRepository.ObtenerClienteAsync(cliente.Id_Cliente);
                if (clienteExistente == null)
                {
                    await _auditoriaService.RegistrarFalloAsync("Editar Cliente", $"Cliente con ID {cliente.Id_Cliente} no encontrado");

                    return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };
                }

                var clientes = await _clienteRepository.ListarClientesAsync();
                if (clientes.Any(c => c.Cedula == cliente.Cedula && c.Id_Cliente != cliente.Id_Cliente))
                {
                    await _auditoriaService.RegistrarFalloAsync("Editar Cliente", $"Cédula {cliente.Cedula} ya existe en otro cliente");

                    return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_CEDULA_EXITS };
                }
                else if (clientes.Any(c => c.Telefono == cliente.Telefono && c.Id_Cliente != cliente.Id_Cliente))
                {
                    await _auditoriaService.RegistrarFalloAsync("Editar Cliente", $"Teléfono {cliente.Telefono} ya existe en otro cliente");

                    return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_PHONE_EXITS };
                }

                var idUsuario = _currentUserService.GetUserId();
                var result = await _clienteRepository.EditarClienteAsync(cliente, idUsuario);

                if (result > 0)
                {
                    await _auditoriaService.RegistrarExitoAsync("Editar Cliente", $"Cliente {cliente.Codigo} actualizado exitosamente");

                    return new ApiResponse<object>{ IsSuccess = true, Message = Mensajes.MESSAGE_UPDATE };
                }

                await _auditoriaService.RegistrarFalloAsync("Editar Cliente", $"No se pudo actualizar el cliente {cliente.Codigo}");

                return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_UPDATE_FAILLED };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Editar Cliente", ex);
                throw;
            }
        }

        public async Task<ApiResponse<int>> EliminarClienteAsync(int id)
        {
            try
            {
                var existe = await _clienteRepository.ObtenerClienteAsync(id);
                if (existe == null)
                {
                    await _auditoriaService.RegistrarFalloAsync("Eliminar Cliente", $"Cliente con ID {id} no encontrado");

                    return new ApiResponse<int>{ IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };
                }

                var result = await _clienteRepository.EliminarClienteAsync(id);

                if (result > 0)
                {
                    await _auditoriaService.RegistrarExitoAsync("Eliminar Cliente", $"Cliente {existe.Codigo} eliminado exitosamente");

                    return new ApiResponse<int>{ IsSuccess = true, Message = Mensajes.MESSAGE_DELETE };
                }

                await _auditoriaService.RegistrarFalloAsync("Eliminar Cliente", $"No se pudo eliminar el cliente {existe.Codigo}");

                return new ApiResponse<int>{ IsSuccess = false, Message = Mensajes.MESSAGE_DELETE_FAILLED };
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                await _auditoriaService.RegistrarErrorAsync("Eliminar Cliente",  new Exception("No se puede eliminar: cliente tiene ventas asociadas"));

                return new ApiResponse<int>{ IsSuccess = false, Message = "No se puede eliminar al cliente porque tiene ventas asociadas." };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Eliminar Cliente", ex);
                throw;
            }
        }
    }
}
