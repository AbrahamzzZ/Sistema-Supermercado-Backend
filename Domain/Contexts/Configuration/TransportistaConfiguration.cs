using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Contexts.Configuration
{
    public class TransportistaConfiguration : IEntityTypeConfiguration<Transportistum>
    {
        public void Configure(EntityTypeBuilder<Transportistum> builder)
        {
            builder.HasKey(e => e.Id_Transportista).HasName("PK__TRANSPOR__057F895C84D4FAF7");

            builder.ToTable("TRANSPORTISTA");

            builder.Property(e => e.Id_Transportista).HasColumnName("ID_TRANSPORTISTA");
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
            builder.Property(e => e.Usuario_Creacion).HasColumnName("USUARIO_CREACION");
            builder.Property(e => e.Fecha_Creacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("FECHA_CREACION");
            builder.Property(e => e.Usuario_Modificacion).HasColumnName("USUARIO_MODIFICACION");
            builder.Property(e => e.Fecha_Modificacion)
                .HasColumnType("datetime")
                .HasColumnName("FECHA_MODIFICACION");
            builder.Property(e => e.Foto).HasColumnName("FOTO");
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
