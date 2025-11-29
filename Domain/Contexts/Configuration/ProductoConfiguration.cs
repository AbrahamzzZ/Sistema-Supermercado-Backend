using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Contexts.Configuration
{
    public class ProductoConfiguration : IEntityTypeConfiguration<Producto>
    {
        public void Configure(EntityTypeBuilder<Producto> builder)
        {
            builder.HasKey(e => e.Id_Producto).HasName("PK__PRODUCTO__88BD0357A802EF59");

            builder.ToTable("PRODUCTO");

            builder.Property(e => e.Id_Producto).HasColumnName("ID_PRODUCTO");
            builder.Property(e => e.Codigo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CODIGO");
            builder.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .HasColumnName("DESCRIPCION");
            builder.Property(e => e.Estado).HasColumnName("ESTADO");
            builder.Property(e => e.Id_Categoria).HasColumnName("ID_CATEGORIA");
            builder.Property(e => e.Nombre_Producto)
                .HasMaxLength(30)
                .HasColumnName("NOMBRE_PRODUCTO");
            builder.Property(e => e.Pais_Origen)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("PAIS_ORIGEN");
            builder.Property(e => e.Precio_Compra)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("PRECIO_COMPRA");
            builder.Property(e => e.Precio_Venta)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("PRECIO_VENTA");
            builder.Property(e => e.Stock).HasColumnName("STOCK");

            builder.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.Id_Categoria)
                .HasConstraintName("FK__PRODUCTO__ID_CAT__7F2BE32F");
        }
    }
}
