using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Contexts.Configuration
{
    public class SucursalConfiguration : IEntityTypeConfiguration<Sucursal>
    {
        public void Configure(EntityTypeBuilder<Sucursal> builder)
        {
            builder.HasKey(e => e.Id_Sucursal).HasName("PK__SUCURSAL__AB3873837FF5CDB2");

            builder.ToTable("SUCURSAL");

            builder.HasIndex(e => e.Codigo, "UQ__SUCURSAL__CC87E126E64D406E").IsUnique();

            builder.Property(e => e.Id_Sucursal).HasColumnName("ID_SUCURSAL");
            builder.Property(e => e.Ciudad_Sucursal)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("CIUDAD_SUCURSAL");
            builder.Property(e => e.Codigo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CODIGO");
            builder.Property(e => e.Direccion_Sucursal)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("DIRECCION_SUCURSAL");
            builder.Property(e => e.Estado).HasColumnName("ESTADO");
            builder.Property(e => e.Id_Negocio).HasColumnName("ID_NEGOCIO");
            builder.Property(e => e.Nombre_Sucursal)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("NOMBRE_SUCURSAL");

            builder.HasOne(d => d.IdNegocioNavigation).WithMany(p => p.Sucursals)
                .HasForeignKey(d => d.Id_Negocio)
                .HasConstraintName("FK__SUCURSAL__ID_NEG__3E52440B");
        }
    }
}
