using Domain.Models;
using Domain.Models.Dto.Response.Usuario;

namespace Infrastructure.Helpers
{
    public interface IToken
    {
        string GenerarToken(UsuarioRolResponse usuario, List<Menu> permisos);
    }
}
