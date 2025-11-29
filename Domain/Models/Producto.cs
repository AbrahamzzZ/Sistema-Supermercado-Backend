namespace Domain.Models;

public partial class Producto
{
    public int Id_Producto { get; set; }

    public string? Codigo { get; set; }

    public string? Descripcion { get; set; }

    public string? Nombre_Producto { get; set; }

    public int? Id_Categoria { get; set; }

    public string? Pais_Origen { get; set; }

    public int Stock { get; set; }

    public decimal? Precio_Compra { get; set; }

    public decimal? Precio_Venta { get; set; }

    public bool? Estado { get; set; }

    public virtual ICollection<DetalleCompra> DetalleCompras { get; set; } = new List<DetalleCompra>();

    public virtual ICollection<DetalleVentum> DetalleVenta { get; set; } = new List<DetalleVentum>();

    public virtual Categorium? IdCategoriaNavigation { get; set; }

    public virtual ICollection<Ofertum> Oferta { get; set; } = new List<Ofertum>();
}
