namespace Domain.Models;

public partial class Menu
{
    public int IdMenu { get; set; }

    public string? NombreMenu { get; set; }

    public string? UrlMenu { get; set; }

    public string? NombreIcono { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public virtual ICollection<Permiso> Permisos { get; set; } = new List<Permiso>();
}
