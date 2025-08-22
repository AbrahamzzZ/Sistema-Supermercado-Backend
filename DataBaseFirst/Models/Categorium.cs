using System.ComponentModel.DataAnnotations.Schema;

namespace DataBaseFirst.Models;

public partial class Categorium
{
    public int Id_Categoria { get; set; }

    public string? Codigo { get; set; }

    public string? Nombre_Categoria { get; set; }

    public bool? Estado { get; set; }

    public DateTime? Fecha_Creacion { get; set; }

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();

    [NotMapped]
    public int TotalCount { get; set; }
}
