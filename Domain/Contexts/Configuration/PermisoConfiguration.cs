using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Contexts.Configuration
{
    public class PermisoConfiguration : IEntityTypeConfiguration<Permiso>
    {
        public void Configure(EntityTypeBuilder<Permiso> builder)
        {
            builder.HasKey(e => e.IdPermiso).HasName("PK__PERMISO__AC74EBF6599DBB9A");

            builder.ToTable("PERMISO");

            builder.Property(e => e.IdPermiso).HasColumnName("ID_PERMISO");
            builder.Property(e => e.IdMenu).HasColumnName("ID_MENU");
            builder.Property(e => e.IdRol).HasColumnName("ID_ROL");

            builder.HasOne(d => d.IdMenuNavigation).WithMany(p => p.Permisos)
                .HasForeignKey(d => d.IdMenu)
                .HasConstraintName("FK__PERMISO__ID_MENU__4F7CD00D");

            builder.HasOne(d => d.IdRolNavigation).WithMany(p => p.Permisos)
                .HasForeignKey(d => d.IdRol)
                .HasConstraintName("FK__PERMISO__ID_ROL__4E88ABD4");
        }
    }
}
