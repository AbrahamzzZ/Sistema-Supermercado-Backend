using Domain.Models;
using Domain.Models.Dto.Response.Producto;
using FluentValidation;
using Infrastructure.Repository;
using Infrastructure.Repository.InterfacesBusiness;
using Infrastructure.Repository.InterfacesRepository;
using Infrastructure.Repository.InterfacesServices;
using Infrastructure.Services.business;
using Microsoft.Data.SqlClient;
using Utilities.Shared;

namespace Infrastructure.Services
{
    public class ProductoService : IProductoService
    {
        private readonly ProductoRepository _productoRepository;
        private readonly IValidator<Producto> _validator;
        private readonly ICurrentUser _currentUserService;
        private readonly IAuditoriaService _auditoriaService;

        public ProductoService(ProductoRepository productoRepository, IValidator<Producto> validator, ICurrentUser currentUserService, IAuditoriaService auditoriaService)
        {
            _productoRepository = productoRepository;
            _validator = validator;
            _currentUserService = currentUserService;
            _auditoriaService = auditoriaService;

        }

        //Para pruebas unitarias, descomenta este constructor y comenta el constructor anterior.

        /*readonly IProductoRepository _productoRepository;
        private readonly IValidator<Producto> _validator;

        public ProductoService(IProductoRepository productoRepository, IValidator<Producto> validator)
        {
            _productoRepository = productoRepository;
            _validator = validator;
        }*/

        public async Task<ApiResponse<List<ProductoCategoriaResponse>>> ListarProductosAsync()
        {
            try
            {
                var listapProductos = await _productoRepository.ListarProductosAsync();

                if (listapProductos == null || listapProductos.Count == 0)
                {
                    await _auditoriaService.RegistrarFalloAsync("Listar Productos", "No hay productos registrados");

                    return new ApiResponse<List<ProductoCategoriaResponse>>{ IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY, Data = listapProductos };
                }

                await _auditoriaService.RegistrarExitoAsync("Listar Productos", $"Se obtuvieron {listapProductos.Count} productos");

                return new ApiResponse<List<ProductoCategoriaResponse>>{ IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = listapProductos };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Listar Productos", ex);
                throw;
            }
        }

        public async Task<ApiResponse<Paginacion<ProductoCategoriaResponse>>> ListarProductosPaginacionAsync(int pageNumber, int pageSize, string filtro = "")
        {
            try
            {
                var pagedResult = await _productoRepository.ListarProductosPaginacionAsync(pageNumber, pageSize, filtro);

                if (pagedResult.Items == null || pagedResult.Items.Count == 0)
                {
                    await _auditoriaService.RegistrarFalloAsync("Listar Productos Paginación", $"No hay resultados para el filtro: {filtro}");

                    return new ApiResponse<Paginacion<ProductoCategoriaResponse>>{ IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY, Data = pagedResult };
                }

                await _auditoriaService.RegistrarExitoAsync("Listar Productos Paginación", $"Página {pageNumber}, {pagedResult.Items.Count} productos. Filtro: {filtro}");

                return new ApiResponse<Paginacion<ProductoCategoriaResponse>>{ IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = pagedResult };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Listar Productos Paginación", ex);
                throw;
            }
        }

        public async Task<ApiResponse<ProductoResponse>> ObtenerProductoAsync(int idProducto)
        {
            try
            {
                var producto = await _productoRepository.ObtenerProductoAsync(idProducto);

                if (producto == null)
                {
                    await _auditoriaService.RegistrarFalloAsync("Obtener Producto", $"Producto con ID {idProducto} no encontrado");

                    return new ApiResponse<ProductoResponse>{ IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };
                }

                await _auditoriaService.RegistrarExitoAsync("Obtener Producto", $"Producto {producto.Codigo} obtenido correctamente");

                return new ApiResponse<ProductoResponse>{ IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = producto };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Obtener Producto", ex);
                throw;
            }
        }

        public async Task<ApiResponse<object>> RegistrarProductoAsync(Producto producto)
        {
            try
            {
                if (producto == null)
                {
                    await _auditoriaService.RegistrarFalloAsync("Registrar Producto", "Producto nulo");

                    return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_NULL };
                }

                var validationResult = await _validator.ValidateAsync(producto);

                if (!validationResult.IsValid)
                {
                    var errores = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage));
                    await _auditoriaService.RegistrarFalloAsync( "Registrar Producto", $"Validación fallida: {errores}");

                    return new ApiResponse<object>{ IsSuccess = false, Message = errores };
                }

                var productos = await _productoRepository.ListarProductosAsync();

