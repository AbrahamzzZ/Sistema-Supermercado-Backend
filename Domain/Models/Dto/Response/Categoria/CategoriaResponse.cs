namespace Domain.Models.Dto.Response.Categoria
{
    public class CategoriaResponse
    {
        public int Id_Categoria { get; set; }
        public string? Codigo { get; set; }
        public string? Nombre_Categoria { get; set; }
        public bool? Estado { get; set; }
        public DateTime? Fecha_Creacion { get; set; }
    }
}
