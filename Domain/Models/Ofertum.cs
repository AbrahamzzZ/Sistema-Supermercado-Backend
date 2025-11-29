namespace Domain.Models;

public partial class Ofertum
{
    public int Id_Oferta { get; set; }

    public string? Codigo { get; set; }

    public string? Nombre_Oferta { get; set; }

    public int? Id_Producto { get; set; }

    public string? Descripcion { get; set; }

    public DateOnly? Fecha_Inicio { get; set; }

    public DateOnly? Fecha_Fin { get; set; }

    public decimal? Descuento { get; set; }

    public bool? Estado { get; set; }

    public DateTime? Fecha_Creacion { get; set; }

    public virtual Producto? IdProductoNavigation { get; set; }
}
