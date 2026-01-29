using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Contexts.Configuration
{
    public class DetalleVentaConfiguration : IEntityTypeConfiguration<DetalleVentum>
    {
        public void Configure(EntityTypeBuilder<DetalleVentum> builder)
        {
            builder.HasKey(e => e.IdDetalleVenta).HasName("PK__DETALLE___49DABA0CBFC2BF98");

            builder.ToTable("DETALLE_VENTA");

            builder.Property(e => e.IdDetalleVenta).HasColumnName("ID_DETALLE_VENTA");
            builder.Property(e => e.Cantidad).HasColumnName("CANTIDAD");
            builder.Property(e => e.Descuento)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("DESCUENTO");
            builder.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("FECHA_REGISTRO");
            builder.Property(e => e.IdProducto).HasColumnName("ID_PRODUCTO");
            builder.Property(e => e.IdVenta).HasColumnName("ID_VENTA");
            builder.Property(e => e.PrecioVenta)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("PRECIO_VENTA");
            builder.Property(e => e.Subtotal)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("SUBTOTAL");

            builder.HasOne(d => d.IdProductoNavigation).WithMany(p => p.DetalleVenta)
                .HasForeignKey(d => d.IdProducto)
                .HasConstraintName("FK__DETALLE_V__ID_PR__2EDAF651");

            builder.HasOne(d => d.IdVentaNavigation).WithMany(p => p.DetalleVenta)
                .HasForeignKey(d => d.IdVenta)
                .HasConstraintName("FK__DETALLE_V__ID_VE__2DE6D218");
        }
    }
}
