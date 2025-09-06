using DataBaseFirst.Models;
using DataBaseFirst.Repository.InterfacesRepository;
using FluentValidation;
using Infrastructure.Repository;
using Infrastructure.Repository.InterfacesServices;
using Microsoft.Data.SqlClient;
using Utilities.Shared;

namespace Infrastructure.Services
{
    public class ClienteService : IClienteService
    {
        private readonly ClienteRepository _clienteRepository;
        private readonly IValidator<Cliente> _validator;

        public ClienteService(ClienteRepository clienteRepository, IValidator<Cliente> validator)
        {
            _clienteRepository = clienteRepository;
            _validator = validator;
        }


        //Para pruebas unitarias, descomenta este constructor y comenta el constructor anterior.

        /*readonly IClienteRepository _clienteRepository;

        public ClienteService(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }*/

        public async Task<ApiResponse<List<Cliente>>> ListarClientesAsync()
        {
            var listaClientes = await _clienteRepository.ListarClientesAsync();

            if (listaClientes == null || listaClientes.Count == 0)
                return new ApiResponse<List<Cliente>> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY, Data = listaClientes };

            return new ApiResponse<List<Cliente>> { IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = listaClientes };
        }

        public async Task<ApiResponse<Paginacion<Cliente>>> ListarClientesPaginacionAsync(int pageNumber, int pageSize)
        {
            var pagedResult = await _clienteRepository.ListarClientesPaginacionAsync(pageNumber, pageSize);

            if (pagedResult.Items == null || pagedResult.Items.Count == 0)
            {
                return new ApiResponse<Paginacion<Cliente>> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY, Data = pagedResult };
            }

            return new ApiResponse<Paginacion<Cliente>> { IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = pagedResult };
        }

        public async Task<ApiResponse<Cliente>> ObtenerClienteAsync(int idCliente)
        {
            var cliente = await _clienteRepository.ObtenerClienteAsync(idCliente);

            if (cliente == null)
            {
                return new ApiResponse<Cliente> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };
            }

            return new ApiResponse<Cliente> { IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = cliente };
        }

        public async Task<ApiResponse<object>> RegistrarClienteAsync(Cliente cliente)
        {
            var validationResult = await _validator.ValidateAsync(cliente);

            if (!validationResult.IsValid)
                return new ApiResponse<object> { IsSuccess = false, Message = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage)) };

            var clientes = await _clienteRepository.ListarClientesAsync();
            if (clientes.Any(c => c.Codigo == cliente.Codigo))
                return new ApiResponse<object> { IsSuccess = false, Message = "El código ya existe" };

            if (clientes.Any(c => c.Cedula == cliente.Cedula))
                return new ApiResponse<object> { IsSuccess = false, Message = "El cédula ya existe" };

            if (clientes.Any(c => c.Telefono == cliente.Telefono))
                return new ApiResponse<object> { IsSuccess = false, Message = "El télefono ya existe" };

            var result = await _clienteRepository.RegistrarClienteAsync(cliente);
            if (result > 0)
                return new ApiResponse<object> { IsSuccess = true, Message = Mensajes.MESSAGE_REGISTER };

            return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_REGISTER_FAILLED };
        }


        public async Task<ApiResponse<object>> EditarClienteAsync(Cliente cliente)
        {
            if (cliente == null)
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_NULL };

            var validationResult = await _validator.ValidateAsync(cliente);
            if (!validationResult.IsValid)
                return new ApiResponse<object> { IsSuccess = false, Message = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage)) };

            var clienteExistente = await _clienteRepository.ObtenerClienteAsync(cliente.Id_Cliente);
            if (clienteExistente == null)
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };

            var clientes = await _clienteRepository.ListarClientesAsync();
            if (clientes.Any(c =>c.Cedula == cliente.Cedula && c.Id_Cliente != cliente.Id_Cliente))
            {
                return new ApiResponse<object> { IsSuccess = false, Message = "El cédula ya existe." };
            }else if (clientes.Any(c => c.Telefono == cliente.Telefono && c.Id_Cliente != cliente.Id_Cliente))
            {
                return new ApiResponse<object> { IsSuccess = false, Message = "El teléfono ya existe." };
            }

            var result = await _clienteRepository.EditarClienteAsync(cliente);
            if (result > 0)
                return new ApiResponse<object> { IsSuccess = true, Message = Mensajes.MESSAGE_UPDATE };

            return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_UPDATE_FAILLED };
        }

        public async Task<ApiResponse<int>> EliminarClienteAsync(int id)
        {
            try
            {
                var existe = await _clienteRepository.ObtenerClienteAsync(id);
                if (existe == null)
                {
                    return new ApiResponse<int> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };
                }

                var result = await _clienteRepository.EliminarClienteAsync(id);

                if (result > 0)
                    return new ApiResponse<int> { IsSuccess = true, Message = Mensajes.MESSAGE_DELETE };

                return new ApiResponse<int> { IsSuccess = false, Message = Mensajes.MESSAGE_DELETE_FAILLED };
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                return new ApiResponse<int> { IsSuccess = false, Message = "No se puede eliminar al cliente porque tiene ventas asociadas." };
            }
        }
    }
}
