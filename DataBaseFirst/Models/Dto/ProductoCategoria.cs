using System.ComponentModel.DataAnnotations.Schema;

namespace DataBaseFirst.Models.Dto
{
    public class ProductoCategoria
    {
        public int Id_Producto { get; set; }
        public string? Codigo { get; set; }
        public string? Descripcion { get; set; }
        public string? Nombre_Producto { get; set; }
        public int? Id_Categoria { get; set; }
        public string? Nombre_Categoria { get; set; }
        public string? Pais_Origen { get; set; }
        public int Stock { get; set; }
        public decimal? Precio_Compra { get; set; }
        public decimal? Precio_Venta { get; set; }
        public bool? Estado { get; set; }
        [NotMapped]
        public int TotalCount { get; set; }
    }
}
