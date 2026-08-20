using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Contexts.Configuration
{
    public class HistorialPrecioProductoConfiguration : IEntityTypeConfiguration<HistorialPrecioProducto>
    {
        public void Configure(EntityTypeBuilder<HistorialPrecioProducto> builder)
        {
            builder.HasKey(e => e.Id_Historial);
            builder.ToTable("HISTORIAL_PRECIO_PRODUCTO");

            builder.Property(e => e.Id_Historial)
                .HasColumnName("ID_HISTORIAL");
            builder.Property(e => e.Id_Producto)
                .HasColumnName("ID_PRODUCTO");
            builder.Property(e => e.Precio_Compra_Anterior)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("PRECIO_COMPRA_ANTERIOR");
            builder.Property(e => e.Precio_Compra_Nuevo)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("PRECIO_COMPRA_NUEVO");
            builder.Property(e => e.Precio_Venta_Anterior)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("PRECIO_VENTA_ANTERIOR");
            builder.Property(e => e.Precio_Venta_Nuevo)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("PRECIO_VENTA_NUEVO");
            builder.Property(e => e.Usuario_Modificacion)
                .HasMaxLength(100).IsUnicode(false)
                .HasColumnName("USUARIO_MODIFICACION");
            builder.Property(e => e.Fecha_Cambio)
                .HasColumnName("FECHA_CAMBIO");
            builder.HasOne(d => d.IdProductonavitaion)
                .WithMany(p => p.HistorialPrecios)
                .HasForeignKey(d => d.Id_Producto)
                .HasConstraintName("FK_HISTORIAL_PRECIO_PRODUCTO");
        }
    }
}
