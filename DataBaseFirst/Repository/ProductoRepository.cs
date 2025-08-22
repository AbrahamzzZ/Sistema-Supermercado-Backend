using DataBaseFirst.Contexts;
using DataBaseFirst.Models;
using DataBaseFirst.Models.Dto;
using DataBaseFirst.Repository.InterfacesRepository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Utilities.Shared;

namespace DataBaseFirst.Repository
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly SistemaSupermercadoContext _context;

        public ProductoRepository(SistemaSupermercadoContext context)
        {
            _context = context;
        }

        public async Task<List<ProductoCategoria>> ListarProductosAsync()
        {
            return await _context.ProductosDto
                .FromSqlRaw("EXEC PA_LISTA_PRODUCTO")
                .ToListAsync();
        }

        public async Task<Paginacion<ProductoCategoria>> ListarProductosPaginacionAsync(int pageNumber, int pageSize)
        {

            using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "PA_LISTA_PRODUCTO_PAGINACION";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@PageNumber", pageNumber));
            command.Parameters.Add(new SqlParameter("@PageSize", pageSize));

            await _context.Database.OpenConnectionAsync();

            var productos = new List<ProductoCategoria>();
            int totalCount = 0;

            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    productos.Add(new ProductoCategoria
                    {
                        Id_Producto = reader.GetInt32(0),
                        Codigo = reader.GetString(1),
                        Descripcion = reader.GetString(2),
                        Nombre_Producto = reader.GetString(3),
                        Id_Categoria = reader.GetInt32(4),
                        Nombre_Categoria = reader.GetString(5),
                        Pais_Origen = reader.GetString(6),
                        Stock = reader.GetInt32(7),
                        Precio_Compra = reader.GetDecimal(8),
                        Precio_Venta = reader.GetDecimal(9),
                        Estado = reader.GetBoolean(10)
                    });
                }

                if (await reader.NextResultAsync() && await reader.ReadAsync())
                {
                    totalCount = reader.GetInt32(0);
                }
            }

            return new Paginacion<ProductoCategoria> { Items = productos, TotalCount = totalCount, PageNumber = pageNumber, PageSize = pageSize };
        }

        public async Task<ProductoRespuesta?> ObtenerProductoAsync(int idProducto)
        {
            var idParam = new SqlParameter("@Id_Producto", idProducto);
            return await Task.Run(() =>
                _context.ProductoDto
                .FromSqlRaw("EXEC PA_OBTENER_PRODUCTO @Id_Producto", idParam)
                .AsNoTracking()
                .AsEnumerable()
                .FirstOrDefault()
            );
        }

        public async Task<int> RegistrarProductoAsync(Producto producto)
        {

            return await _context.Database.ExecuteSqlRawAsync(
                "EXEC PA_REGISTRAR_PRODUCTO @Codigo, @Descripcion, @Nombre_Producto, @Id_Categoria, @Pais_Origen, @Estado",
                new SqlParameter("@Codigo", producto.Codigo ?? (object)DBNull.Value),
                new SqlParameter("@Descripcion", producto.Descripcion ?? (object)DBNull.Value),
                new SqlParameter("@Nombre_Producto", producto.Nombre_Producto ?? (object)DBNull.Value),
                new SqlParameter("@Id_Categoria", producto.Id_Categoria ?? (object)DBNull.Value),
                new SqlParameter("@Pais_Origen", producto.Pais_Origen ?? (object)DBNull.Value),
                new SqlParameter("@Estado", producto.Estado ?? (object)DBNull.Value)
            );
        }

        public async Task<int> EditarProductoAsync(Producto producto)
        {

            return await _context.Database.ExecuteSqlRawAsync(
                "EXEC PA_EDITAR_PRODUCTO @Id_Producto, @Descripcion, @Nombre_Producto, @Id_Categoria, @Pais_Origen, @Estado",
                new SqlParameter("@Id_Producto", producto.Id_Producto),
                new SqlParameter("@Descripcion", producto.Descripcion ?? (object)DBNull.Value),
                new SqlParameter("@Nombre_Producto", producto.Nombre_Producto ?? (object)DBNull.Value),
                new SqlParameter("@Id_Categoria", producto.Id_Categoria ?? (object)DBNull.Value),
                new SqlParameter("@Pais_Origen", producto.Pais_Origen ?? (object)DBNull.Value),
                new SqlParameter("@Estado", producto.Estado ?? (object)DBNull.Value)
            );
        }

        public async Task<int> EliminarProductoAsync(int idProducto)
        {
            return await _context.Database.ExecuteSqlRawAsync(
                "EXEC PA_ELIMINAR_PRODUCTO @Id_Producto",
                new SqlParameter("@Id_Producto", idProducto)
            );
        }
    }
}
