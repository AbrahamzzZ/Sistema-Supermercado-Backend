using DataBaseFirst.Contexts;
using DataBaseFirst.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DataBaseFirst.Services
{
    public class MenuService 
    {
        private readonly SistemaSupermercadoContext _context;

        public MenuService(SistemaSupermercadoContext context)
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
