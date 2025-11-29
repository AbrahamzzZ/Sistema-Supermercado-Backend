namespace Domain.Models.Dto
{
    public class ProductoRespuesta
    {
        public int Id_Producto { get; set; }
        public string? Codigo { get; set; }
        public string? Descripcion { get; set; }
        public string? Nombre_Producto { get; set; }
        public int? Id_Categoria { get; set; }
        public string? Nombre_Categoria { get; set; }
        public string? Pais_Origen { get; set; }
        public bool? Estado { get; set; }
    }
}
