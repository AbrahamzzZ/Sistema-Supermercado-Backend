using Domain.Models;
using Domain.Models.Dto.Response.Compra;
using Domain.Models.Dto.Response.Negocio;
using Domain.Models.Dto.Response.Negocio.IA;
using Domain.Models.Dto.Response.Oferta;
using Domain.Models.Dto.Response.Producto;
using Domain.Models.Dto.Response.Usuario;
using Domain.Models.Dto.Response.Venta;
using Domain.Models.Dto.Response.Categoria;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Domain.Models.Dto.Response.Cliente;
using Domain.Models.Dto.Response.Provedor;
using Domain.Models.Dto.Response.Transportista;
using Domain.Models.Dto.Response.Sucursal;

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

    public DbSet<UsuarioRolResponse> UsuariosDto { get; set; }

    public DbSet<ProductoCategoriaResponse> ProductosDto { get; set; }

    public DbSet<ProductoResponse> ProductoDto { get; set; }

    public DbSet<OfertaProductoResponse> OfertasDto { get; set; }

    public DbSet<CompraResponse> CompraDto { get; set; }

    public DbSet<DetalleCompras> DetalleComprasDto { get; set; }

    public DbSet<DetalleCompraReponse> DetalleComprasRepuestaDto { get; set; }

    public DbSet<VentaResponse> VentaDto { get; set; }

    public DbSet<DetalleVentas> DetalleVentasDto { get; set; }

    public DbSet<DetalleVentaReponse> DetalleVentasRepuestaDto { get; set; }

    public virtual DbSet<HistorialPrecioProducto> HistorialPreciosProducto { get; set; }

    public DbSet<ProductoMasCompradoResponse> ProductoMasComprados { get; set; }

    public DbSet<ProductoMasCompradoAnalisisIA> ProductoMasCompradosAnalisis { get; set; }

    public DbSet<ProductoMasVendidoResponse> ProductoMasVendidos { get; set; }

    public DbSet<ProductoMasVendidoAnalisisIA> ProductoMasVendidosAnalisis { get; set; }

    public DbSet<TopClienteResponse> TopClientes { get; set; }

    public DbSet<TopProveedorResponse> TopProveedores { get; set; }

    public DbSet<ViajesTransportistaResponse> ViajesTransportistas { get; set; }

    public DbSet<EmpleadoProductivoResponse> EmpleadoProductivos { get; set; }

    public DbSet<CategoriaResponse> CategoriasDto { get; set; }

    public DbSet<ClienteResponse> ClientesDto { get; set; }

    public DbSet<ProveedorResponse> ProveedorDto { get; set; }

    public DbSet<TransportistaResponse> TransportistasDto { get; set; }

    public DbSet<SucursalResponse> SucursalesDto { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Clases serializadas
        modelBuilder.Entity<UsuarioRolResponse>().HasNoKey().ToView(null);

        modelBuilder.Entity<ProductoCategoriaResponse>(entity =>
        {
            entity.HasNoKey()
                  .ToView(null);

            entity.Property(c => c.Precio_Compra)
                  .HasPrecision(18, 2);

            entity.Property(c => c.Precio_Venta)
                  .HasPrecision(18, 2);
        });

        modelBuilder.Entity<ProductoResponse>().HasNoKey().ToView(null);

        modelBuilder.Entity<OfertaProductoResponse>().HasNoKey()
            .ToView(null)
            .Property(c => c.Descuento)
            .HasPrecision(18, 2);

        modelBuilder.Entity<CompraResponse>().HasNoKey()
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

        modelBuilder.Entity<DetalleCompraReponse>(entity =>
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

        modelBuilder.Entity<VentaResponse>(entity =>
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

        modelBuilder.Entity<DetalleVentaReponse>(entity =>
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

        modelBuilder.Entity<CategoriaResponse>(entity =>
        {
            entity.HasNoKey()
                .ToView(null);
        });

        modelBuilder.Entity<ClienteResponse>(entity =>
        {
            entity.HasNoKey()
                .ToView(null);
        });

        modelBuilder.Entity<ProveedorResponse>(entity =>
        {
            entity.HasNoKey()
                .ToView(null);
        });

        modelBuilder.Entity<TransportistaResponse>(entity =>
        {
            entity.HasNoKey()
                .ToView(null);
        });

        modelBuilder.Entity<SucursalResponse>(entity =>
        {
            entity.HasNoKey()
                .ToView(null);
        });

        modelBuilder.Entity<ProductoMasCompradoResponse>().HasNoKey().ToView(null);

        modelBuilder.Entity<ProductoMasCompradoAnalisisIA>().HasNoKey().ToView(null);

        modelBuilder.Entity<ProductoMasVendidoResponse>().HasNoKey().ToView(null);

        modelBuilder.Entity<ProductoMasVendidoAnalisisIA>().HasNoKey().ToView(null);

        modelBuilder.Entity<TopProveedorResponse>().HasNoKey().ToView(null);

        modelBuilder.Entity<TopClienteResponse>().HasNoKey().ToView(null);

        modelBuilder.Entity<ViajesTransportistaResponse>().HasNoKey().ToView(null);

        modelBuilder.Entity<EmpleadoProductivoResponse>().HasNoKey().ToView(null);

        modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
