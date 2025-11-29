using Domain.Contexts;
using Domain.Models;
using Infrastructure.Repository.InterfacesRepository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class RolRepository : IRolRepository
    {
        private readonly SistemaSupermercadoContext _context;

        public RolRepository(SistemaSupermercadoContext context)
        {
            _context = context;
        }

        public async Task<List<Rol>> ListarRolesAsync()
        {
            return await _context.Rols
                .FromSqlRaw("EXEC PA_LISTA_Rol")
                .ToListAsync();
        }
    }
}
