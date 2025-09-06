using DataBaseFirst.Models;
using DataBaseFirst.Models.Dto;
using DataBaseFirst.Repository.InterfacesRepository;
using FluentValidation;
using Infrastructure.Repository;
using Infrastructure.Repository.InterfacesServices;
using Microsoft.Data.SqlClient;
using Utilities.Shared;

namespace Infrastructure.Services
{
    public class ProductoService : IProductoService
    {
        private readonly ProductoRepository _productoRepository;
        private readonly IValidator<Producto> _validator;

        public ProductoService(ProductoRepository productoRepository, IValidator<Producto> validator)
        {
            _productoRepository = productoRepository;
            _validator = validator;
        }

        //Para pruebas unitarias, descomenta este constructor y comenta el constructor anterior.

        /*readonly IProductoRepository _productoRepository;

        public ProductoService(IProductoRepository productoRepository)
        {
            _productoRepository = productoRepository;
        }*/

        public async Task<ApiResponse<List<ProductoCategoria>>> ListarProductosAsync()
        {
            var listapProductos = await _productoRepository.ListarProductosAsync();

            if (listapProductos == null || listapProductos.Count == 0)
                return new ApiResponse<List<ProductoCategoria>> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY, Data = listapProductos };

            return new ApiResponse<List<ProductoCategoria>> { IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = listapProductos };
        }

        public async Task<ApiResponse<Paginacion<ProductoCategoria>>> ListarProductosPaginacionAsync(int pageNumber, int pageSize)
        {
            var pagedResult = await _productoRepository.ListarProductosPaginacionAsync(pageNumber, pageSize);

            if (pagedResult.Items == null || pagedResult.Items.Count == 0)
            {
                return new ApiResponse<Paginacion<ProductoCategoria>> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_EMPTY, Data = pagedResult };
            }

            return new ApiResponse<Paginacion<ProductoCategoria>> { IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = pagedResult };
        }

        public async Task<ApiResponse<ProductoRespuesta>> ObtenerProductoAsync(int idProducto)
        {
            var producto = await _productoRepository.ObtenerProductoAsync(idProducto);

            if (producto == null)
            {
                return new ApiResponse<ProductoRespuesta> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };
            }

            return new ApiResponse<ProductoRespuesta> { IsSuccess = true, Message = Mensajes.MESSAGE_QUERY, Data = producto };
        }

        public async Task<ApiResponse<object>> RegistrarProductoAsync(Producto producto)
        {
            var validationResult = await _validator.ValidateAsync(producto);

            if (!validationResult.IsValid)
                return new ApiResponse<object> { IsSuccess = false, Message = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage)) };

            var productos = await _productoRepository.ListarProductosAsync();
            if (productos.Any(c => c.Codigo == producto.Codigo))
                return new ApiResponse<object> { IsSuccess = false, Message = "El código ya existe" };

            if (productos.Any(c => c.Nombre_Producto?.ToLower() == producto.Nombre_Producto?.ToLower()))
                return new ApiResponse<object> { IsSuccess = false, Message = "El nombre ya existe" };

            var result = await _productoRepository.RegistrarProductoAsync(producto);
            if (result > 0)
                return new ApiResponse<object> { IsSuccess = true, Message = Mensajes.MESSAGE_REGISTER };

            return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_REGISTER_FAILLED };
        }

        public async Task<ApiResponse<object>> EditarProductoAsync(Producto producto)
        {
            if (producto == null)
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_NULL };

            var validationResult = await _validator.ValidateAsync(producto);
            if (!validationResult.IsValid)
                return new ApiResponse<object> { IsSuccess = false, Message = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage)) };

            var productoExistente = await _productoRepository.ObtenerProductoAsync(producto.Id_Producto);
            if (productoExistente == null)
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };

            var productos = await _productoRepository.ListarProductosAsync();
            if (productos.Any(c => c.Nombre_Producto?.ToLower() == producto.Nombre_Producto?.ToLower() && c.Id_Producto != producto.Id_Producto))
            {
                return new ApiResponse<object> { IsSuccess = false, Message = "El nombre ya existe." };
            }

            var result = await _productoRepository.EditarProductoAsync(producto);
            if (result > 0)
                return new ApiResponse<object> { IsSuccess = true, Message = Mensajes.MESSAGE_UPDATE };

            return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_UPDATE_FAILLED };
        }

        public async Task<ApiResponse<int>> EliminarProductoAsync(int id)
        {
            try
            {
                var existe = await _productoRepository.ObtenerProductoAsync(id);
                if (existe == null)
                {
                    return new ApiResponse<int> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };
                }

                var result = await _productoRepository.EliminarProductoAsync(id);

                if (result > 0)
                    return new ApiResponse<int> { IsSuccess = true, Message = Mensajes.MESSAGE_DELETE };

                return new ApiResponse<int> { IsSuccess = false, Message = Mensajes.MESSAGE_DELETE_FAILLED };

            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                return new ApiResponse<int> { IsSuccess = false, Message = "No se puede eliminar el producto porque tiene compras y ventas asociadas." };
            }
        }
    }
}
