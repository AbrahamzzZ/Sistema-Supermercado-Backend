namespace Domain.Models.Dto.Negocio.IA
{
    public class ProductoMasCompradoAnalisisIA
    {
        public string? Nombre_Producto { get; set; }
        public int Cantidad_Comprada { get; set; }
        public DateOnly? Fecha { get; set; }
    }
}
