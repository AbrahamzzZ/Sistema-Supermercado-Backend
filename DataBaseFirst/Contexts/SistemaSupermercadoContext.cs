using DataBaseFirst.Models;
using DataBaseFirst.Models.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using WebApiRest.Dto;

namespace DataBaseFirst.Contexts;

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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost\\SQLSEXPRESS;Database=Sistema_Supermercado;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categorium>(entity =>
        {
            entity.HasKey(e => e.Id_Categoria).HasName("PK__CATEGORI__4BD51FA5A0E6919F");

            entity.ToTable("CATEGORIA");

            entity.Property(e => e.Id_Categoria).HasColumnName("ID_CATEGORIA");
            entity.Property(e => e.Codigo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CODIGO");
            entity.Property(e => e.Estado).HasColumnName("ESTADO");
            entity.Property(e => e.Fecha_Creacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("FECHA_CREACION");
            entity.Property(e => e.Nombre_Categoria)
                .HasMaxLength(50)
                .HasColumnName("NOMBRE_CATEGORIA");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.Id_Cliente).HasName("PK__CLIENTE__23A34130DBF5B48A");

            entity.ToTable("CLIENTE");

            entity.Property(e => e.Id_Cliente).HasColumnName("ID_CLIENTE");
            entity.Property(e => e.Apellidos)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("APELLIDOS");
            entity.Property(e => e.Cedula)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CEDULA");
            entity.Property(e => e.Codigo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CODIGO");
            entity.Property(e => e.Correo_Electronico)
                .HasMaxLength(50)
                .HasColumnName("CORREO_ELECTRONICO");
            entity.Property(e => e.Fecha_Registro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("FECHA_REGISTRO");
            entity.Property(e => e.Nombres)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("NOMBRES");
            entity.Property(e => e.Telefono)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("TELEFONO");
        });

        modelBuilder.Entity<Compra>(entity =>
        {
            entity.HasKey(e => e.IdCompra).HasName("PK__COMPRA__16C0FA95D282DC28");

            entity.ToTable("COMPRA");

            entity.HasIndex(e => e.NumeroDocumento, "UQ__COMPRA__87B6EC7EAF707742").IsUnique();

            entity.Property(e => e.IdCompra).HasColumnName("ID_COMPRA");
            entity.Property(e => e.FechaCompra)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("FECHA_COMPRA");
            entity.Property(e => e.IdProveedor).HasColumnName("ID_PROVEEDOR");
            entity.Property(e => e.IdSucursal).HasColumnName("ID_SUCURSAL");
            entity.Property(e => e.IdTransportista).HasColumnName("ID_TRANSPORTISTA");
            entity.Property(e => e.IdUsuario).HasColumnName("ID_USUARIO");
            entity.Property(e => e.MontoTotal)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("MONTO_TOTAL");
            entity.Property(e => e.NumeroDocumento)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NUMERO_DOCUMENTO");
            entity.Property(e => e.TipoDocumento)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TIPO_DOCUMENTO");

            entity.HasOne(d => d.IdProveedorNavigation).WithMany(p => p.Compras)
                .HasForeignKey(d => d.IdProveedor)
                .HasConstraintName("FK__COMPRA__ID_PROVE__17F790F9");

            entity.HasOne(d => d.IdSucursalNavigation).WithMany(p => p.Compras)
                .HasForeignKey(d => d.IdSucursal)
                .HasConstraintName("FK__COMPRA__ID_SUCUR__17036CC0");

            entity.HasOne(d => d.IdTransportistaNavigation).WithMany(p => p.Compras)
                .HasForeignKey(d => d.IdTransportista)
                .HasConstraintName("FK__COMPRA__ID_TRANS__18EBB532");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Compras)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__COMPRA__ID_USUAR__160F4887");
        });

        modelBuilder.Entity<DetalleCompra>(entity =>
        {
            entity.HasKey(e => e.IdDetalleCompra).HasName("PK__DETALLE___2E6E01ED3B4D3BB9");

            entity.ToTable("DETALLE_COMPRA");

            entity.Property(e => e.IdDetalleCompra).HasColumnName("ID_DETALLE_COMPRA");
            entity.Property(e => e.Cantidad).HasColumnName("CANTIDAD");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("FECHA_REGISTRO");
            entity.Property(e => e.IdCompra).HasColumnName("ID_COMPRA");
            entity.Property(e => e.IdProducto).HasColumnName("ID_PRODUCTO");
            entity.Property(e => e.PrecioCompra)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("PRECIO_COMPRA");
            entity.Property(e => e.PrecioVenta)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("PRECIO_VENTA");
            entity.Property(e => e.Subtotal)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("SUBTOTAL");

            entity.HasOne(d => d.IdCompraNavigation).WithMany(p => p.DetalleCompras)
                .HasForeignKey(d => d.IdCompra)
                .HasConstraintName("FK__DETALLE_C__ID_CO__1CBC4616");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.DetalleCompras)
                .HasForeignKey(d => d.IdProducto)
                .HasConstraintName("FK__DETALLE_C__ID_PR__1DB06A4F");
        });

        modelBuilder.Entity<DetalleVentum>(entity =>
        {
            entity.HasKey(e => e.IdDetalleVenta).HasName("PK__DETALLE___49DABA0CBFC2BF98");

            entity.ToTable("DETALLE_VENTA");

            entity.Property(e => e.IdDetalleVenta).HasColumnName("ID_DETALLE_VENTA");
            entity.Property(e => e.Cantidad).HasColumnName("CANTIDAD");
            entity.Property(e => e.Descuento)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("DESCUENTO");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("FECHA_REGISTRO");
            entity.Property(e => e.IdProducto).HasColumnName("ID_PRODUCTO");
            entity.Property(e => e.IdVenta).HasColumnName("ID_VENTA");
            entity.Property(e => e.PrecioVenta)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("PRECIO_VENTA");
            entity.Property(e => e.Subtotal)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("SUBTOTAL");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.DetalleVenta)
                .HasForeignKey(d => d.IdProducto)
                .HasConstraintName("FK__DETALLE_V__ID_PR__2EDAF651");

            entity.HasOne(d => d.IdVentaNavigation).WithMany(p => p.DetalleVenta)
                .HasForeignKey(d => d.IdVenta)
                .HasConstraintName("FK__DETALLE_V__ID_VE__2DE6D218");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.IdMenu).HasName("PK__MENU__4728FC60709407A7");

            entity.ToTable("MENU");

            entity.Property(e => e.IdMenu).HasColumnName("ID_MENU");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("FECHA_CREACION");
            entity.Property(e => e.NombreIcono)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NOMBRE_ICONO");
            entity.Property(e => e.NombreMenu)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NOMBRE_MENU");
            entity.Property(e => e.UrlMenu)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("URL_MENU");
        });

        modelBuilder.Entity<Negocio>(entity =>
        {
            entity.HasKey(e => e.Id_Negocio).HasName("PK__NEGOCIO__D81B51AFBEE006A4");

            entity.ToTable("NEGOCIO");

            entity.HasIndex(e => e.Ruc, "UQ__NEGOCIO__CAF3326B49023C6A").IsUnique();

            entity.HasIndex(e => e.Telefono, "UQ__NEGOCIO__D6F1694583FA83D5").IsUnique();

            entity.Property(e => e.Id_Negocio).HasColumnName("ID_NEGOCIO");
            entity.Property(e => e.Correo_Electronico)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("CORREO_ELECTRONICO");
            entity.Property(e => e.Direccion)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("DIRECCION");
            entity.Property(e => e.Logo).HasColumnName("LOGO");
            entity.Property(e => e.Nombre)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");
            entity.Property(e => e.Ruc)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("RUC");
            entity.Property(e => e.Telefono)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("TELEFONO");
        });

        modelBuilder.Entity<Ofertum>(entity =>
        {
            entity.HasKey(e => e.Id_Oferta).HasName("PK__OFERTA__D1CAC4475441B3C5");

            entity.ToTable("OFERTA");

            entity.Property(e => e.Id_Oferta).HasColumnName("ID_OFERTA");
            entity.Property(e => e.Codigo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CODIGO");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("DESCRIPCION");
            entity.Property(e => e.Descuento)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("DESCUENTO");
            entity.Property(e => e.Estado).HasColumnName("ESTADO");
            entity.Property(e => e.Fecha_Creacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("FECHA_CREACION");
            entity.Property(e => e.Fecha_Fin).HasColumnName("FECHA_FIN");
            entity.Property(e => e.Fecha_Inicio).HasColumnName("FECHA_INICIO");
            entity.Property(e => e.Id_Producto).HasColumnName("ID_PRODUCTO");
            entity.Property(e => e.Nombre_Oferta)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NOMBRE_OFERTA");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.Oferta)
                .HasForeignKey(d => d.Id_Producto)
                .HasConstraintName("FK__OFERTA__ID_PRODU__0A9D95DB");
        });

        modelBuilder.Entity<Permiso>(entity =>
        {
            entity.HasKey(e => e.IdPermiso).HasName("PK__PERMISO__AC74EBF6599DBB9A");

            entity.ToTable("PERMISO");

            entity.Property(e => e.IdPermiso).HasColumnName("ID_PERMISO");
            entity.Property(e => e.IdMenu).HasColumnName("ID_MENU");
            entity.Property(e => e.IdRol).HasColumnName("ID_ROL");

            entity.HasOne(d => d.IdMenuNavigation).WithMany(p => p.Permisos)
                .HasForeignKey(d => d.IdMenu)
                .HasConstraintName("FK__PERMISO__ID_MENU__4F7CD00D");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Permisos)
                .HasForeignKey(d => d.IdRol)
                .HasConstraintName("FK__PERMISO__ID_ROL__4E88ABD4");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.Id_Producto).HasName("PK__PRODUCTO__88BD0357A802EF59");

            entity.ToTable("PRODUCTO");

            entity.Property(e => e.Id_Producto).HasColumnName("ID_PRODUCTO");
            entity.Property(e => e.Codigo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CODIGO");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .HasColumnName("DESCRIPCION");
            entity.Property(e => e.Estado).HasColumnName("ESTADO");
            entity.Property(e => e.Id_Categoria).HasColumnName("ID_CATEGORIA");
            entity.Property(e => e.Nombre_Producto)
                .HasMaxLength(30)
                .HasColumnName("NOMBRE_PRODUCTO");
            entity.Property(e => e.Pais_Origen)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("PAIS_ORIGEN");
            entity.Property(e => e.Precio_Compra)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("PRECIO_COMPRA");
            entity.Property(e => e.Precio_Venta)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("PRECIO_VENTA");
            entity.Property(e => e.Stock).HasColumnName("STOCK");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.Id_Categoria)
                .HasConstraintName("FK__PRODUCTO__ID_CAT__7F2BE32F");
        });

        modelBuilder.Entity<Proveedor>(entity =>
        {
            entity.HasKey(e => e.Id_Proveedor).HasName("PK__PROVEEDO__733DB4C4F2A65B2A");

            entity.ToTable("PROVEEDOR");

            entity.Property(e => e.Id_Proveedor).HasColumnName("ID_PROVEEDOR");
            entity.Property(e => e.Apellidos)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("APELLIDOS");
            entity.Property(e => e.Cedula)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CEDULA");
            entity.Property(e => e.Codigo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CODIGO");
            entity.Property(e => e.Correo_Electronico)
                .HasMaxLength(50)
                .HasColumnName("CORREO_ELECTRONICO");
            entity.Property(e => e.Estado).HasColumnName("ESTADO");
            entity.Property(e => e.Fecha_Registro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("FECHA_REGISTRO");
            entity.Property(e => e.Nombres)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("NOMBRES");
            entity.Property(e => e.Telefono)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("TELEFONO");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__ROL__203B0F683CC4AE59");

            entity.ToTable("ROL");

            entity.Property(e => e.IdRol).HasColumnName("ID_ROL");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("FECHA_CREACION");
            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");
        });

        modelBuilder.Entity<Sucursal>(entity =>
        {
            entity.HasKey(e => e.Id_Sucursal).HasName("PK__SUCURSAL__AB3873837FF5CDB2");

            entity.ToTable("SUCURSAL");

            entity.HasIndex(e => e.Codigo, "UQ__SUCURSAL__CC87E126E64D406E").IsUnique();

            entity.Property(e => e.Id_Sucursal).HasColumnName("ID_SUCURSAL");
            entity.Property(e => e.Ciudad_Sucursal)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("CIUDAD_SUCURSAL");
            entity.Property(e => e.Codigo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CODIGO");
            entity.Property(e => e.Direccion_Sucursal)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("DIRECCION_SUCURSAL");
            entity.Property(e => e.Estado).HasColumnName("ESTADO");
            entity.Property(e => e.Id_Negocio).HasColumnName("ID_NEGOCIO");
            entity.Property(e => e.Nombre_Sucursal)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("NOMBRE_SUCURSAL");

            entity.HasOne(d => d.IdNegocioNavigation).WithMany(p => p.Sucursals)
                .HasForeignKey(d => d.Id_Negocio)
                .HasConstraintName("FK__SUCURSAL__ID_NEG__3E52440B");
        });

        modelBuilder.Entity<Transportistum>(entity =>
        {
            entity.HasKey(e => e.Id_Transportista).HasName("PK__TRANSPOR__057F895C84D4FAF7");

            entity.ToTable("TRANSPORTISTA");

            entity.Property(e => e.Id_Transportista).HasColumnName("ID_TRANSPORTISTA");
            entity.Property(e => e.Apellidos)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("APELLIDOS");
            entity.Property(e => e.Cedula)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CEDULA");
            entity.Property(e => e.Codigo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CODIGO");
            entity.Property(e => e.Correo_Electronico)
                .HasMaxLength(50)
                .HasColumnName("CORREO_ELECTRONICO");
            entity.Property(e => e.Estado).HasColumnName("ESTADO");
            entity.Property(e => e.Fecha_Registro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("FECHA_REGISTRO");
            entity.Property(e => e.Imagen).HasColumnName("IMAGEN");
            entity.Property(e => e.Nombres)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("NOMBRES");
            entity.Property(e => e.Telefono)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("TELEFONO");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id_Usuario).HasName("PK__USUARIO__91136B90182039E5");

            entity.ToTable("USUARIO");

            entity.Property(e => e.Id_Usuario).HasColumnName("ID_USUARIO");
            entity.Property(e => e.Clave)
                .HasMaxLength(300)
                .HasColumnName("CLAVE");
            entity.Property(e => e.Codigo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CODIGO");
            entity.Property(e => e.Correo_Electronico)
                .HasMaxLength(50)
                .HasColumnName("CORREO_ELECTRONICO");
            entity.Property(e => e.Estado).HasColumnName("ESTADO");
            entity.Property(e => e.Fecha_Creacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("FECHA_CREACION");
            entity.Property(e => e.Id_Rol).HasColumnName("ID_ROL");
            entity.Property(e => e.Nombre_Completo)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("NOMBRE_COMPLETO");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.Id_Rol)
                .HasConstraintName("FK__USUARIO__ID_ROL__52593CB8");
        });

        modelBuilder.Entity<Ventum>(entity =>
        {
            entity.HasKey(e => e.IdVenta).HasName("PK__VENTA__F3B6C1B480CEC06E");

            entity.ToTable("VENTA");

            entity.HasIndex(e => e.NumeroDocumento, "UQ__VENTA__87B6EC7E76F1FEDA").IsUnique();

            entity.Property(e => e.IdVenta).HasColumnName("ID_VENTA");
            entity.Property(e => e.Descuento)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("DESCUENTO");
            entity.Property(e => e.FechaVenta)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("FECHA_VENTA");
            entity.Property(e => e.IdCliente).HasColumnName("ID_CLIENTE");
            entity.Property(e => e.IdSucursal).HasColumnName("ID_SUCURSAL");
            entity.Property(e => e.IdUsuario).HasColumnName("ID_USUARIO");
            entity.Property(e => e.MontoCambio)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("MONTO_CAMBIO");
            entity.Property(e => e.MontoPago)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("MONTO_PAGO");
            entity.Property(e => e.MontoTotal)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("MONTO_TOTAL");
            entity.Property(e => e.NumeroDocumento)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NUMERO_DOCUMENTO");
            entity.Property(e => e.TipoDocumento)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TIPO_DOCUMENTO");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.IdCliente)
                .HasConstraintName("FK__VENTA__ID_CLIENT__2A164134");

            entity.HasOne(d => d.IdSucursalNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.IdSucursal)
                .HasConstraintName("FK__VENTA__ID_SUCURS__29221CFB");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__VENTA__ID_USUARI__282DF8C2");
        });

        //Clases serializadas
        modelBuilder.Entity<UsuarioRol>().HasNoKey().ToView(null);

        modelBuilder.Entity<ProductoCategoria>().HasNoKey().ToView(null);

        modelBuilder.Entity<ProductoRespuesta>().HasNoKey().ToView(null);

        modelBuilder.Entity<OfertaProducto>().HasNoKey().ToView(null);

        modelBuilder.Entity<CompraRespuesta>().HasNoKey().ToView(null);

        modelBuilder.Entity<DetalleCompras>().HasNoKey().ToView(null);

        modelBuilder.Entity<DetalleComprasRepuesta>().HasNoKey().ToView(null);

        modelBuilder.Entity<VentaRespuesta>().HasNoKey().ToView(null);

        modelBuilder.Entity<DetalleVentas>().HasNoKey().ToView(null);

        modelBuilder.Entity<DetalleVentasRepuesta>().HasNoKey().ToView(null);

        modelBuilder.Entity<ProductoMasComprado>().HasNoKey().ToView(null);

        modelBuilder.Entity<ProductoMasVendido>().HasNoKey().ToView(null);

        modelBuilder.Entity<TopProveedor>().HasNoKey().ToView(null);

        modelBuilder.Entity<TopCliente>().HasNoKey().ToView(null);

        modelBuilder.Entity<ViajesTransportista>().HasNoKey().ToView(null);

        modelBuilder.Entity<EmpleadoProductivo>().HasNoKey().ToView(null);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
