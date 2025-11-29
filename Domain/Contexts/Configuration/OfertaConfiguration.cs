using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Contexts.Configuration
{
    public class OfertaConfiguration : IEntityTypeConfiguration<Ofertum>
    {
        public void Configure(EntityTypeBuilder<Ofertum> builder)
        {
            builder.HasKey(e => e.Id_Oferta).HasName("PK__OFERTA__D1CAC4475441B3C5");

            builder.ToTable("OFERTA");

            builder.Property(e => e.Id_Oferta).HasColumnName("ID_OFERTA");
            builder.Property(e => e.Codigo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CODIGO");
            builder.Property(e => e.Descripcion)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("DESCRIPCION");
            builder.Property(e => e.Descuento)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("DESCUENTO");
            builder.Property(e => e.Estado).HasColumnName("ESTADO");
            builder.Property(e => e.Fecha_Creacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("FECHA_CREACION");
            builder.Property(e => e.Fecha_Fin).HasColumnName("FECHA_FIN");
            builder.Property(e => e.Fecha_Inicio).HasColumnName("FECHA_INICIO");
            builder.Property(e => e.Id_Producto).HasColumnName("ID_PRODUCTO");
            builder.Property(e => e.Nombre_Oferta)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NOMBRE_OFERTA");

            builder.HasOne(d => d.IdProductoNavigation).WithMany(p => p.Oferta)
                .HasForeignKey(d => d.Id_Producto)
                .HasConstraintName("FK__OFERTA__ID_PRODU__0A9D95DB");
        }
    }
}
