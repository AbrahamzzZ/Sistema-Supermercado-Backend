using DataBaseFirst.Models;
using DataBaseFirst.Repository;
using DataBaseFirst.Repository.InterfacesServices;
using Microsoft.Data.SqlClient;
using System.Text.RegularExpressions;
using Utilities.Shared;

namespace DataBaseFirst.Services
{
    public class ClienteService : IClienteService
    {
        private readonly ClienteRepository _clienteRepository;

        public ClienteService(ClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

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
            if (cliente == null)
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_VALIDATE };

            if (string.IsNullOrWhiteSpace(cliente.Codigo) || string.IsNullOrWhiteSpace(cliente.Nombres) || string.IsNullOrWhiteSpace(cliente.Apellidos) || string.IsNullOrWhiteSpace(cliente.Cedula) || string.IsNullOrWhiteSpace(cliente.Telefono) || string.IsNullOrWhiteSpace(cliente.Correo_Electronico))
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_EMPTY };

            var regex = new Regex("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$");
            if (!regex.IsMatch(cliente.Nombres) || !regex.IsMatch(cliente.Apellidos))
                return new ApiResponse<object> { IsSuccess = false, Message = "Los nombres y apellidos solo puede contener letras y espacios" };

            var regexCedula = new Regex(@"^\d{10}$");
            if (!regexCedula.IsMatch(cliente.Cedula) || !regexCedula.IsMatch(cliente.Telefono))
                return new ApiResponse<object> { IsSuccess = false, Message = "La cédula y el teléfono deben contener exactamente 10 dígitos numéricos" };

            var regexCorreo = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            if (!regexCorreo.IsMatch(cliente.Correo_Electronico))
                return new ApiResponse<object> { IsSuccess = false, Message = "El correo electrónico no tiene un formato válido" };

            var clientes = await _clienteRepository.ListarClientesAsync();
            if (clientes.Any(c => c.Codigo == cliente.Codigo))
                return new ApiResponse<object> { IsSuccess = false, Message = "El código ya existe" };

            if (clientes.Any(c => c.Cedula?.ToLower() == cliente.Cedula.ToLower()))
                return new ApiResponse<object> { IsSuccess = false, Message = "El cédula ya existe" };

            if (clientes.Any(c => c.Telefono?.ToLower() == cliente.Telefono.ToLower()))
                return new ApiResponse<object> { IsSuccess = false, Message = "El télefono ya existe" };

            var result = await _clienteRepository.RegistrarClienteAsync(cliente);
            if (result > 0)
                return new ApiResponse<object> { IsSuccess = true, Message = Mensajes.MESSAGE_REGISTER };

            return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_REGISTER_FAILLED };
        }


        public async Task<ApiResponse<object>> EditarClienteAsync(Cliente cliente)
        {
            if (cliente == null)
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_VALIDATE };

            if (string.IsNullOrWhiteSpace(cliente.Nombres) || string.IsNullOrWhiteSpace(cliente.Apellidos) || string.IsNullOrWhiteSpace(cliente.Cedula) || string.IsNullOrWhiteSpace(cliente.Telefono) || string.IsNullOrWhiteSpace(cliente.Correo_Electronico))
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_EMPTY };

            var clienteExistente = await _clienteRepository.ObtenerClienteAsync(cliente.Id_Cliente);
            if (clienteExistente == null)
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };

            var regex = new Regex("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$");
            if (!regex.IsMatch(cliente.Nombres) || !regex.IsMatch(cliente.Apellidos))
                return new ApiResponse<object> { IsSuccess = false, Message = "Los nombres y apellidos solo puede contener letras y espacios" };

            var regexCedula = new Regex(@"^\d{10}$");
            if (!regexCedula.IsMatch(cliente.Cedula) || !regexCedula.IsMatch(cliente.Telefono))
                return new ApiResponse<object> { IsSuccess = false, Message = "La cédula y el teléfono deben contener exactamente 10 dígitos numéricos" };

            var regexCorreo = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            if (!regexCorreo.IsMatch(cliente.Correo_Electronico))
                return new ApiResponse<object> { IsSuccess = false, Message = "El correo electrónico no tiene un formato válido" };

            var clientes = await _clienteRepository.ListarClientesAsync();
            if (clientes.Any(c =>c.Cedula == cliente.Cedula && c.Id_Cliente != cliente.Id_Cliente) || clientes.Any(c =>c.Telefono == cliente.Telefono && c.Id_Cliente != cliente.Id_Cliente))
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
