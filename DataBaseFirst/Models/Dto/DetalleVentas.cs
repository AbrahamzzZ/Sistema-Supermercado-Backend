namespace DataBaseFirst.Models.Dto
{
    public class DetalleVentas
    {
        public int Id_Producto { get; set; }
        public decimal Precio_Venta { get; set; }
        public int Cantidad { get; set; }
        public decimal Descuento { get; set; }
        public decimal SubTotal { get; set; }
    }
}
