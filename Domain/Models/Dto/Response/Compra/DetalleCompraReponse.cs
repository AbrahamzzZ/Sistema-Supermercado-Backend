namespace Domain.Models.Dto.Response.Compra
{
    public class DetalleCompraReponse
    {
        public int Id_Producto { get; set; }
        public string? Productos { get; set; }
        public decimal Precio_Compra { get; set; }
        public decimal Precio_Venta { get; set; }
        public int Cantidad { get; set; }
        public decimal SubTotal { get; set; }
    }
}
