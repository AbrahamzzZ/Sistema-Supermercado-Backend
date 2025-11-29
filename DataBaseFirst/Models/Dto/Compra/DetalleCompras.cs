namespace DataBaseFirst.Models.Dto.Compra
{
    public class DetalleCompras
    {
        public int Id_Producto { get; set; }
        public decimal Precio_Compra { get; set; }
        public decimal Precio_Venta { get; set; }
        public int Cantidad { get; set; }
        public decimal SubTotal {get; set; }
    }
}
