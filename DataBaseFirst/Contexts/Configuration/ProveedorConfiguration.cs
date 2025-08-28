using DataBaseFirst.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataBaseFirst.Contexts.Configuration
{
    public class ProveedorConfiguration : IEntityTypeConfiguration<Proveedor>
    {
        public void Configure(EntityTypeBuilder<Proveedor> builder)
        {
            builder.HasKey(e => e.Id_Proveedor).HasName("PK__PROVEEDO__733DB4C4F2A65B2A");

            builder.ToTable("PROVEEDOR");

            builder.Property(e => e.Id_Proveedor).HasColumnName("ID_PROVEEDOR");
            builder.Property(e => e.Apellidos)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("APELLIDOS");
            builder.Property(e => e.Cedula)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CEDULA");
            builder.Property(e => e.Codigo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CODIGO");
            builder.Property(e => e.Correo_Electronico)
                .HasMaxLength(50)
                .HasColumnName("CORREO_ELECTRONICO");
            builder.Property(e => e.Estado).HasColumnName("ESTADO");
            builder.Property(e => e.Fecha_Registro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("FECHA_REGISTRO");
            builder.Property(e => e.Nombres)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("NOMBRES");
            builder.Property(e => e.Telefono)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("TELEFONO");
        }
    }
}
