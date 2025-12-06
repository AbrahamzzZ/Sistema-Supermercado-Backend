namespace Domain.Models.Dto.Negocio.IA
{
    public class ProductoMasVendidoAnalisisIA
    {
        public string? Nombre_Producto { get; set; }
        public int Cantidad_Vendida { get; set; }
        public DateOnly? Fecha { get; set; }
    }
}
