using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Contexts.Configuration
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasKey(e => e.Id_Usuario).HasName("PK__USUARIO__91136B90182039E5");

            builder.ToTable("USUARIO");

            builder.Property(e => e.Id_Usuario).HasColumnName("ID_USUARIO");
            builder.Property(e => e.Clave)
                .HasMaxLength(300)
                .HasColumnName("CLAVE");
            builder.Property(e => e.Codigo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CODIGO");
            builder.Property(e => e.Correo_Electronico)
                .HasMaxLength(50)
                .HasColumnName("CORREO_ELECTRONICO");
            builder.Property(e => e.Estado).HasColumnName("ESTADO");
            builder.Property(e => e.Fecha_Creacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("FECHA_CREACION");
            builder.Property(e => e.Id_Rol).HasColumnName("ID_ROL");
            builder.Property(e => e.Nombre_Completo)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("NOMBRE_COMPLETO");

            builder.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.Id_Rol)
                .HasConstraintName("FK__USUARIO__ID_ROL__52593CB8");
        }
    }
}
