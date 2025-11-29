using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Contexts.Configuration
{
    public class LogConfiguration : IEntityTypeConfiguration<Log>
    {
        public void Configure(EntityTypeBuilder<Log> builder)
        {
            builder.HasKey(e => e.Id_Log).HasName("PK__LOG_ERRO__2DBE379D9B7F916B");

            builder.ToTable("LOG");

            builder.Property(e => e.Id_Log).HasColumnName("ID_LOG");
            builder.Property(e => e.Codigo_Error)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("CODIGO_ERROR");
            builder.Property(e => e.Detalle_Error)
                .IsUnicode(false)
                .HasColumnName("DETALLE_ERROR");
            builder.Property(e => e.Endpoint)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("ENDPOINT");
            builder.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("FECHA");
            builder.Property(e => e.Id_Usuario).HasColumnName("ID_USUARIO");
            builder.Property(e => e.Mensaje_Error)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("MENSAJE_ERROR");
            builder.Property(e => e.Metodo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("METODO");
            builder.Property(e => e.Nivel)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("ERROR")
                .HasColumnName("NIVEL");
        }
    }
}
