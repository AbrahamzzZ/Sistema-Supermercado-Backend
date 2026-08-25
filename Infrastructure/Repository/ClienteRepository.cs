using Domain.Contexts;
using Domain.Models;
using Domain.Models.Dto.Response.Cliente;
using Infrastructure.Repository.InterfacesRepository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Utilities.Shared;


namespace Infrastructure.Repository
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly SistemaSupermercadoContext _context;

        public ClienteRepository(SistemaSupermercadoContext context)
        {
            _context = context;
        }

        public async Task<List<ClienteResponse>> ListarClientesAsync()
        {
            return await _context.ClientesDto
                .FromSqlRaw("EXEC PA_LISTA_CLIENTE")
                .ToListAsync();
        }

        public async Task<Paginacion<ClienteResponse>> ListarClientesPaginacionAsync(int pageNumber, int pageSize)
        {
            using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "PA_LISTA_CLIENTE_PAGINACION";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@PageNumber", pageNumber));
            command.Parameters.Add(new SqlParameter("@PageSize", pageSize));

            await _context.Database.OpenConnectionAsync();

            var clientes = new List<ClienteResponse>();
            int totalCount = 0;

            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    clientes.Add(new ClienteResponse
                    {
                        Id_Cliente = reader.GetInt32(0),
                        Codigo = reader.GetString(1),
                        Nombres = reader.GetString(2),
                        Apellidos = reader.GetString(3),
                        Cedula = reader.GetString(4),
                        Telefono = reader.GetString(5),
                        Correo_Electronico = reader.GetString(6),
                        Fecha_Creacion = reader.GetDateTime(7)
                    });
                }

                if (await reader.NextResultAsync() && await reader.ReadAsync())
                {
                    totalCount = reader.GetInt32(0);
                }
            }


            return new Paginacion<ClienteResponse> { Items = clientes, TotalCount = totalCount, PageNumber = pageNumber, PageSize = pageSize };
        }

        public async Task<ClienteResponse?> ObtenerClienteAsync(int idCliente)
        {
            var idParam = new SqlParameter("@Id_Cliente", idCliente);
            return await Task.Run(() => _context.ClientesDto
                .FromSqlRaw("EXEC PA_OBTENER_CLIENTE @Id_Cliente", idParam)
                .AsNoTracking()
                .AsEnumerable()
                .FirstOrDefault()
            );
        }

        public async Task<int> RegistrarClienteAsync(Cliente cliente, int usuarioCreacion)
        {
            return await _context.Database.ExecuteSqlRawAsync(
                "EXEC PA_REGISTRAR_CLIENTE @Codigo, @Nombres, @Apellidos, @Cedula, @Telefono, @Correo_Electronico, @Usuario_Creacion",
                new SqlParameter("@Codigo", cliente.Codigo ?? (object)DBNull.Value),
                new SqlParameter("@Nombres", cliente.Nombres ?? (object)DBNull.Value),
                new SqlParameter("@Apellidos", cliente.Apellidos ?? (object)DBNull.Value),
                new SqlParameter("@Cedula", cliente.Cedula ?? (object)DBNull.Value),
                new SqlParameter("@Telefono", cliente.Telefono ?? (object)DBNull.Value),
                new SqlParameter("@Correo_Electronico", cliente.Correo_Electronico ?? (object)DBNull.Value),
                new SqlParameter("@Usuario_Creacion", usuarioCreacion)
            );
        }

        public async Task<int> EditarClienteAsync(Cliente cliente, int usuarioModificacion)
        {
            return await _context.Database.ExecuteSqlRawAsync(
                "EXEC PA_EDITAR_CLIENTE @Id_Cliente, @Nombres, @Apellidos, @Cedula, @Telefono, @Correo_Electronico, @Usuario_Modificacion",
                new SqlParameter("@Id_Cliente", cliente.Id_Cliente),
                new SqlParameter("@Nombres", cliente.Nombres ?? (object)DBNull.Value),
                new SqlParameter("@Apellidos", cliente.Apellidos ?? (object)DBNull.Value),
                new SqlParameter("@Cedula", cliente.Cedula ?? (object)DBNull.Value),
                new SqlParameter("@Telefono", cliente.Telefono ?? (object)DBNull.Value),
                new SqlParameter("@Correo_Electronico", cliente.Correo_Electronico ?? (object)DBNull.Value),
                new SqlParameter("@Usuario_Modificacion", usuarioModificacion)
            );
        }

        public async Task<int> EliminarClienteAsync(int idCliente)
        {
            return await _context.Database.ExecuteSqlRawAsync(
                "EXEC PA_ELIMINAR_CLIENTE @Id_Cliente",
                new SqlParameter("@Id_Cliente", idCliente)
            );
        }
    }
}
