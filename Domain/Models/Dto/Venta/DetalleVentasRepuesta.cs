namespace Domain.Models.Dto.Venta
{
    public class DetalleVentasRepuesta
    {
        public int Id_Producto { get; set; }
        public string? Productos { get; set; }
        public decimal Precio_Venta { get; set; }
        public int Cantidad { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Descuento { get; set; }
    }
}
