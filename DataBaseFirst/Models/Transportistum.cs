using System.ComponentModel.DataAnnotations.Schema;

namespace DataBaseFirst.Models;

public partial class Transportistum
{
    public int Id_Transportista { get; set; }

    public string? Codigo { get; set; }

    public string? Nombres { get; set; }

    public string? Apellidos { get; set; }

    public string? Cedula { get; set; }

    public string? Telefono { get; set; }

    public string? Correo_Electronico { get; set; }

    [NotMapped]
    public string? ImagenBase64 { get; set; }

    public byte[]? Imagen { get; set; }

    public bool? Estado { get; set; }

    public DateTime? Fecha_Registro { get; set; }

    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();

    [NotMapped]
    public int TotalCount { get; set; }
}
