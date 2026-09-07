using Domain.Models;
using Domain.Models.Dto.Response.Usuario;

namespace Infrastructure.Repository.InterfacesBusiness
{
    public interface IToken
    {
        string GenerarToken(UsuarioRolResponse usuario, List<Menu> permisos);
    }
}
