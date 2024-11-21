using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;

public class EvolucionConfiguration : IEntityTypeConfiguration<Evolucion>
{
    public void Configure(EntityTypeBuilder<Evolucion> builder)
    {
        builder.ToTable("Evolucion");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.IdIncendio)
            .IsRequired();

        builder.HasOne(d => d.Incendio)
            .WithMany(i => i.Evoluciones)
            .HasForeignKey(d => d.IdIncendio)
            .OnDelete(DeleteBehavior.Restrict);
    }
}