                if (productos.Any(c => c.Codigo == producto.Codigo))
                {
                    await _auditoriaService.RegistrarFalloAsync("Registrar Producto", $"Código {producto.Codigo} ya existe");

                    return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_CODE_EXITS };
                }

                if (productos.Any(c => c.Nombre_Producto?.ToLower() == producto.Nombre_Producto?.ToLower()))
                {
                    await _auditoriaService.RegistrarFalloAsync("Registrar Producto", $"Nombre {producto.Nombre_Producto} ya existe");

                    return new ApiResponse<object>{ IsSuccess = false, Message = "El nombre ya existe" };
                }

                var idUsuario = _currentUserService.GetUserId();
                var result = await _productoRepository.RegistrarProductoAsync(producto, idUsuario);

                if (result > 0)
                {
                    await _auditoriaService.RegistrarExitoAsync("Registrar Producto", $"Producto {producto.Codigo} registrado exitosamente");

                    return new ApiResponse<object>{ IsSuccess = true, Message = Mensajes.MESSAGE_REGISTER };
                }

                await _auditoriaService.RegistrarFalloAsync("Registrar Producto", $"No se pudo guardar el producto {producto.Codigo}");

                return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_REGISTER_FAILLED };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Registrar Producto", ex);
                throw;
            }
        }

        public async Task<ApiResponse<object>> EditarProductoAsync(Producto producto)
        {
            try
            {
                if (producto == null)
                {
                    await _auditoriaService.RegistrarFalloAsync("Editar Producto", "Producto nulo");

                    return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_NULL };
                }

                var validationResult = await _validator.ValidateAsync(producto);
                if (!validationResult.IsValid)
                {
                    var errores = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage));
                    await _auditoriaService.RegistrarFalloAsync("Editar Producto", $"Validación fallida: {errores}");

                    return new ApiResponse<object>{ IsSuccess = false, Message = errores };
                }

                var productoExistente = await _productoRepository.ObtenerProductoAsync(producto.Id_Producto);
                if (productoExistente == null)
                {
                    await _auditoriaService.RegistrarFalloAsync( "Editar Producto", $"Producto con ID {producto.Id_Producto} no encontrado");

                    return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };
                }

                var productos = await _productoRepository.ListarProductosAsync();
                if (productos.Any(c => c.Nombre_Producto?.ToLower() == producto.Nombre_Producto?.ToLower() && c.Id_Producto != producto.Id_Producto))
                {
                    await _auditoriaService.RegistrarFalloAsync("Editar Producto", $"Nombre {producto.Nombre_Producto} ya existe en otro producto");

                    return new ApiResponse<object>{ IsSuccess = false, Message = "El nombre ya existe." };
                }

                var idUsuario = _currentUserService.GetUserId();
                var result = await _productoRepository.EditarProductoAsync(producto, idUsuario);

                if (result > 0)
                {
                    await _auditoriaService.RegistrarExitoAsync("Editar Producto", $"Producto {producto.Codigo} actualizado exitosamente");

                    return new ApiResponse<object>{ IsSuccess = true, Message = Mensajes.MESSAGE_UPDATE };
                }

                await _auditoriaService.RegistrarFalloAsync("Editar Producto", $"No se pudo actualizar el producto {producto.Codigo}");

                return new ApiResponse<object>{ IsSuccess = false, Message = Mensajes.MESSAGE_UPDATE_FAILLED };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Editar Producto", ex);
                throw;
            }
        }

        public async Task<ApiResponse<int>> EliminarProductoAsync(int id)
        {
            try
            {
                var existe = await _productoRepository.ObtenerProductoAsync(id);
                if (existe == null)
                {
                    await _auditoriaService.RegistrarFalloAsync("Eliminar Producto", $"Producto con ID {id} no encontrado");

                    return new ApiResponse<int>{ IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };
                }

                var result = await _productoRepository.EliminarProductoAsync(id);

                if (result > 0)
                {
                    await _auditoriaService.RegistrarExitoAsync("Eliminar Producto", $"Producto {existe.Codigo} eliminado exitosamente");

                    return new ApiResponse<int>{ IsSuccess = true, Message = Mensajes.MESSAGE_DELETE };
                }

                await _auditoriaService.RegistrarFalloAsync("Eliminar Producto", $"No se pudo eliminar el producto {existe.Codigo}");

                return new ApiResponse<int>
                { IsSuccess = false, Message = Mensajes.MESSAGE_DELETE_FAILLED };
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                await _auditoriaService.RegistrarErrorAsync("Eliminar Producto", new Exception("No se puede eliminar: producto tiene compras y ventas asociadas"));

                return new ApiResponse<int>{ IsSuccess = false, Message = "No se puede eliminar el producto porque tiene compras y ventas asociadas." };
            }
            catch (Exception ex)
            {
                await _auditoriaService.RegistrarErrorAsync("Eliminar Producto", ex);
                throw;
            }
        }
    }
}
