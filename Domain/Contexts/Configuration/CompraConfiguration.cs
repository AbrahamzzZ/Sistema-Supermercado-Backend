using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Contexts.Configuration
{
    public class CompraConfiguration : IEntityTypeConfiguration<Compra>
    {
        public void Configure(EntityTypeBuilder<Compra> builder)
        {
            builder.HasKey(e => e.IdCompra).HasName("PK__COMPRA__16C0FA95D282DC28");

            builder.ToTable("COMPRA");

            builder.HasIndex(e => e.NumeroDocumento, "UQ__COMPRA__87B6EC7EAF707742").IsUnique();

            builder.Property(e => e.IdCompra).HasColumnName("ID_COMPRA");
            builder.Property(e => e.FechaCompra)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("FECHA_COMPRA");
            builder.Property(e => e.IdProveedor).HasColumnName("ID_PROVEEDOR");
            builder.Property(e => e.IdSucursal).HasColumnName("ID_SUCURSAL");
            builder.Property(e => e.IdTransportista).HasColumnName("ID_TRANSPORTISTA");
            builder.Property(e => e.IdUsuario).HasColumnName("ID_USUARIO");
            builder.Property(e => e.MontoTotal)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("MONTO_TOTAL");
            builder.Property(e => e.NumeroDocumento)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NUMERO_DOCUMENTO");
            builder.Property(e => e.TipoDocumento)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TIPO_DOCUMENTO");

            builder.HasOne(d => d.IdProveedorNavigation).WithMany(p => p.Compras)
                .HasForeignKey(d => d.IdProveedor)
                .HasConstraintName("FK__COMPRA__ID_PROVE__17F790F9");

            builder.HasOne(d => d.IdSucursalNavigation).WithMany(p => p.Compras)
                .HasForeignKey(d => d.IdSucursal)
                .HasConstraintName("FK__COMPRA__ID_SUCUR__17036CC0");

            builder.HasOne(d => d.IdTransportistaNavigation).WithMany(p => p.Compras)
                .HasForeignKey(d => d.IdTransportista)
                .HasConstraintName("FK__COMPRA__ID_TRANS__18EBB532");

            builder.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Compras)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__COMPRA__ID_USUAR__160F4887");
        }
    }
}
