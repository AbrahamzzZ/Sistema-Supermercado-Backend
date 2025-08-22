using DataBaseFirst.Models;
using DataBaseFirst.Models.Dto;
using DataBaseFirst.Repository;
using DataBaseFirst.Repository.InterfacesServices;
using System.Text.RegularExpressions;
using Utilities.Shared;
using WebApiRest.Dto;

namespace DataBaseFirst.Services
{
    public class NegocioService : INegocioService
    {
        private readonly NegocioRepository _negocioRepository;

        public NegocioService(NegocioRepository negocioRepository)
        {
            _negocioRepository = negocioRepository;
        }

        public async Task<ApiResponse<Negocio>> ObtenerNegocioAsync(int idNegocio)
        {
            var negocio = await _negocioRepository.ObtenerNegocioAsync(idNegocio);
            if(negocio == null)
                return new ApiResponse<Negocio> {IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND};

            return new ApiResponse<Negocio> { IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = negocio };
        }

        public async Task<ApiResponse<object>> EditarNegocioAsync(Negocio negocio)
        {
            if (negocio == null)
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_VALIDATE };

            if (string.IsNullOrWhiteSpace(negocio.Nombre) || string.IsNullOrWhiteSpace(negocio.Telefono) || string.IsNullOrWhiteSpace(negocio.Ruc) || string.IsNullOrWhiteSpace(negocio.Direccion) || string.IsNullOrWhiteSpace(negocio.Correo_Electronico))
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_EMPTY };

            var negocioExistente = await _negocioRepository.ObtenerNegocioAsync(negocio.Id_Negocio);
            if (negocioExistente == null)
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };

            var regexCedula = new Regex(@"^\d{10}$");
            if (!regexCedula.IsMatch(negocio.Telefono))
                return new ApiResponse<object> { IsSuccess = false, Message = "El teléfono deben contener exactamente 10 dígitos numéricos" };

            var regexRuc = new Regex(@"^\d{13}$");
            if (!regexRuc.IsMatch(negocio.Ruc))
                return new ApiResponse<object> { IsSuccess = false, Message = "El RUC deben contener exactamente 13 dígitos numéricos" };

            var regexCorreo = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            if (!regexCorreo.IsMatch(negocio.Correo_Electronico))
                return new ApiResponse<object> { IsSuccess = false, Message = "El correo electrónico no tiene un formato válido" };

            var result = await _negocioRepository.EditarNegocioAsync(negocio);
            if (result > 0)
                return new ApiResponse<object> { IsSuccess = true, Message = Mensajes.MESSAGE_UPDATE };

            return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_UPDATE_FAILLED };
        }

        public async Task<ApiResponse<List<ProductoMasComprado>>> ObtenerProductoMasComprado()
        {
            var lista = await _negocioRepository.ObtenerProductoMasComprado();

            if (lista == null)
                return new ApiResponse<List<ProductoMasComprado>> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY};

            return new ApiResponse<List<ProductoMasComprado>> { IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = lista };
        }

        public async Task<ApiResponse<List<ProductoMasVendido>>> ObtenerProductoMasVendido()
        {
            var lista = await _negocioRepository.ObtenerProductoMasVendido();

            if (lista == null || lista.Count == 0)
                return new ApiResponse<List<ProductoMasVendido>> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY };

            return new ApiResponse<List<ProductoMasVendido>> { IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = lista };
        }

        public async Task<ApiResponse<List<TopCliente>>> ObtenerTopClientes()
        {
            var lista = await _negocioRepository.ObtenerTopClientes();

            if (lista == null || lista.Count == 0)
                return new ApiResponse<List<TopCliente>> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY };

            return new ApiResponse<List<TopCliente>> { IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = lista };
        }

        public async Task<ApiResponse<List<TopProveedor>>> ObtenerTopProveedores()
        {
            var lista = await _negocioRepository.ObtenerTopProveedores();

            if (lista == null || lista.Count == 0)
                return new ApiResponse<List<TopProveedor>> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY };

            return new ApiResponse<List<TopProveedor>> { IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = lista };
        }

        public async Task<ApiResponse<List<ViajesTransportista>>> ObtenerViajesTransportista()
        {
            var lista = await _negocioRepository.ObtenerViajesTransportista();

            if (lista == null || lista.Count == 0)
                return new ApiResponse<List<ViajesTransportista>> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY };

            return new ApiResponse<List<ViajesTransportista>> { IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = lista };
        }

        public async Task<ApiResponse<List<EmpleadoProductivo>>> ObtenerEmpleadosProductivos()
        {
            var lista = await _negocioRepository.ObtenerEmpleadosProductivos();

            if (lista == null || lista.Count == 0)
                return new ApiResponse<List<EmpleadoProductivo>> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY };

            return new ApiResponse<List<EmpleadoProductivo>> { IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = lista };
        }
    }
}
