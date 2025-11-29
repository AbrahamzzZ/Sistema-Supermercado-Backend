using Domain.Contexts;
using Domain.Models;
using Infrastructure.Repository.InterfacesRepository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Utilities.Shared;

namespace Infrastructure.Repository
{
    public class LogRepository : ILogRepository
    {
        private readonly SistemaSupermercadoContext _context;

        public LogRepository(SistemaSupermercadoContext context)
        {
            _context = context;
        }

        public async Task<Paginacion<Log>> ListarLogsPaginacionAsync(int pageNumber, int pageSize)
        {
            using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "PA_LISTA_LOG_PAGINACION";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@PageNumber", pageNumber));
            command.Parameters.Add(new SqlParameter("@PageSize", pageSize));

            await _context.Database.OpenConnectionAsync();
            var logs = new List<Log>();
            int totalCount = 0;

            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    logs.Add(new Log
                    {
                        Id_Log = reader.GetInt32(0),
                        Codigo_Error = reader.GetString(1),
                        Mensaje_Error = reader.GetString(2),
                        Detalle_Error = reader.GetString(3),
                        Id_Usuario = reader.IsDBNull(4) ? null : reader.GetInt32(4),
                        Fecha = reader.GetDateTime(5),
                        Endpoint = reader.GetString(6),
                        Metodo = reader.GetString(7),
                        Nivel = reader.GetString(8)
                    });
                }

                if (await reader.NextResultAsync() && await reader.ReadAsync())
                {
                    totalCount = reader.GetInt32(0);
                }
            }

            return new Paginacion<Log> { Items = logs, TotalCount = totalCount, PageNumber = pageNumber, PageSize = pageSize };
        }

        public async Task<Log?> ObtenerLogAsync(int idLog)
        {
            var idParam = new SqlParameter("@Id_Log", idLog);
            return await Task.Run(() => _context.Logs
                .FromSqlRaw("EXEC PA_OBTENER_LOG @Id_Log", idParam)
                .AsNoTracking()
                .AsEnumerable()
                .FirstOrDefault()
            );
        }

        public async Task<int> RegistrarLogAsync(Log log)
        {
            return await _context.Database.ExecuteSqlRawAsync(
                "EXEC PA_REGISTRAR_LOG @Codigo_Error, @Mensaje_Error, @Detalle_Error, @Id_Usuario, @EndPoint, @Metodo, @Nivel",
                new SqlParameter("@Codigo_Error", log.Codigo_Error ?? (object)DBNull.Value),
                new SqlParameter("@Mensaje_Error", log.Mensaje_Error ?? (object)DBNull.Value),
                new SqlParameter("@Detalle_Error", log.Detalle_Error ?? (object)DBNull.Value),
                new SqlParameter("@Id_Usuario", log.Id_Usuario ?? (object)DBNull.Value),
                new SqlParameter("@EndPoint", log.Endpoint ?? (object)DBNull.Value),
                new SqlParameter("@Metodo", log.Metodo ?? (object) DBNull.Value),
                new SqlParameter("@Nivel", log.Nivel ?? (object)DBNull.Value)
            );
        }
    }
}
