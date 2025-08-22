using DataBaseFirst.Models;
using DataBaseFirst.Repository;
using DataBaseFirst.Repository.InterfacesServices;
using Microsoft.Data.SqlClient;
using System.Text.RegularExpressions;
using Utilities.Shared;

namespace DataBaseFirst.Services
{
    public class TransportistaService : ITransportistaService
    {
        private readonly TransportistaRepository _transportistaRepository;

        public TransportistaService(TransportistaRepository transportistaRepository)
        {
            _transportistaRepository = transportistaRepository;
        }

        public async Task<ApiResponse<List<Transportistum>>> ListarTransportistasAsync()
        {
            var listaTransportistas = await _transportistaRepository.ListarTransportistasAsync();

            if (listaTransportistas == null || listaTransportistas.Count == 0)
                return new ApiResponse<List<Transportistum>> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY, Data = listaTransportistas };

            return new ApiResponse<List<Transportistum>> { IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = listaTransportistas };
        }

        public async Task<ApiResponse<Paginacion<Transportistum>>> ListarTransportistasPaginacionAsync(int pageNumber, int pageSize)
        {
            var pagedResult = await _transportistaRepository.ListarTransportistasPaginacionAsync(pageNumber, pageSize);

            if (pagedResult.Items == null || !pagedResult.Items.Any())
            {
                return new ApiResponse<Paginacion<Transportistum>> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY, Data = pagedResult };
            }

            return new ApiResponse<Paginacion<Transportistum>> { IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = pagedResult };
        }

        public async Task<ApiResponse<Transportistum>> ObtenerTransportistaAsync(int idTranportista)
        {
            var transportista = await _transportistaRepository.ObtenerTransportistaAsync(idTranportista);

            if (transportista == null)
            {
                return new ApiResponse<Transportistum> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };
            }

            return new ApiResponse<Transportistum> { IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = transportista };
        }

        public async Task<ApiResponse<object>> RegistrarTransportistaAsync(Transportistum transportista)
        {
            if (transportista == null)
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_VALIDATE };

            if (string.IsNullOrWhiteSpace(transportista.Codigo) || string.IsNullOrWhiteSpace(transportista.Nombres) || string.IsNullOrWhiteSpace(transportista.Apellidos) || string.IsNullOrWhiteSpace(transportista.Cedula) || string.IsNullOrWhiteSpace(transportista.Telefono) || string.IsNullOrWhiteSpace(transportista.Correo_Electronico))
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_EMPTY };

            var regex = new Regex("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$");
            if (!regex.IsMatch(transportista.Nombres) || !regex.IsMatch(transportista.Apellidos))
                return new ApiResponse<object> { IsSuccess = false, Message = "Los nombres y apellidos solo puede contener letras y espacios" };

            var regexCedula = new Regex(@"^\d{10}$");
            if (!regexCedula.IsMatch(transportista.Cedula) || !regexCedula.IsMatch(transportista.Telefono))
                return new ApiResponse<object> { IsSuccess = false, Message = "La cédula y el teléfono deben contener exactamente 10 dígitos numéricos" };

            var regexCorreo = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            if (!regexCorreo.IsMatch(transportista.Correo_Electronico))
                return new ApiResponse<object> { IsSuccess = false, Message = "El correo electrónico no tiene un formato válido" };

            var categorias = await _transportistaRepository.ListarTransportistasAsync();
            if (categorias.Any(c => c.Codigo == transportista.Codigo))
                return new ApiResponse<object> { IsSuccess = false, Message = "El código ya existe" };

            if (categorias.Any(c => c.Cedula?.ToLower() == transportista.Cedula.ToLower()))
                return new ApiResponse<object> { IsSuccess = false, Message = "El cédula ya existe" };

            if (categorias.Any(c => c.Telefono?.ToLower() == transportista.Telefono.ToLower()))
                return new ApiResponse<object> { IsSuccess = false, Message = "El télefono ya existe" };

            var result = await _transportistaRepository.RegistrarTransportistaAsync(transportista);
            if (result > 0)
                return new ApiResponse<object> { IsSuccess = true, Message = Mensajes.MESSAGE_REGISTER };

            return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_REGISTER_FAILLED };
        }

        public async Task<ApiResponse<object>> EditarTransportistaAsync(Transportistum transportista)
        {
            if (transportista == null)
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_VALIDATE };

            if (string.IsNullOrWhiteSpace(transportista.Nombres) || string.IsNullOrWhiteSpace(transportista.Apellidos) || string.IsNullOrWhiteSpace(transportista.Cedula) || string.IsNullOrWhiteSpace(transportista.Telefono) || string.IsNullOrWhiteSpace(transportista.Correo_Electronico))
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_EMPTY };

            var clienteExistente = await _transportistaRepository.ObtenerTransportistaAsync(transportista.Id_Transportista);
            if (clienteExistente == null)
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };

            var regex = new Regex("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$");
            if (!regex.IsMatch(transportista.Nombres) || !regex.IsMatch(transportista.Apellidos))
                return new ApiResponse<object> { IsSuccess = false, Message = "Los nombres y apellidos solo puede contener letras y espacios" };

            var regexCedula = new Regex(@"^\d{10}$");
            if (!regexCedula.IsMatch(transportista.Cedula) || !regexCedula.IsMatch(transportista.Telefono))
                return new ApiResponse<object> { IsSuccess = false, Message = "La cédula y el teléfono deben contener exactamente 10 dígitos numéricos" };

            var regexCorreo = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            if (!regexCorreo.IsMatch(transportista.Correo_Electronico))
                return new ApiResponse<object> { IsSuccess = false, Message = "El correo electrónico no tiene un formato válido" };

            var clientes = await _transportistaRepository.ListarTransportistasAsync();
            if (clientes.Any(c => c.Cedula == transportista.Cedula && c.Id_Transportista != transportista.Id_Transportista) || clientes.Any(c => c.Telefono == transportista.Telefono && c.Id_Transportista != transportista.Id_Transportista))
            {
                return new ApiResponse<object> { IsSuccess = false, Message = "El cédula ya existe." };
            }
            else if (clientes.Any(c => c.Telefono == transportista.Telefono && c.Id_Transportista != transportista.Id_Transportista))
            {
                return new ApiResponse<object> { IsSuccess = false, Message = "El teléfono ya existe." };
            }

            var result = await _transportistaRepository.EditarTransportistaAsync(transportista);
            if (result > 0)
                return new ApiResponse<object> { IsSuccess = true, Message = Mensajes.MESSAGE_UPDATE };

            return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_UPDATE_FAILLED };
        }

        public async Task<ApiResponse<int>> EliminarTransportistaAsync(int id)
        {
            try
            {
                var existe = await _transportistaRepository.ObtenerTransportistaAsync(id);
                if (existe == null)
                {
                    return new ApiResponse<int>
                    { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };
                }

                var result = await _transportistaRepository.EliminarTransportistaAsync(id);

                if (result > 0)
                    return new ApiResponse<int> { IsSuccess = true, Message = Mensajes.MESSAGE_DELETE };

                return new ApiResponse<int> { IsSuccess = false, Message = Mensajes.MESSAGE_DELETE_FAILLED };
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                return new ApiResponse<int> { IsSuccess = false, Message = "No se puede eliminar al transportista porque tiene compras asociadas." };
            }
        }

    }
}
