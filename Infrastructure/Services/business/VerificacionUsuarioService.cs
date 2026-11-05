using Infrastructure.Repository.InterfacesBusiness;
using Infrastructure.Repository.InterfacesRepository;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services.business
{
    // El token JWT sigue siendo válido hasta que vence aunque el usuario se desactive o cambie de rol.
    // Este servicio consulta el estado real del usuario y lo guarda en memoria unos segundos para no ir a la base de datos en cada petición.
    public class VerificacionUsuarioService : IVerificacionUsuario
    {
        private const int SegundosPorDefecto = 60;

        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMemoryCache _cache;
        private readonly TimeSpan _duracion;

        public VerificacionUsuarioService(IUsuarioRepository usuarioRepository, IMemoryCache cache, IConfiguration configuration)
        {
            _usuarioRepository = usuarioRepository;
            _cache = cache;
            var segundos = configuration.GetValue<int?>("Seguridad:VerificacionUsuarioSegundos") ?? SegundosPorDefecto;
            _duracion = TimeSpan.FromSeconds(Math.Max(0, segundos));
        }

        public async Task<bool> EsSesionValidaAsync(int idUsuario, string? rolDelToken)
        {
            if (idUsuario <= 0)
                return false;

            var estado = await _cache.GetOrCreateAsync(Clave(idUsuario), async entrada =>
            {
                entrada.AbsoluteExpirationRelativeToNow = _duracion;
                var usuario = await _usuarioRepository.ObtenerUsuarioAsync(idUsuario);
                return usuario == null ? null : new EstadoUsuario(usuario.Estado == true, usuario.Nombre_Rol);
            });

            // Si el usuario no existe, está inactivo o su rol cambió, el token deja de servir
            return estado is { Activo: true }
                && string.Equals(estado.Rol, rolDelToken, StringComparison.OrdinalIgnoreCase);
        }

        // Se llama al editar o eliminar un usuario para que el cambio tenga efecto inmediato
        public void Invalidar(int idUsuario) => _cache.Remove(Clave(idUsuario));

        private static string Clave(int idUsuario) => $"verificacion-usuario:{idUsuario}";

        private sealed record EstadoUsuario(bool Activo, string? Rol);
    }
}
