using DataBaseFirst.Contexts;
using DataBaseFirst.Models;
using Infrastructure.Repository.InterfacesRepository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Utilities.Shared;

namespace Infrastructure.Repository
{
    public class TransportistaRepository : ITransportistaRepository
    {
        private readonly SistemaSupermercadoContext _context;

        public TransportistaRepository(SistemaSupermercadoContext context)
        {
            _context = context;
        }

        public async Task<List<Transportistum>> ListarTransportistasAsync()
        {
            return await _context.Transportista
                .FromSqlRaw("EXEC PA_LISTA_TRANSPORTISTA")
                .ToListAsync();
        }

        public async Task<Paginacion<Transportistum>> ListarTransportistasPaginacionAsync(int pageNumber, int pageSize)
        {
            using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "PA_LISTA_TRANSPORTISTA_PAGINACION";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@PageNumber", pageNumber));
            command.Parameters.Add(new SqlParameter("@PageSize", pageSize));

            await _context.Database.OpenConnectionAsync();

            var transportistas = new List<Transportistum>();
            int totalCount = 0;


            using (var reader = await command.ExecuteReaderAsync())
            {

                while (await reader.ReadAsync())
                {
                    byte[]? imagen = null;

                    if (!await reader.IsDBNullAsync(7))
                    {
                        long length = reader.GetBytes(7, 0, null, 0, 0);
                        imagen = new byte[length];
                        reader.GetBytes(7, 0, imagen, 0, (int)length);
                    }

                    transportistas.Add(new Transportistum
                    {
                        Id_Transportista = reader.GetInt32(0),
                        Codigo = reader.GetString(1),
                        Nombres = reader.GetString(2),
                        Apellidos = reader.GetString(3),
                        Cedula = reader.GetString(4),
                        Telefono = reader.GetString(5),
                        Correo_Electronico = reader.GetString(6),
                        Imagen = imagen,
                        Estado = reader.GetBoolean(8),
                        Fecha_Registro = reader.GetDateTime(9)
                    });
                }

                if (await reader.NextResultAsync() && await reader.ReadAsync())
                {
                    totalCount = reader.GetInt32(0);
                }
            }

            return new Paginacion<Transportistum> { Items = transportistas, TotalCount = totalCount, PageNumber = pageNumber, PageSize = pageSize };
        }

        public async Task<Transportistum?> ObtenerTransportistaAsync(int idTranportista)
        {
            var idParam = new SqlParameter("@Id_Transportista", idTranportista);
            return await Task.Run(() =>
                _context.Transportista
                .FromSqlRaw("EXEC PA_OBTENER_TRANSPORTISTA @Id_Transportista", idParam)
                .AsNoTracking()
                .AsEnumerable()
                .FirstOrDefault()
            );
        }

        public async Task<int> RegistrarTransportistaAsync(Transportistum transportista)
        {
            if (!string.IsNullOrWhiteSpace(transportista.ImagenBase64))
            {
                if (transportista.ImagenBase64.Contains(","))
                {
                    transportista.ImagenBase64 = transportista.ImagenBase64.Split(',')[1];
                }

                transportista.Imagen = Convert.FromBase64String(transportista.ImagenBase64);
            }

            return await _context.Database.ExecuteSqlRawAsync(
                "EXEC PA_REGISTRAR_TRANSPORTISTA @Codigo, @Nombres, @Apellidos, @Cedula, @Telefono, @Correo_Electronico, @Imagen, @Estado",
                new SqlParameter("@Codigo", transportista.Codigo ?? (object)DBNull.Value),
                new SqlParameter("@Nombres", transportista.Nombres ?? (object)DBNull.Value),
                new SqlParameter("@Apellidos", transportista.Apellidos ?? (object)DBNull.Value),
                new SqlParameter("@Cedula", transportista.Cedula ?? (object)DBNull.Value),
                new SqlParameter("@Telefono", transportista.Telefono ?? (object)DBNull.Value),
                new SqlParameter("@Correo_Electronico", transportista.Correo_Electronico ?? (object)DBNull.Value),
                new SqlParameter("@Imagen", transportista.Imagen ?? (object)DBNull.Value),
                new SqlParameter("@Estado", transportista.Estado ?? (object)DBNull.Value)
            );
        }

        public async Task<int> EditarTransportistaAsync(Transportistum transportista)
        {
            if (!string.IsNullOrWhiteSpace(transportista.ImagenBase64))
            {
                if (transportista.ImagenBase64.Contains(","))
                {
                    transportista.ImagenBase64 = transportista.ImagenBase64.Split(',')[1];
                }

                transportista.Imagen = Convert.FromBase64String(transportista.ImagenBase64);
            }

            return await _context.Database.ExecuteSqlRawAsync(
                "EXEC PA_EDITAR_TRANSPORTISTA @Id_Transportista, @Nombres, @Apellidos, @Cedula, @Telefono, @Correo_Electronico, @Imagen, @Estado",
                new SqlParameter("@Id_Transportista", transportista.Id_Transportista),
                new SqlParameter("@Nombres", transportista.Nombres ?? (object)DBNull.Value),
                new SqlParameter("@Apellidos", transportista.Apellidos ?? (object)DBNull.Value),
                new SqlParameter("@Cedula", transportista.Cedula ?? (object)DBNull.Value),
                new SqlParameter("@Telefono", transportista.Telefono ?? (object)DBNull.Value),
                new SqlParameter("@Correo_Electronico", transportista.Correo_Electronico ?? (object)DBNull.Value),
                new SqlParameter("@Imagen", transportista.Imagen ?? (object)DBNull.Value),
                new SqlParameter("@Estado", transportista.Estado ?? (object)DBNull.Value)
            );
        }

        public async Task<int> EliminarTransportistaAsync(int idTransportista)
        {
            return await _context.Database.ExecuteSqlRawAsync(
                "EXEC PA_ELIMINAR_TRANSPORTISTA @Id_Transportista",
                new SqlParameter("@Id_Transportista", idTransportista)
            );
        }
    }
}
