using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Contexts.Configuration
{
    public class CategoriaConfiguration : IEntityTypeConfiguration<Categorium>
    {
        public void Configure(EntityTypeBuilder<Categorium> builder)
        {
            builder.HasKey(e => e.Id_Categoria).HasName("PK__CATEGORI__4BD51FA5A0E6919F");

            builder.ToTable("CATEGORIA");

            builder.Property(e => e.Id_Categoria).HasColumnName("ID_CATEGORIA");
            builder.Property(e => e.Codigo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CODIGO");
            builder.Property(e => e.Estado).HasColumnName("ESTADO");
            builder.Property(e => e.Fecha_Creacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("FECHA_CREACION");
            builder.Property(e => e.Nombre_Categoria)
                .HasMaxLength(50)
                .HasColumnName("NOMBRE_CATEGORIA");
        }
    }
}
