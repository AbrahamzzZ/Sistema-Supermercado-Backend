using DataBaseFirst.Contexts;
using DataBaseFirst.Models;
using DataBaseFirst.Models.Dto;
using Infrastructure.Repository.InterfacesRepository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Types;
using System.Data;
using Utilities.Shared;

namespace Infrastructure.Repository
{
    public class SucursalRepository : ISucursalRepository
    {
        private readonly SistemaSupermercadoContext _context;

        public SucursalRepository(SistemaSupermercadoContext context)
        {
            _context = context;
        }

        public async Task<List<Sucursal>> ListarSucursalesAsync()
        {
            return await _context.Sucursals
                .FromSqlRaw("EXEC PA_LISTA_SUCURSAL")
                .ToListAsync();
        }

        public async Task<Paginacion<Sucursal>> ListarSucursalesPaginacionAsync(int pageNumber, int pageSize)
        {
            using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "PA_LISTA_SUCURSAL_PAGINACION";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@PageNumber", pageNumber));
            command.Parameters.Add(new SqlParameter("@PageSize", pageSize));

            await _context.Database.OpenConnectionAsync();

            var sucursales = new List<Sucursal>();
            int totalCount = 0;


            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    sucursales.Add(new Sucursal
                    {
                        Id_Sucursal = reader.GetInt32(0),
                        Codigo = reader.GetString(1),
                        Id_Negocio = reader.GetInt32(2),
                        Nombre_Sucursal = reader.GetString(3),
                        Direccion_Sucursal = reader.GetString(4),
                        Latitud = reader.GetDouble(5),
                        Longitud = reader.GetDouble(6),
                        Ciudad_Sucursal = reader.GetString(7),
                        Estado = reader.GetBoolean(8)
                    });
                }

                if (await reader.NextResultAsync() && await reader.ReadAsync())
                {
                    totalCount = reader.GetInt32(0);
                }
            }
            return new Paginacion<Sucursal> { Items = sucursales, TotalCount = totalCount, PageNumber = pageNumber, PageSize = pageSize };
        }

        public async Task<Sucursal?> ObtenerSucursalAsync(int idSucursal)
        {
            var idParam = new SqlParameter("@Id_Sucursal", idSucursal);
            return await Task.Run(() =>
                _context.Sucursals
                .FromSqlRaw("EXEC PA_OBTENER_SUCURSAL @Id_Sucursal", idParam)
                .AsNoTracking()
                .AsEnumerable()
                .FirstOrDefault()
            );
        }

        public async Task<int> RegistrarSucursalAsync(Sucursal sucursal)
        {
            using var connection = _context.Database.GetDbConnection();
            await connection.OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandText = "PA_REGISTRAR_SUCURSAL";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("@Codigo", sucursal.Codigo ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@Id_Negocio", sucursal.Id_Negocio ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@Nombre", sucursal.Nombre_Sucursal ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@Direccion", sucursal.Direccion_Sucursal ?? (object)DBNull.Value));

            var ubicacionParam = new SqlParameter("@Ubicacion", SqlDbType.Udt)
            {
                UdtTypeName = "geography"
            };

            if (sucursal.Latitud.HasValue && sucursal.Longitud.HasValue)
            {
                var geography = SqlGeography.Point(sucursal.Latitud.Value, sucursal.Longitud.Value, 4326);
                ubicacionParam.Value = geography;
            }
            else
            {
                ubicacionParam.Value = DBNull.Value;
            }

            command.Parameters.Add(ubicacionParam);

            command.Parameters.Add(new SqlParameter("@Ciudad", sucursal.Ciudad_Sucursal ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@Estado", sucursal.Estado ?? (object)DBNull.Value));

            return await command.ExecuteNonQueryAsync();
        }

        public async Task<int> EditarSucursalAsync(Sucursal sucursal)
        {
            using var connection = _context.Database.GetDbConnection();
            await connection.OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandText = "PA_EDITAR_SUCURSAL";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("@Id_Sucursal", sucursal.Id_Sucursal));
            command.Parameters.Add(new SqlParameter("@Nombre", sucursal.Nombre_Sucursal ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@Direccion", sucursal.Direccion_Sucursal ?? (object)DBNull.Value));

            var ubicacionParam = new SqlParameter("@Ubicacion", SqlDbType.Udt)
            {
                UdtTypeName = "geography"
            };

            if (sucursal.Latitud.HasValue && sucursal.Longitud.HasValue)
            {
                var geography = SqlGeography.Point(sucursal.Latitud.Value, sucursal.Longitud.Value, 4326);
                ubicacionParam.Value = geography;
            }
            else
            {
                ubicacionParam.Value = DBNull.Value;
            }

            command.Parameters.Add(ubicacionParam);

            command.Parameters.Add(new SqlParameter("@Ciudad", sucursal.Ciudad_Sucursal ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@Estado", sucursal.Estado ?? (object)DBNull.Value));

            return await command.ExecuteNonQueryAsync();
        }

        public async Task<int> EliminarSucursalAsync(int idSucursal)
        {
            return await _context.Database.ExecuteSqlRawAsync(
                "EXEC PA_ELIMINAR_SUCURSAL @Id_Sucursal",
                new SqlParameter("@Id_Sucursal", idSucursal)
            );
        }
    }
}
