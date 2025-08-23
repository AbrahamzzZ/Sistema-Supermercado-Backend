using DataBaseFirst.Models;
using DataBaseFirst.Models.Dto;
using DataBaseFirst.Repository;
using DataBaseFirst.Repository.InterfacesRepository;
using DataBaseFirst.Repository.InterfacesServices;
using Microsoft.Data.SqlClient;
using System.Text.RegularExpressions;
using Utilities.Shared;

namespace DataBaseFirst.Services
{
    public class ProductoService : IProductoService
    {
        private readonly ProductoRepository _productoRepository;

        public ProductoService(ProductoRepository productoRepository)
        {
            _productoRepository = productoRepository;
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
            if (producto == null)
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_VALIDATE };

            if (producto.Id_Categoria <= 0 || string.IsNullOrWhiteSpace(producto.Nombre_Producto) || string.IsNullOrWhiteSpace(producto.Descripcion) || string.IsNullOrWhiteSpace(producto.Pais_Origen))
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_EMPTY };

            var regex = new Regex("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$");
            if (!regex.IsMatch(producto.Nombre_Producto))
                return new ApiResponse<object> { IsSuccess = false, Message = "El nombre solo puede contener letras y espacios." };

            if (!regex.IsMatch(producto.Pais_Origen))
                return new ApiResponse<object> { IsSuccess = false, Message = "El país de origen solo puede contener letras y espacios." };

            var productos = await _productoRepository.ListarProductosAsync();
            if (productos.Any(c => c.Codigo == producto.Codigo))
                return new ApiResponse<object> { IsSuccess = false, Message = "El código ya existe" };

            if (productos.Any(c => c.Nombre_Producto?.ToLower() == producto.Nombre_Producto.ToLower()))
                return new ApiResponse<object> { IsSuccess = false, Message = "El nombre ya existe" };

            var result = await _productoRepository.RegistrarProductoAsync(producto);
            if (result > 0)
                return new ApiResponse<object> { IsSuccess = true, Message = Mensajes.MESSAGE_REGISTER };

            return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_REGISTER_FAILLED };
        }

        public async Task<ApiResponse<object>> EditarProductoAsync(Producto producto)
        {
            if (producto == null)
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_VALIDATE };

            if (producto.Id_Categoria <= 0 || string.IsNullOrWhiteSpace(producto.Nombre_Producto) || string.IsNullOrWhiteSpace(producto.Descripcion) || string.IsNullOrWhiteSpace(producto.Pais_Origen))
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_EMPTY };

            var categoriaExistente = await _productoRepository.ObtenerProductoAsync(producto.Id_Producto);
            if (categoriaExistente == null)
                return new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND };

            var regex = new Regex("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$");
            if (!regex.IsMatch(producto.Nombre_Producto))
                return new ApiResponse<object> { IsSuccess = false, Message = "El nombre solo puede contener letras y espacios." };

            if (!regex.IsMatch(producto.Pais_Origen))
                return new ApiResponse<object> { IsSuccess = false, Message = "El país de origen solo puede contener letras y espacios." };

            var productos = await _productoRepository.ListarProductosAsync();
            if (productos.Any(c => c.Nombre_Producto?.ToLower() == producto.Nombre_Producto.ToLower() && c.Id_Producto != producto.Id_Producto))
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
