using Domain.Contexts;
using Domain.Models.Dto.Venta;
using Infrastructure.Repository.InterfacesRepository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Infrastructure.Repository
{
    public class VentaRepository : IVentaRepository
    {
        private readonly SistemaSupermercadoContext _context;

        public VentaRepository(SistemaSupermercadoContext context)
        {
            _context = context;
        }

        public async Task<string> ObtenerNumeroDocumentoAsync()
        {
            string nuevoNumero = "00001";

            var ultimaCompra = await _context.Venta
                .OrderByDescending(c => c.IdVenta)
                .FirstOrDefaultAsync();

            if (ultimaCompra != null && !string.IsNullOrEmpty(ultimaCompra.NumeroDocumento))
            {
                if (int.TryParse(ultimaCompra.NumeroDocumento, out int numero))
                {
                    numero++;
                    nuevoNumero = numero.ToString("D5"); // 5 dígitos con ceros a la izquierda
                }
            }

            return nuevoNumero;
        }

        public async Task<VentaRespuesta?> ObtenerVentaAsync(string numeroDocumento)
        {
            var resultado = await _context.VentaDto
                .FromSqlRaw("EXEC PA_OBTENER_VENTA @Numero_Documento", new SqlParameter("@Numero_Documento", numeroDocumento))
                .AsNoTracking()
                .ToListAsync();

            return resultado.FirstOrDefault();
        }

        public async Task<List<DetalleVentasRepuesta>> ObtenerDetallesVentaAsync(int idVenta)
        {
            return await _context.DetalleVentasRepuestaDto
                .FromSqlRaw("EXEC PA_OBTENER_DETALLES_VENTA @Id_Venta", new SqlParameter("@Id_Venta", idVenta))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> RegistrarVentaAsync(Ventas ventaDto)
        {
            string nuevoNumeroDocumento = await ObtenerNumeroDocumentoAsync();

            var tablaDetalle = new DataTable();
            tablaDetalle.Columns.Add("Id_Producto", typeof(int));
            tablaDetalle.Columns.Add("Precio_Venta", typeof(decimal));
            tablaDetalle.Columns.Add("Cantidad", typeof(int));
            tablaDetalle.Columns.Add("SubTotal", typeof(decimal));
            tablaDetalle.Columns.Add("Descuento", typeof(decimal));

            foreach (var item in ventaDto.Detalles)
            {
                tablaDetalle.Rows.Add(item.Id_Producto, item.Precio_Venta, item.Cantidad, item.SubTotal, item.Descuento);
            }

            var resultadoParam = new SqlParameter("@RESULTADO", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };

            var parametros = new[]
            {
                new SqlParameter("@ID_USUARIO", ventaDto.Id_Usuario),
                new SqlParameter("@ID_SUCURSAL", ventaDto.Id_Sucursal),
                new SqlParameter("@ID_CLIENTE", ventaDto.Id_Cliente),
                new SqlParameter("@TIPO_DOCUMENTO", ventaDto.Tipo_Documento),
                new SqlParameter("@NUMERO_DOCUMENTO", ventaDto.Numero_Documento),
                //new SqlParameter("@NUMERO_DOCUMENTO",nuevoNumeroDocumento),
                new SqlParameter("@MONTO_PAGO", ventaDto.Monto_Pago),
                new SqlParameter("@MONTO_CAMBIO", ventaDto.Monto_Cambio),
                new SqlParameter("@MONTO_TOTAL", ventaDto.Monto_Total),
                new SqlParameter("@DESCUENTO", ventaDto.Descuento),
                new SqlParameter("@DETALLE_VENTA", SqlDbType.Structured)
                {
                    TypeName = "TI_Detalle_Venta",
                    Value = tablaDetalle
                },
                resultadoParam
            };

            await _context.Database.ExecuteSqlRawAsync("EXEC PA_REGISTRAR_VENTA @ID_USUARIO, @ID_SUCURSAL, @ID_CLIENTE, @TIPO_DOCUMENTO, @NUMERO_DOCUMENTO, @MONTO_PAGO, @MONTO_CAMBIO, @MONTO_TOTAL, @DESCUENTO, @DETALLE_VENTA, @RESULTADO OUT", parametros);

            return (bool)resultadoParam.Value;
        }
    }
}
