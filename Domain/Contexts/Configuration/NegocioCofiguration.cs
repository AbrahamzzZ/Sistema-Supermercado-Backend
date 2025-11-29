using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Contexts.Configuration
{
    public class NegocioCofiguration : IEntityTypeConfiguration<Negocio>
    {
        public void Configure(EntityTypeBuilder<Negocio> builder)
        {
            builder.HasKey(e => e.Id_Negocio).HasName("PK__NEGOCIO__D81B51AFBEE006A4");

            builder.ToTable("NEGOCIO");

            builder.HasIndex(e => e.Ruc, "UQ__NEGOCIO__CAF3326B49023C6A").IsUnique();

            builder.HasIndex(e => e.Telefono, "UQ__NEGOCIO__D6F1694583FA83D5").IsUnique();

            builder.Property(e => e.Id_Negocio).HasColumnName("ID_NEGOCIO");
            builder.Property(e => e.Correo_Electronico)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("CORREO_ELECTRONICO");
            builder.Property(e => e.Direccion)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("DIRECCION");
            builder.Property(e => e.Logo).HasColumnName("LOGO");
            builder.Property(e => e.Nombre)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");
            builder.Property(e => e.Ruc)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("RUC");
            builder.Property(e => e.Telefono)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("TELEFONO");
        }
    }
}
