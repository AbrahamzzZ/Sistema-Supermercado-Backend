using Domain.Models;
using Domain.Models.Dto.Negocio;
using FluentValidation;
using Infrastructure.Repository;
using Infrastructure.Repository.InterfacesRepository;
using Infrastructure.Repository.InterfacesServices;
using Utilities.IA;
using Utilities.Shared;
using static Utilities.IA.Reglas;

namespace Infrastructure.Services
{
    public class NegocioService : INegocioService
    {
        private readonly NegocioRepository _negocioRepository;
        private readonly ProductoRepository _productoRepository;
        private readonly CategoriaRepository _categoriaRepository;
        private readonly IValidator<Negocio> _validator;
        private readonly OllamaClient _ollama;

        public NegocioService(NegocioRepository negocioRepository, ProductoRepository productoRepository, CategoriaRepository categoriaRepository, IValidator<Negocio> validator, OllamaClient ollama)
        {
            _negocioRepository = negocioRepository;
            _productoRepository = productoRepository;
            _categoriaRepository = categoriaRepository;
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

            if (lista == null || lista.Count == 0)
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

        public async Task<ApiResponse<object>> AnalisisIA(string promptUsuario)
        {
            var tipo = Reglas.DetectarTipoAnalisis(promptUsuario);

            if (tipo == TipoAnalisis.Invalido)
            {
                return new ApiResponse<object>
                {
                    IsSuccess = false,
                    Message = Mensajes.MESSAGE_IA_FAILLED
                };
            }

            string datos;

            switch (tipo)
            {
                case TipoAnalisis.Producto:
                    var productos = await _productoRepository.ListarProductosAsync();
                    datos = string.Join("\n", productos.Select(x => $"{x.Nombre_Producto}: {x.Stock} unidades"));
                    break;

                case TipoAnalisis.Categoria:
                    var categorias = await _categoriaRepository.ListarCategoriasAsync();
                    datos = string.Join("\n", categorias.Select(x => $"{x.Nombre_Categoria}"));
                    break;

                case TipoAnalisis.Cliente:
                    var clientes = await _negocioRepository.ObtenerTopClientes();
                    datos = string.Join("\n", clientes.Select(x => $"{x.Nombre_Completo}: {x.Compras_Totales} compras"));
                    break;

                case TipoAnalisis.Comprado:
                    var comprados = await _negocioRepository.ObtenerAnalisisProductosComprados();
                    datos = string.Join("\n", comprados.Select(x => $"{x.Nombre_Producto}: {x.Cantidad_Comprada} unidades"));
                    break;

                case TipoAnalisis.Vendido:
                    var vendidos = await _negocioRepository.ObtenerAnalisisProductosVendidos();
                    datos = string.Join("\n", vendidos.Select(x => $"{x.Nombre_Producto}: {x.Cantidad_Vendida} unidades"));
                    break;

                default:
                    return new ApiResponse<object>
                    {
                        IsSuccess = false,
                        Message = Mensajes.MESSAGE_IA_FAILLED
                    };
            }

            string promptFinal = $@" Analiza los siguientes datos del sistema de ventas: {datos} Solicitud del usuario: {promptUsuario}. Una respuesta clara y máximo de 5 líneas.";

            var respuesta = await _ollama.GenerateAsync(promptFinal);

            return new ApiResponse<object>
            {
                IsSuccess = true,
                Message = Mensajes.MESSAGE_IA,
                Data = respuesta
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
