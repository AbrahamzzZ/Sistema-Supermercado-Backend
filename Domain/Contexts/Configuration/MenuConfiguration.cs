using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Contexts.Configuration
{
    public class MenuConfiguration : IEntityTypeConfiguration<Menu>
    {
        public void Configure(EntityTypeBuilder<Menu> builder)
        {
            builder.HasKey(e => e.IdMenu).HasName("PK__MENU__4728FC60709407A7");

            builder.ToTable("MENU");

            builder.Property(e => e.IdMenu).HasColumnName("ID_MENU");
            builder.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("FECHA_CREACION");
            builder.Property(e => e.NombreIcono)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NOMBRE_ICONO");
            builder.Property(e => e.NombreMenu)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NOMBRE_MENU");
            builder.Property(e => e.UrlMenu)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("URL_MENU");
        }
    }
}
