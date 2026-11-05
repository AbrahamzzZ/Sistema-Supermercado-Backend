namespace Domain.Models.Dto.Response.Venta
{
    public class Ventas
    {
        public int Id_Sucursal { get; set; }
        public int Id_Cliente { get; set; }
        public string Tipo_Documento { get; set; } = string.Empty;
        public decimal Monto_Pago { get; set; }
        public List<DetalleVentas>? Detalles { get; set; } = new();
    }
}
