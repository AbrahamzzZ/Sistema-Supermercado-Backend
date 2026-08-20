namespace Domain.Models
{
    public class HistorialPrecioProducto
    {
        public int Id_Historial { get; set; }
        public int Id_Producto { get; set; }
        public decimal? Precio_Compra_Anterior { get; set; }
        public decimal? Precio_Compra_Nuevo { get; set; }
        public decimal? Precio_Venta_Anterior { get; set; }
        public decimal? Precio_Venta_Nuevo { get; set; }
        public string? Usuario_Modificacion { get; set; }
        public DateTime Fecha_Cambio { get; set; }
        public virtual Producto IdProductonavitaion { get; set; } = null!;
    }
}
