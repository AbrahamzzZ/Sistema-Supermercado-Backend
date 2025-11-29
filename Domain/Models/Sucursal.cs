using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models;

public partial class Sucursal
{
    public int Id_Sucursal { get; set; }

    public string? Codigo { get; set; }

    public int? Id_Negocio { get; set; }

    public string? Nombre_Sucursal { get; set; }

    public string? Direccion_Sucursal { get; set; }

    public double? Latitud { get; set; }

    public double? Longitud { get; set; }

    public string? Ciudad_Sucursal { get; set; }

    public bool? Estado { get; set; }

    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();

    public virtual Negocio? IdNegocioNavigation { get; set; }

    public virtual ICollection<Ventum> Venta { get; set; } = new List<Ventum>();

    [NotMapped]
    public int TotalCount { get; set; }
}
