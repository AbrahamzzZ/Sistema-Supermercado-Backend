using Domain.Models;
using Domain.Models.Dto.Negocio;
using Infrastructure.Repository.InterfacesRepository;
using FluentValidation;
using Infrastructure.Repository;
using Infrastructure.Repository.InterfacesServices;
using Utilities.IA;
using Utilities.Shared;

namespace Infrastructure.Services
{
    public class NegocioService : INegocioService
    {
        private readonly NegocioRepository _negocioRepository;
        private readonly IValidator<Negocio> _validator;
        private readonly OllamaClient _ollama;

        public NegocioService(NegocioRepository negocioRepository, IValidator<Negocio> validator, OllamaClient ollama)
        {
            _negocioRepository = negocioRepository;
            _validator = validator;
            _ollama = ollama;
        }

        //Para pruebas unitarias, descomenta este constructor y comenta el constructor anterior.

        /*readonly INegocioRepository _negocioRepository;
        private readonly IValidator<Negocio> _validator;

        public NegocioService(INegocioRepository negocioRepository, IValidator<Negocio> validator)
        {
            _negocioRepository = negocioRepository;
            _validator = validator;
        }*/

        public async Task<ApiResponse<Negocio>> ObtenerNegocioAsync(int idNegocio)
        {
            var negocio = await _negocioRepository.ObtenerNegocioAsync(idNegocio);
            if (negocio == null)
                return new ApiResponse<Negocio> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };

            return new ApiResponse<Negocio> { IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = negocio };
        }

        public async Task<ApiResponse<object>> EditarNegocioAsync(Negocio negocio)
        {
            if (negocio == null)
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_NULL };

            var validationResult = await _validator.ValidateAsync(negocio);
            if (!validationResult.IsValid)
                return new ApiResponse<object> { IsSuccess = false, Message = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage)) };

            var negocioExistente = await _negocioRepository.ObtenerNegocioAsync(negocio.Id_Negocio);
            if (negocioExistente == null)
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };

            var result = await _negocioRepository.EditarNegocioAsync(negocio);
            if (result > 0)
                return new ApiResponse<object> { IsSuccess = true, Message = Mensajes.MESSAGE_UPDATE };

            return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_UPDATE_FAILLED };
        }

        public async Task<ApiResponse<List<ProductoMasComprado>>> ObtenerProductoMasComprado()
        {
            var lista = await _negocioRepository.ObtenerProductoMasComprado();

            if (lista == null)
                return new ApiResponse<List<ProductoMasComprado>> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY };

            return new ApiResponse<List<ProductoMasComprado>> { IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = lista };
        }

        public async Task<ApiResponse<List<ProductoMasVendido>>> ObtenerProductoMasVendido()
        {
            var lista = await _negocioRepository.ObtenerProductoMasVendido();

            if (lista == null || lista.Count == 0)
                return new ApiResponse<List<ProductoMasVendido>> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY };

            return new ApiResponse<List<ProductoMasVendido>> { IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = lista };
        }


        public async Task<ApiResponse<object>> ObtenerProductoMasVendidoIA()
        {
            var lista = await _negocioRepository.ObtenerProductoMasVendido();

            if (lista == null || lista.Count == 0)
                return new ApiResponse<object> { IsSuccess = false, Message = "No hay datos." };

            // Convertimos los datos a texto para enviarlos a la IA
            string texto = string.Join("\n", lista.Select(x => $"{x.Nombre_Producto}: {x.Cantidad_Vendida} unidades"));

            string prompt = $@"Analiza estos productos y sus cantidades vendidas: {texto}
            Formato requerido:
            - Producto más vendido.
            - Comparación rápida entre productos.
            - Si existe o no una tendencia clara.

            No escribas más de 5 líneas.
            Sé directo e informativo.
            ";
            string analisisIA = await _ollama.GenerateAsync(prompt);

            return new ApiResponse<object>
            {
                IsSuccess = true,
                Message = "Análisis generado",
                Data = new
                {
                    productos = lista,
                    analisis = analisisIA
                }
            };
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
