using DataBaseFirst.Contexts;
using DataBaseFirst.Models.Dto;
using DataBaseFirst.Repository.InterfacesRepository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DataBaseFirst.Repository
{
    public class CompraRepository : ICompraRepository
    {
        private readonly SistemaSupermercadoContext _context;

        public CompraRepository(SistemaSupermercadoContext context)
        {
            _context = context;
        }

        public async Task<string> ObtenerNumeroDocumentoAsync()
        {
            string nuevoNumero = "00001";

            var ultimaCompra = await _context.Compras
                .OrderByDescending(c => c.IdCompra)
                .FirstOrDefaultAsync();

            if (ultimaCompra != null && !string.IsNullOrEmpty(ultimaCompra.NumeroDocumento))
            {
                if (int.TryParse(ultimaCompra.NumeroDocumento, out int numero))
                {
                    numero++;
                    nuevoNumero = numero.ToString("D5");
                }
            }

            return nuevoNumero;
        }

        public async Task<CompraRespuesta?> ObtenerCompraAsync(string numeroDocumento)
        {
            var resultado = await _context.CompraDto
                .FromSqlRaw("EXEC PA_OBTENER_COMPRA @Numero_Documento", new SqlParameter("@Numero_Documento", numeroDocumento))
                .AsNoTracking()
                .ToListAsync();

            return resultado.FirstOrDefault();
        }

        public async Task<List<DetalleComprasRepuesta>> ObtenerDetallesCompraAsync(int idCompra)
        {
            return await _context.DetalleComprasRepuestaDto
                .FromSqlRaw("EXEC PA_OBTENER_DETALLES_COMPRA @Id_Compra", new SqlParameter("@Id_Compra", idCompra))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> RegistrarCompraAsync(Compras compraDto)
        {
            string nuevoNumeroDocumento = await ObtenerNumeroDocumentoAsync();

            var tablaDetalle = new DataTable();
            tablaDetalle.Columns.Add("Id_Producto", typeof(int));
            tablaDetalle.Columns.Add("Precio_Compra", typeof(decimal));
            tablaDetalle.Columns.Add("Precio_Venta", typeof(decimal));
            tablaDetalle.Columns.Add("Cantidad", typeof(int));
            tablaDetalle.Columns.Add("SubTotal", typeof(decimal));

            foreach (var item in compraDto.Detalles)
            {
                tablaDetalle.Rows.Add(item.Id_Producto, item.Precio_Compra, item.Precio_Venta, item.Cantidad, item.SubTotal);
            }

            var resultadoParam = new SqlParameter("@RESULTADO", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };

            var parametros = new[]
            {
                new SqlParameter("@ID_USUARIO", compraDto.Id_Usuario),
                new SqlParameter("@ID_SUCURSAL", compraDto.Id_Sucursal),
                new SqlParameter("@ID_PROVEEDOR", compraDto.Id_Proveedor),
                new SqlParameter("@ID_TRANSPORTISTA", compraDto.Id_Transportista),
                new SqlParameter("@TIPO_DOCUMENTO", compraDto.Tipo_Documento),
                new SqlParameter("@NUMERO_DOCUMENTO", compraDto.Numero_Documento),
                //new SqlParameter("@NUMERO_DOCUMENTO",nuevoNumeroDocumento),
                new SqlParameter("@MONTO_TOTAL", compraDto.Monto_Total),
                new SqlParameter("@DETALLE_COMPRA", SqlDbType.Structured)
                {
                    TypeName = "TI_Detalle_Compra",
                    Value = tablaDetalle
                },
                resultadoParam
            };

            await _context.Database.ExecuteSqlRawAsync("EXEC PA_REGISTRAR_COMPRA @ID_USUARIO, @ID_SUCURSAL, @ID_PROVEEDOR, @ID_TRANSPORTISTA, @TIPO_DOCUMENTO, @NUMERO_DOCUMENTO, @MONTO_TOTAL, @DETALLE_COMPRA, @RESULTADO OUT", parametros);

            return (bool)resultadoParam.Value;
        }
    }
}
