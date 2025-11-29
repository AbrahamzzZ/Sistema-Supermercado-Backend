using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseFirst.Models.Dto.Venta
{
    public class Ventas
    {
        public int Id_Usuario { get; set; }
        public int Id_Sucursal { get; set; }
        public int Id_Cliente { get; set; }
        public string Tipo_Documento { get; set; } = string.Empty;
        public string Numero_Documento { get; set; } = string.Empty;
        public decimal Monto_Pago { get; set; }
        public decimal Monto_Cambio { get; set; }
        public decimal Monto_Total { get; set; }
        public decimal Descuento { get; set; }
        public List<DetalleVentas>? Detalles { get; set; } = new();
    }
}
