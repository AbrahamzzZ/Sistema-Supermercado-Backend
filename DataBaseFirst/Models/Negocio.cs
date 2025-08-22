using System.ComponentModel.DataAnnotations.Schema;

namespace DataBaseFirst.Models;

public partial class Negocio
{
    public int Id_Negocio { get; set; }

    public string? Nombre { get; set; }

    public string? Telefono { get; set; }

    public string? Ruc { get; set; }

    public string? Direccion { get; set; }

    public string? Correo_Electronico { get; set; }

    [NotMapped]
    public string? ImagenBase64 { get; set; }

    public byte[]? Logo { get; set; }

    public virtual ICollection<Sucursal> Sucursals { get; set; } = new List<Sucursal>();
}
