namespace Domain.Models.Dto.Response.Venta
{
    public class VentaRegistradaResponse
    {
        public string Numero_Documento { get; set; } = string.Empty;
        public decimal Monto_Total { get; set; }
        public decimal Descuento { get; set; }
        public decimal Monto_Pago { get; set; }
        public decimal Monto_Cambio { get; set; }
    }
}
