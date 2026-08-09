using Domain.Contexts;
using Domain.Models;
using Infrastructure.Repository.InterfacesRepository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class MenuRepository : IMenuRepository
    {
        private readonly SistemaSupermercadoContext _context;

        public MenuRepository(SistemaSupermercadoContext context)
        {
            _context = context;
        }

        public async Task<List<Menu>> ObtenerMenusAsync(int idUsuario)
        {
            var idParam = new SqlParameter("@Id_Usuario", idUsuario);
            return await _context.Menus
                .FromSqlRaw("EXEC PA_OBTENER_MENU @Id_Usuario", idParam)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
