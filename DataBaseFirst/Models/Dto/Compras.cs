using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseFirst.Models.Dto
{
    public class Compras
    {
        public int Id_Usuario { get; set; }
        public int Id_Sucursal { get; set; }
        public int Id_Proveedor { get; set; }
        public int Id_Transportista { get; set; }
        public string Tipo_Documento { get; set; } = string.Empty;
        public string Numero_Documento { get; set; } = string.Empty;
        public decimal Monto_Total { get; set; }
        public List<DetalleCompras>? Detalles { get; set; } = new();
    }
}
