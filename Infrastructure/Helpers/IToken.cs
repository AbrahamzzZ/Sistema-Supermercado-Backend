using Domain.Models;
using Domain.Models.Dto;

namespace Infrastructure.Helpers
{
    public interface IToken
    {
        string GenerarToken(UsuarioRol usuario, List<Menu> permisos);
    }
}
