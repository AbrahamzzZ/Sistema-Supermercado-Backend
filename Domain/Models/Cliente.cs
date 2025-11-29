using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models;

public partial class Cliente
{
    public int Id_Cliente { get; set; }

    public string? Codigo { get; set; }

    public string? Nombres { get; set; }

    public string? Apellidos { get; set; }

    public string? Cedula { get; set; }

    public string? Telefono { get; set; }

    public string? Correo_Electronico { get; set; }

    public DateTime? Fecha_Registro { get; set; }

    public virtual ICollection<Ventum> Venta { get; set; } = new List<Ventum>();

    [NotMapped]
    public int TotalCount { get; set; }
}
