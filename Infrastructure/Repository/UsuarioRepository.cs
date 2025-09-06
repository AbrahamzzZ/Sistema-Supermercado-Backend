using DataBaseFirst.Contexts;
using DataBaseFirst.Models;
using DataBaseFirst.Models.Dto;
using Infrastructure.Repository.InterfacesRepository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Utilities.Security;
using Utilities.Shared;

namespace Infrastructure.Repository
{
    public class UsuarioRepository :IUsuarioRepository
    {
        private readonly SistemaSupermercadoContext _context;

        public UsuarioRepository(SistemaSupermercadoContext context)
        {
            _context = context;
        }

        public async Task<List<UsuarioRol>> ListarUsuariosAsync()
        {
            return await _context.UsuariosDto
                .FromSqlRaw("EXEC PA_LISTA_USUARIO")
                .ToListAsync();
        }

        public async Task<Paginacion<UsuarioRol>> ListarUsuariosPaginacionAsync(int pageNumber, int pageSize)
        {

            using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "PA_LISTA_USUARIO_PAGINACION";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@PageNumber", pageNumber));
            command.Parameters.Add(new SqlParameter("@PageSize", pageSize));

            await _context.Database.OpenConnectionAsync();

            var usuarios = new List<UsuarioRol>();
            int totalCount = 0;

            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    usuarios.Add(new UsuarioRol
                    {
                        Id_Usuario = reader.GetInt32(0),
                        Codigo = reader.GetString(1),
                        Nombre_Completo = reader.GetString(2),
                        Correo_Electronico = reader.GetString(3),
                        Id_Rol = reader.GetInt32(4),
                        Nombre_Rol = reader.GetString(5),
                        Estado = reader.GetBoolean(6),
                        Fecha_Creacion = reader.GetDateTime(7)
                    });
                }

                if (await reader.NextResultAsync() && await reader.ReadAsync())
                {
                    totalCount = reader.GetInt32(0);
                }
            }

            return new Paginacion<UsuarioRol> { Items = usuarios, TotalCount = totalCount, PageNumber = pageNumber, PageSize = pageSize };
        }

        public async Task<UsuarioRol?> ObtenerUsuarioAsync(int idUsuario)
        {
            var idParam = new SqlParameter("@Id_Usuario", idUsuario);
            return await Task.Run(() =>
                _context.UsuariosDto
                .FromSqlRaw("EXEC PA_OBTENER_USUARIO @Id_Usuario", idParam)
                .AsNoTracking()
                .AsEnumerable()
                .FirstOrDefault()
            );
        }

        public async Task<UsuarioRol?> IniciarSesionAsync(Login login)
        {
            string claveEncriptada = Encriptacion.EncriptarSHA256(login.Clave ?? "");

            var correoParam = new SqlParameter("@Correo_Electronico", login.Correo_Electronico ?? (object)DBNull.Value);
            var claveParam = new SqlParameter("@Clave", claveEncriptada);

            return await Task.Run(() =>
                _context.UsuariosDto
                .FromSqlRaw("EXEC PA_INICIAR_SESION @Correo_Electronico, @Clave", correoParam, claveParam)
                .AsNoTracking()
                .AsEnumerable()
                .FirstOrDefault()
            );
        }

        public async Task<int> RegistrarUsuarioAsync(Usuario usuario)
        {
            string claveEncriptada = Encriptacion.EncriptarSHA256(usuario.Clave ?? "");

            return await _context.Database.ExecuteSqlRawAsync(
                "EXEC PA_REGISTRAR_USUARIO @Codigo, @Nombre_Completo, @Correo_Electronico, @Clave, @Id_Rol, @Estado",
                new SqlParameter("@Codigo", usuario.Codigo ?? (object)DBNull.Value),
                new SqlParameter("@Nombre_Completo", usuario.Nombre_Completo ?? (object)DBNull.Value),
                new SqlParameter("@Correo_Electronico", usuario.Correo_Electronico ?? (object)DBNull.Value),
                new SqlParameter("@Clave", claveEncriptada),
                new SqlParameter("@Id_Rol", usuario.Id_Rol ?? (object)DBNull.Value),
                new SqlParameter("@Estado", usuario.Estado ?? (object)DBNull.Value)
            );
        }

        public async Task<int> EditarUsuarioAsync(Usuario usuario)
        {
            var usuarioActual = await _context.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id_Usuario == usuario.Id_Usuario);

            if (usuarioActual == null)
                throw new Exception("Usuario no encontrado");

            string claveEncriptada;

            // Si el usuario envía una nueva clave
            if (!string.IsNullOrEmpty(usuario.Clave))
            {
                claveEncriptada = Encriptacion.EncriptarSHA256(usuario.Clave);
            }
            else
            {
                // Si no envió nada, mantenemos la clave actual
                claveEncriptada = usuarioActual.Clave ?? "";
            }

            return await _context.Database.ExecuteSqlRawAsync(
                "EXEC PA_EDITAR_USUARIO @Id_Usuario, @Nombre_Completo, @Correo_Electronico, @Clave, @Id_Rol, @Estado",
                new SqlParameter("@Id_Usuario", usuario.Id_Usuario),
                new SqlParameter("@Nombre_Completo", usuario.Nombre_Completo ?? (object)DBNull.Value),
                new SqlParameter("@Correo_Electronico", usuario.Correo_Electronico ?? (object)DBNull.Value),
                new SqlParameter("@Clave", claveEncriptada),
                new SqlParameter("@Id_Rol", usuario.Id_Rol ?? (object)DBNull.Value),
                new SqlParameter("@Estado", usuario.Estado ?? (object)DBNull.Value)
            );
        }

        public async Task<int> EliminarUsuarioAsync(int idUsuario)
        {
            return await _context.Database.ExecuteSqlRawAsync(
                "EXEC PA_ELIMINAR_USUARIO @Id_Usuario",
                new SqlParameter("@Id_Usuario", idUsuario)
            );
        }
    }
}
