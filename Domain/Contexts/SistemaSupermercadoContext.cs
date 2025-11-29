using Domain.Models;
using Domain.Models.Dto;
using Domain.Models.Dto.Compra;
using Domain.Models.Dto.Negocio;
using Domain.Models.Dto.Venta;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Domain.Contexts;

public partial class SistemaSupermercadoContext : DbContext
{
    public SistemaSupermercadoContext()
    {
    }

    public SistemaSupermercadoContext(DbContextOptions<SistemaSupermercadoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categorium> Categoria { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Compra> Compras { get; set; }

    public virtual DbSet<DetalleCompra> DetalleCompras { get; set; }

    public virtual DbSet<DetalleVentum> DetalleVenta { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<Negocio> Negocios { get; set; }

    public virtual DbSet<Ofertum> Oferta { get; set; }

    public virtual DbSet<Permiso> Permisos { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Proveedor> Proveedors { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<Sucursal> Sucursals { get; set; }

    public virtual DbSet<Transportistum> Transportista { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Ventum> Venta { get; set; }

    public DbSet<UsuarioRol> UsuariosDto { get; set; }

    public DbSet<ProductoCategoria> ProductosDto { get; set; }

    public DbSet<ProductoRespuesta> ProductoDto { get; set; }

    public DbSet<OfertaProducto> OfertasDto { get; set; }

    public DbSet<CompraRespuesta> CompraDto { get; set; }

    public DbSet<DetalleCompras> DetalleComprasDto { get; set; }

    public DbSet<DetalleComprasRepuesta> DetalleComprasRepuestaDto { get; set; }

    public DbSet<VentaRespuesta> VentaDto { get; set; }

    public DbSet<DetalleVentas> DetalleVentasDto { get; set; }

    public DbSet<DetalleVentasRepuesta> DetalleVentasRepuestaDto { get; set; }

    public DbSet<ProductoMasComprado> ProductoMasComprados { get; set; }

    public DbSet<ProductoMasVendido> ProductoMasVendidos { get; set; }

    public DbSet<TopCliente> TopClientes { get; set; }

    public DbSet<TopProveedor> TopProveedores { get; set; }

    public DbSet<ViajesTransportista> ViajesTransportistas { get; set; }

    public DbSet<EmpleadoProductivo> EmpleadoProductivos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Clases serializadas
        modelBuilder.Entity<UsuarioRol>().HasNoKey().ToView(null);

        modelBuilder.Entity<ProductoCategoria>(entity =>
        {
            entity.HasNoKey()
                  .ToView(null);

            entity.Property(c => c.Precio_Compra)
                  .HasPrecision(18, 2);

            entity.Property(c => c.Precio_Venta)
                  .HasPrecision(18, 2);
        });

        modelBuilder.Entity<ProductoRespuesta>().HasNoKey().ToView(null);

        modelBuilder.Entity<OfertaProducto>().HasNoKey()
            .ToView(null)
            .Property(c => c.Descuento)
            .HasPrecision(18, 2);

        modelBuilder.Entity<CompraRespuesta>().HasNoKey()
            .ToView(null)
            .Property(c => c.Monto_Total)
            .HasPrecision(18, 2);

        modelBuilder.Entity<DetalleCompras>(entity =>
        {
            entity.HasNoKey()
                  .ToView(null);

            entity.Property(e => e.Precio_Compra)
                  .HasPrecision(18, 2);

            entity.Property(e => e.Precio_Venta)
                  .HasPrecision(18, 2);

            entity.Property(e => e.SubTotal)
                  .HasPrecision(18, 2);
        });

        modelBuilder.Entity<DetalleComprasRepuesta>(entity =>
        {
            entity.HasNoKey()
                  .ToView(null);

            entity.Property(e => e.Precio_Compra)
                   .HasPrecision(18, 2);

            entity.Property(e => e.Precio_Venta)
                  .HasPrecision(18, 2);

            entity.Property(e => e.SubTotal)
                  .HasPrecision(18, 2);
        });

        modelBuilder.Entity<VentaRespuesta>(entity =>
        {
            entity.HasNoKey()
                  .ToView(null);

            entity.Property(e => e.Monto_Cambio)
                  .HasPrecision(18, 2);

            entity.Property(e => e.Monto_Pago)
                  .HasPrecision(18, 2);

            entity.Property(e => e.Monto_Total)
                  .HasPrecision(18, 2);

            entity.Property(c => c.Descuento)
                  .HasPrecision(18, 2);
        });

        modelBuilder.Entity<DetalleVentas>(entity =>
        {
            entity.HasNoKey()
            .ToView(null);

            entity.Property(c => c.Descuento)
                  .HasPrecision(18, 2);

            entity.Property(e => e.Precio_Venta)
                  .HasPrecision(18, 2);

            entity.Property(e => e.SubTotal)
                  .HasPrecision(18, 2);

        });

        modelBuilder.Entity<DetalleVentasRepuesta>(entity =>
        {
            entity.HasNoKey()
                  .ToView(null);

            entity.Property(e => e.Precio_Venta)
                  .HasPrecision(18, 2);

            entity.Property(e => e.SubTotal)
                  .HasPrecision(18, 2);

            entity.Property(e => e.Descuento)
                  .HasPrecision(18, 2);
        });

        modelBuilder.Entity<ProductoMasComprado>().HasNoKey().ToView(null);

        modelBuilder.Entity<ProductoMasVendido>().HasNoKey().ToView(null);

        modelBuilder.Entity<TopProveedor>().HasNoKey().ToView(null);

        modelBuilder.Entity<TopCliente>().HasNoKey().ToView(null);

        modelBuilder.Entity<ViajesTransportista>().HasNoKey().ToView(null);

        modelBuilder.Entity<EmpleadoProductivo>().HasNoKey().ToView(null);

        modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
