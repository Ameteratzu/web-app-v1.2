using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class ImpactoEvolucionConfiguration : IEntityTypeConfiguration<ImpactoEvolucion>
{
    public void Configure(EntityTypeBuilder<ImpactoEvolucion> builder)
    {
        builder.HasKey(e => e.Id);
        builder.ToTable("ImpactoEvolucion");

        builder.Property(e => e.ZonaPlanificacion).HasColumnType("geometry");

        builder.HasOne(d => d.Evolucion)
            .WithMany()
            .HasForeignKey(d => d.IdEvolucion)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.ImpactoClasificado)
            .WithMany()
            .HasForeignKey(d => d.IdImpactoClasificado)
            .OnDelete(DeleteBehavior.Restrict);


        builder.HasOne(d => d.TipoDanio)
            .WithMany()
            .HasForeignKey(d => d.IdTipoDanio)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
