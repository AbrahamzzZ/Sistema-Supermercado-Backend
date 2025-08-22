namespace DataBaseFirst.Models;

public partial class Usuario
{
    public int Id_Usuario { get; set; }

    public string? Codigo { get; set; }

    public string? Nombre_Completo { get; set; }

    public string? Correo_Electronico { get; set; }

    public string? Clave { get; set; }

    public int? Id_Rol { get; set; }

    public bool? Estado { get; set; }

    public DateTime? Fecha_Creacion { get; set; }

    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();

    public virtual Rol? IdRolNavigation { get; set; }

    public virtual ICollection<Ventum> Venta { get; set; } = new List<Ventum>();
}
