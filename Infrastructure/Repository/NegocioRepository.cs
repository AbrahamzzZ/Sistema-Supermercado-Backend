using Domain.Contexts;
using Domain.Models;
using Domain.Models.Dto.Negocio;
using Domain.Models.Dto.Negocio.IA;
using Infrastructure.Repository.InterfacesRepository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class NegocioRepository : INegocioRepository
    {
        private readonly SistemaSupermercadoContext _context;

        public NegocioRepository(SistemaSupermercadoContext context)
        {
            _context = context;
        }

        public async Task<Negocio?> ObtenerNegocioAsync(int idNegocio)
        {
            var idParam = new SqlParameter("@Id_Negocio", idNegocio);
            return await Task.Run(() => _context.Negocios
                .FromSqlRaw("EXEC PA_OBTENER_NEGOCIO @Id_Negocio", idParam)
                .AsNoTracking()
                .AsEnumerable()
                .FirstOrDefault()
            );
        }

        public async Task<int> EditarNegocioAsync(Negocio negocio)
        {
            if (!string.IsNullOrWhiteSpace(negocio.ImagenBase64))
            {
                if (negocio.ImagenBase64.Contains(','))
                {
                    negocio.ImagenBase64 = negocio.ImagenBase64.Split(',')[1];
                }

                negocio.Logo = Convert.FromBase64String(negocio.ImagenBase64);
            }

            return await _context.Database.ExecuteSqlRawAsync(
                "EXEC PA_EDITAR_NEGOCIO @Id_Negocio, @Nombre, @Telefono, @Ruc, @Direccion, @Correo_Electronico, @Logo",
                new SqlParameter("@Id_Negocio", negocio.Id_Negocio),
                new SqlParameter("@Nombre", negocio.Nombre ?? (object)DBNull.Value),
                new SqlParameter("@Telefono", negocio.Telefono ?? (object)DBNull.Value),
                new SqlParameter("@Ruc", negocio.Ruc ?? (object)DBNull.Value),
                new SqlParameter("@Direccion", negocio.Direccion ?? (object)DBNull.Value),
                new SqlParameter("@Correo_Electronico", negocio.Correo_Electronico ?? (object)DBNull.Value),
                new SqlParameter("@Logo", negocio.Logo ?? (object)DBNull.Value)
            );
        }

        public async Task<List<ProductoMasComprado>> ObtenerProductoMasComprado()
        {
            return await _context.ProductoMasComprados.FromSqlRaw("EXEC PA_PRODUCTOS_MAS_COMPRADOS").ToListAsync();
        }

        public async Task<List<ProductoMasCompradoAnalisisIA>> ObtenerAnalisisProductosComprados()
        {
            return await _context.ProductoMasCompradosAnalisis.FromSqlRaw("EXEC PA_TENDENCIA_PRODUCTOS_COMPRADOS").ToListAsync();
        }

        public async Task<List<ProductoMasVendido>> ObtenerProductoMasVendido()
        {
            return await _context.ProductoMasVendidos.FromSqlRaw("EXEC PA_PRODUCTOS_MAS_VENDIDOS").ToListAsync();
        }

        public async Task<List<ProductoMasVendidoAnalisisIA>> ObtenerAnalisisProductosVendidos()
        {
            return await _context.ProductoMasVendidosAnalisis.FromSqlRaw("EXEC PA_TENDENCIA_PRODUCTOS_VENDIDOS").ToListAsync();
        }

        public async Task<List<TopCliente>> ObtenerTopClientes()
        {
            return await _context.TopClientes.FromSqlRaw("EXEC PA_TOP_CLIENTES").ToListAsync();
        }

        public async Task<List<TopProveedor>> ObtenerTopProveedores()
        {
            return await _context.TopProveedores.FromSqlRaw("EXEC PA_TOP_PROVEEDORES").ToListAsync();
        }

        public async Task<List<ViajesTransportista>> ObtenerViajesTransportista()
        {
            return await _context.ViajesTransportistas.FromSqlRaw("PA_VIAJES_TRANSPORTISTA").ToListAsync();
        }

        public async Task<List<EmpleadoProductivo>> ObtenerEmpleadosProductivos()
        {
            return await _context.EmpleadoProductivos.FromSqlRaw("EXEC PA_VENTAS_EMPLEADO").ToListAsync();
        }
    }
}
