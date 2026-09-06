using Domain.Models;
using Domain.Models.Dto.Response.Negocio;
using FluentValidation;
using Infrastructure.Repository;
using Infrastructure.Repository.InterfacesBusiness;
using Infrastructure.Repository.InterfacesRepository;
using Infrastructure.Repository.InterfacesServices;
using Infrastructure.Services.business;
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

        private readonly IAuditoriaService _auditoriaService;
        private readonly ICurrentUser _currentUserService;
        private readonly IValidator<Negocio> _validator;
        private readonly OllamaClient _ollama;

        public NegocioService(NegocioRepository negocioRepository, ProductoRepository productoRepository, CategoriaRepository categoriaRepository, ICurrentUser currentUserService, IValidator<Negocio> validator, OllamaClient ollama, IAuditoriaService auditoriaService)
        {
            _negocioRepository = negocioRepository;
            _productoRepository = productoRepository;
            _auditoriaService = auditoriaService;
            _categoriaRepository = categoriaRepository;
            _currentUserService = currentUserService;
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
            try
            {
                var negocio = await _negocioRepository.ObtenerNegocioAsync(idNegocio);

                if (negocio == null)
                {
                    await _auditoriaService.RegistrarFalloAsync("Obtener Negocio", $"Negocio con ID {idNegocio} no encontrado");

                    return new ApiResponse<Negocio>{ IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };
                }

                await _auditoriaService.RegistrarExitoAsync("Obtener Negocio", $"Negocio ID {idNegocio} obtenido correctamente");

                return new ApiResponse<Negocio>{ IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = negocio };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Obtener Negocio", ex);
                throw;
            }
        }

        public async Task<ApiResponse<object>> EditarNegocioAsync(Negocio negocio)
        {
            try
            {
                if (negocio == null)
                {
                    await _auditoriaService.RegistrarFalloAsync("Editar Negocio", "Negocio nulo");

                    return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_NULL };
                }

                var validationResult = await _validator.ValidateAsync(negocio);
                if (!validationResult.IsValid)
                {
                    var errores = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage));
                    await _auditoriaService.RegistrarFalloAsync("Editar Negocio", $"Validación fallida: {errores}");

                    return new ApiResponse<object>{ IsSuccess = false, Message = errores };
                }

                var negocioExistente = await _negocioRepository.ObtenerNegocioAsync(negocio.Id_Negocio);
                if (negocioExistente == null)
                {
                    await _auditoriaService.RegistrarFalloAsync("Editar Negocio", $"Negocio con ID {negocio.Id_Negocio} no encontrado");

                    return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };
                }

                var idUsuario = _currentUserService.GetUserId();
                var result = await _negocioRepository.EditarNegocioAsync(negocio, idUsuario);

                if (result > 0)
                {
                    await _auditoriaService.RegistrarExitoAsync("Editar Negocio", $"Negocio ID {negocio.Id_Negocio} actualizado exitosamente");

                    return new ApiResponse<object>{ IsSuccess = true, Message = Mensajes.MESSAGE_UPDATE };
                }

                await _auditoriaService.RegistrarFalloAsync("Editar Negocio", $"No se pudo actualizar el negocio ID {negocio.Id_Negocio}");

                return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_UPDATE_FAILLED };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Editar Negocio", ex);
                throw;
            }
        }

        public async Task<ApiResponse<List<ProductoMasCompradoResponse>>> ObtenerProductoMasComprado()
        {
            try
            {
                var lista = await _negocioRepository.ObtenerProductoMasComprado();

                if (lista == null || lista.Count == 0)
                {
                    await _auditoriaService.RegistrarFalloAsync("Obtener Producto Más Comprado", "No hay datos de productos comprados");

                    return new ApiResponse<List<ProductoMasCompradoResponse>>{ IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY };
                }

                await _auditoriaService.RegistrarExitoAsync("Obtener Producto Más Comprado", $"Se obtuvieron {lista.Count} registros de productos más comprados");

                return new ApiResponse<List<ProductoMasCompradoResponse>>{ IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = lista };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Obtener Producto Más Comprado", ex);
                throw;
            }
        }

        public async Task<ApiResponse<List<ProductoMasVendidoResponse>>> ObtenerProductoMasVendido()
        {
            try
            {
                var lista = await _negocioRepository.ObtenerProductoMasVendido();

                if (lista == null || lista.Count == 0)
                {
                    await _auditoriaService.RegistrarFalloAsync("Obtener Producto Más Vendido", "No hay datos de productos vendidos");

                    return new ApiResponse<List<ProductoMasVendidoResponse>>{ IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY };
                }

                await _auditoriaService.RegistrarExitoAsync("Obtener Producto Más Vendido", $"Se obtuvieron {lista.Count} registros de productos más vendidos");

                return new ApiResponse<List<ProductoMasVendidoResponse>>{ IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = lista };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Obtener Producto Más Vendido", ex);
                throw;
            }
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

            string promptFinal = $@"Analiza estos datos y responde en máximo 3 líneas: {datos} Pregunta: {promptUsuario}";
            var respuesta = await _ollama.GenerateAsync(promptFinal);

            return new ApiResponse<object>
            {
                IsSuccess = true,
                Message = Mensajes.MESSAGE_IA,
                Data = respuesta
            };
        }

        public async Task<ApiResponse<List<TopClienteResponse>>> ObtenerTopClientes()
        {
            try
            {
                var lista = await _negocioRepository.ObtenerTopClientes();

                if (lista == null || lista.Count == 0)
                {
                    await _auditoriaService.RegistrarFalloAsync("Obtener Top Clientes", "No hay datos de clientes");

                    return new ApiResponse<List<TopClienteResponse>>{ IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY };
                }

                await _auditoriaService.RegistrarExitoAsync("Obtener Top Clientes", $"Se obtuvieron {lista.Count} clientes principales");

                return new ApiResponse<List<TopClienteResponse>>{ IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = lista };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Obtener Top Clientes", ex);
                throw;
            }
        }

        public async Task<ApiResponse<List<TopProveedorResponse>>> ObtenerTopProveedores()
        {
            try
            {
                var lista = await _negocioRepository.ObtenerTopProveedores();

                if (lista == null || lista.Count == 0)
                {
                    await _auditoriaService.RegistrarFalloAsync("Obtener Top Proveedores", "No hay datos de proveedores");

                    return new ApiResponse<List<TopProveedorResponse>>{ IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY };
                }

                await _auditoriaService.RegistrarExitoAsync("Obtener Top Proveedores", $"Se obtuvieron {lista.Count} proveedores principales");

                return new ApiResponse<List<TopProveedorResponse>>{ IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = lista};
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Obtener Top Proveedores", ex);
                throw;
            }
        }

        public async Task<ApiResponse<List<ViajesTransportistaResponse>>> ObtenerViajesTransportista()
        {
            try
            {
                var lista = await _negocioRepository.ObtenerViajesTransportista();

                if (lista == null || lista.Count == 0)
                {
                    await _auditoriaService.RegistrarFalloAsync("Obtener Viajes Transportista", "No hay datos de viajes");

                    return new ApiResponse<List<ViajesTransportistaResponse>>{ IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY };
                }

                await _auditoriaService.RegistrarExitoAsync("Obtener Viajes Transportista", $"Se obtuvieron {lista.Count} registros de viajes");

                return new ApiResponse<List<ViajesTransportistaResponse>>{ IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = lista };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Obtener Viajes Transportista", ex);
                throw;
            }
        }

        public async Task<ApiResponse<List<EmpleadoProductivoResponse>>> ObtenerEmpleadosProductivos()
        {
            try
            {
                var lista = await _negocioRepository.ObtenerEmpleadosProductivos();

                if (lista == null || lista.Count == 0)
                {
                    await _auditoriaService.RegistrarFalloAsync("Obtener Empleados Productivos","No hay datos de empleados productivos");

                    return new ApiResponse<List<EmpleadoProductivoResponse>>{ IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY };
                }

                await _auditoriaService.RegistrarExitoAsync("Obtener Empleados Productivos", $"Se obtuvieron {lista.Count} empleados productivos");

                return new ApiResponse<List<EmpleadoProductivoResponse>>{ IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = lista };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Obtener Empleados Productivos", ex);
                throw;
            }
        }
    }
}
