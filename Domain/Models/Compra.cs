namespace Domain.Models;

public partial class Compra
{
    public int IdCompra { get; set; }

    public int? IdUsuario { get; set; }

    public int? IdSucursal { get; set; }

    public int? IdProveedor { get; set; }

    public int? IdTransportista { get; set; }

    public string? TipoDocumento { get; set; }

    public string? NumeroDocumento { get; set; }

    public decimal? MontoTotal { get; set; }

    public DateTime? FechaCompra { get; set; }

    public virtual ICollection<DetalleCompra> DetalleCompras { get; set; } = new List<DetalleCompra>();

    public virtual Proveedor? IdProveedorNavigation { get; set; }

    public virtual Sucursal? IdSucursalNavigation { get; set; }

    public virtual Transportistum? IdTransportistaNavigation { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
