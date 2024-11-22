using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class DatosPrincipalesConfiguracion : IEntityTypeConfiguration<DatoPrincipal>
{
    public void Configure(EntityTypeBuilder<DatoPrincipal> builder)
    {

        builder.ToTable("DatoPrincipal");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.FechaCreacion)
            .HasColumnType("datetime");

        builder.Property(e => e.FechaModificacion)
            .HasColumnType("datetime");

        builder.Property(e => e.FechaModificacion)
            .HasColumnType("datetime");

        builder.Property(e => e.CreadoPor)
          .HasMaxLength(500)
          .IsUnicode(false);

        builder.Property(e => e.ModificadoPor)
          .HasMaxLength(500)
          .IsUnicode(false);

        builder.Property(e => e.EliminadoPor)
        .HasMaxLength(500)
        .IsUnicode(false);

        // Relación uno a uno con Evolucion        
        builder.HasOne(r => r.Evolucion)
            .WithOne(e => e.DatoPrincipal)
            .HasForeignKey<DatoPrincipal>(r => r.Id)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
