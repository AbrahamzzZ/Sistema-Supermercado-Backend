using Domain.Contexts;
using Domain.Models.Dto.Response.Venta;
using Infrastructure.Repository.InterfacesRepository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Utilities.Shared;

namespace Infrastructure.Repository
{
    public class VentaRepository : IVentaRepository
    {
        private readonly SistemaSupermercadoContext _context;

        public VentaRepository(SistemaSupermercadoContext context)
        {
            _context = context;
        }

        // Número referencial para mostrar en pantalla; el definitivo lo asigna PA_REGISTRAR_VENTA al guardar
        public async Task<string> ObtenerNumeroDocumentoAsync()
        {
            var ultimo = await _context.Database
                .SqlQueryRaw<int?>("SELECT MAX(TRY_CAST(NUMERO_DOCUMENTO AS INT)) AS [Value] FROM VENTA")
                .SingleAsync();

            var siguiente = (ultimo ?? 0) + 1;
            return siguiente < 100000 ? siguiente.ToString("D5") : siguiente.ToString();
        }

        public async Task<VentaResponse?> ObtenerVentaAsync(string numeroDocumento)
        {
            var resultado = await _context.VentaDto
                .FromSqlRaw("EXEC PA_OBTENER_VENTA @Numero_Documento", new SqlParameter("@Numero_Documento", numeroDocumento))
                .AsNoTracking()
                .ToListAsync();

            return resultado.FirstOrDefault();
        }

        public async Task<List<DetalleVentaReponse>> ObtenerDetallesVentaAsync(int idVenta)
        {
            return await _context.DetalleVentasRepuestaDto
                .FromSqlRaw("EXEC PA_OBTENER_DETALLES_VENTA @Id_Venta", new SqlParameter("@Id_Venta", idVenta))
                .AsNoTracking()
                .ToListAsync();
        }

        // El procedimiento calcula precios, ofertas, totales, cambio y número de documento.
        // Sus errores de negocio llegan como THROW 50001..50099 y se convierten en ReglaNegocioException.
        public async Task<VentaRegistradaResponse> RegistrarVentaAsync(Ventas ventaDto, int idUsuario)
        {
            var tablaDetalle = new DataTable();
            tablaDetalle.Columns.Add("Id_Producto", typeof(int));
            tablaDetalle.Columns.Add("Cantidad", typeof(int));

            foreach (var item in ventaDto.Detalles ?? Enumerable.Empty<DetalleVentas>())
            {
                tablaDetalle.Rows.Add(item.Id_Producto, item.Cantidad);
            }

            var numeroParam = new SqlParameter("@NUMERO_DOCUMENTO", SqlDbType.VarChar, 50) { Direction = ParameterDirection.Output };
            var totalParam = new SqlParameter("@MONTO_TOTAL", SqlDbType.Decimal) { Precision = 10, Scale = 2, Direction = ParameterDirection.Output };
            var descuentoParam = new SqlParameter("@DESCUENTO", SqlDbType.Decimal) { Precision = 10, Scale = 2, Direction = ParameterDirection.Output };
            var cambioParam = new SqlParameter("@MONTO_CAMBIO", SqlDbType.Decimal) { Precision = 10, Scale = 2, Direction = ParameterDirection.Output };

            var parametros = new[]
            {
                new SqlParameter("@ID_USUARIO", idUsuario),
                new SqlParameter("@ID_SUCURSAL", ventaDto.Id_Sucursal),
                new SqlParameter("@ID_CLIENTE", ventaDto.Id_Cliente),
                new SqlParameter("@TIPO_DOCUMENTO", ventaDto.Tipo_Documento),
                new SqlParameter("@MONTO_PAGO", SqlDbType.Decimal) { Precision = 10, Scale = 2, Value = ventaDto.Monto_Pago },
                new SqlParameter("@DETALLE_VENTA", SqlDbType.Structured)
                {
                    TypeName = "TI_Detalle_Venta",
                    Value = tablaDetalle
                },
                numeroParam,
                totalParam,
                descuentoParam,
                cambioParam
            };

            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC PA_REGISTRAR_VENTA @ID_USUARIO, @ID_SUCURSAL, @ID_CLIENTE, @TIPO_DOCUMENTO, @MONTO_PAGO, @DETALLE_VENTA, " +
                    "@NUMERO_DOCUMENTO OUT, @MONTO_TOTAL OUT, @DESCUENTO OUT, @MONTO_CAMBIO OUT", parametros);
            }
            catch (SqlException ex) when (ex.Number >= 50001 && ex.Number <= 50099)
            {
                throw new ReglaNegocioException(ex.Message, ex);
            }

            return new VentaRegistradaResponse
            {
                Numero_Documento = (string)numeroParam.Value,
                Monto_Total = (decimal)totalParam.Value,
                Descuento = (decimal)descuentoParam.Value,
                Monto_Pago = ventaDto.Monto_Pago,
                Monto_Cambio = (decimal)cambioParam.Value
            };
        }
    }
}
