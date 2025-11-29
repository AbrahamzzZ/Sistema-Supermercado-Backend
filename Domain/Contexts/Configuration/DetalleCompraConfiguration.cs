using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Contexts.Configuration
{
    public class DetalleCompraConfiguration : IEntityTypeConfiguration<DetalleCompra>
    {
        public void Configure(EntityTypeBuilder<DetalleCompra> builder)
        {
            builder.HasKey(e => e.IdDetalleCompra).HasName("PK__DETALLE___2E6E01ED3B4D3BB9");

            builder.ToTable("DETALLE_COMPRA");

            builder.Property(e => e.IdDetalleCompra).HasColumnName("ID_DETALLE_COMPRA");
            builder.Property(e => e.Cantidad).HasColumnName("CANTIDAD");
            builder.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("FECHA_REGISTRO");
            builder.Property(e => e.IdCompra).HasColumnName("ID_COMPRA");
            builder.Property(e => e.IdProducto).HasColumnName("ID_PRODUCTO");
            builder.Property(e => e.PrecioCompra)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("PRECIO_COMPRA");
            builder.Property(e => e.PrecioVenta)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("PRECIO_VENTA");
            builder.Property(e => e.Subtotal)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("SUBTOTAL");

            builder.HasOne(d => d.IdCompraNavigation).WithMany(p => p.DetalleCompras)
                .HasForeignKey(d => d.IdCompra)
                .HasConstraintName("FK__DETALLE_C__ID_CO__1CBC4616");

            builder.HasOne(d => d.IdProductoNavigation).WithMany(p => p.DetalleCompras)
                .HasForeignKey(d => d.IdProducto)
                .HasConstraintName("FK__DETALLE_C__ID_PR__1DB06A4F");
        }
    }
}
