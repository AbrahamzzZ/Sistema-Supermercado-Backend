using Domain.Contexts;
using Domain.Models;
using Domain.Models.Dto;
using Infrastructure.Repository.InterfacesRepository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Utilities.Shared;

namespace Infrastructure.Repository
{
    public class OfertaRepository : IOfertaRepository
    {
        private readonly SistemaSupermercadoContext _context;

        public OfertaRepository(SistemaSupermercadoContext context)
        {
            _context = context;
        }

        public async Task<List<OfertaProducto>> ListarOfertasAsync()
        {
            return await _context.OfertasDto
                .FromSqlRaw("EXEC PA_LISTA_OFERTA")
                .ToListAsync();
        }

        public async Task<Paginacion<OfertaProducto>> ListarOfertasPaginacionAsync(int pageNumber, int pageSize)
        {

            using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "PA_LISTA_OFERTA_PAGINACION";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@PageNumber", pageNumber));
            command.Parameters.Add(new SqlParameter("@PageSize", pageSize));

            await _context.Database.OpenConnectionAsync();

            var ofertas = new List<OfertaProducto>();
            int totalCount = 0;

            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    ofertas.Add(new OfertaProducto
                    {
                        Id_Oferta = reader.GetInt32(0),
                        Codigo = reader.GetString(1),
                        Nombre_Oferta = reader.GetString(2),
                        Id_Producto = reader.GetInt32(3),
                        Nombre_Producto = reader.GetString(4),
                        Descripcion = reader.GetString(5),
                        Fecha_Inicio = reader.IsDBNull(6) ? null : DateOnly.FromDateTime(reader.GetDateTime(6)),
                        Fecha_Fin = reader.IsDBNull(7) ? null : DateOnly.FromDateTime(reader.GetDateTime(7)),
                        Descuento = reader.GetDecimal(8),
                        Estado = reader.GetBoolean(9),
                        Fecha_Creacion = reader.GetDateTime(10)
                    });
                }

                if (await reader.NextResultAsync() && await reader.ReadAsync())
                {
                    totalCount = reader.GetInt32(0);
                }
            }

            return new Paginacion<OfertaProducto> { Items = ofertas, TotalCount = totalCount, PageNumber = pageNumber, PageSize = pageSize };
        }

        public async Task<Ofertum?> ObtenerOfertaAsync(int idOferta)
        {
            var idParam = new SqlParameter("@Id_Oferta", idOferta);
            return await Task.Run(() =>
                _context.Oferta
                .FromSqlRaw("EXEC PA_OBTENER_OFERTA @Id_Oferta", idParam)
                .AsNoTracking()
                .AsEnumerable()
                .FirstOrDefault()
            );
        }

        public async Task<int> RegistrarOfertaAsync(Ofertum oferta)
        {

            return await _context.Database.ExecuteSqlRawAsync(
                "EXEC PA_REGISTRAR_OFERTA @Codigo, @Nombre_Oferta, @Id_Producto, @Descripcion, @Fecha_Inicio, @Fecha_Fin, @Descuento, @Estado",
                new SqlParameter("@Codigo", oferta.Codigo ?? (object)DBNull.Value),
                new SqlParameter("@Nombre_Oferta", oferta.Nombre_Oferta ?? (object)DBNull.Value),
                new SqlParameter("@Id_Producto", oferta.Id_Producto ?? (object)DBNull.Value),
                new SqlParameter("@Descripcion", oferta.Descripcion ?? (object)DBNull.Value),
                new SqlParameter("@Fecha_Inicio", oferta.Fecha_Inicio ?? (object)DBNull.Value),
                new SqlParameter("@Fecha_Fin", oferta.Fecha_Fin ?? (object)DBNull.Value),
                new SqlParameter("@Descuento", oferta.Descuento ?? (object)DBNull.Value),
                new SqlParameter("@Estado", oferta.Estado ?? (object)DBNull.Value)
            );
        }

        public async Task<int> EditarOfertaAsync(Ofertum oferta)
        {

            return await _context.Database.ExecuteSqlRawAsync(
                "EXEC PA_EDITAR_OFERTA @Id_Oferta, @Nombre_Oferta, @Id_Producto, @Descripcion, @Fecha_Inicio, @Fecha_Fin, @Descuento, @Estado",
                new SqlParameter("@Id_Oferta", oferta.Id_Oferta),
                new SqlParameter("@Nombre_Oferta", oferta.Nombre_Oferta ?? (object)DBNull.Value),
                new SqlParameter("@Id_Producto", oferta.Id_Producto ?? (object)DBNull.Value),
                new SqlParameter("@Descripcion", oferta.Descripcion ?? (object)DBNull.Value),
                new SqlParameter("@Fecha_Inicio", oferta.Fecha_Inicio ?? (object)DBNull.Value),
                new SqlParameter("@Fecha_Fin", oferta.Fecha_Fin ?? (object)DBNull.Value),
                new SqlParameter("@Descuento", oferta.Descuento ?? (object)DBNull.Value),
                new SqlParameter("@Estado", oferta.Estado ?? (object)DBNull.Value)
            );
        }

        public async Task<int> EliminarOfertaAsync(int idOferta)
        {
            return await _context.Database.ExecuteSqlRawAsync(
                "EXEC PA_ELIMINAR_OFERTA @Id_Oferta",
                new SqlParameter("@Id_Oferta", idOferta)
            );
        }
    }
}
