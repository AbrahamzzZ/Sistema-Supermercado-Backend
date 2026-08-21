namespace Domain.Models.Dto.Response.Categoria
{
    public class CategoriaResponse
    {
        public int Id_actegoria { get; set; }
        public string? Codigo { get; set; }
        public string? Nombre_categoria { get; set; }
        public bool? Estado { get; set; }
        public DateTime? Fecha_Creacion { get; set; }
    }
}
