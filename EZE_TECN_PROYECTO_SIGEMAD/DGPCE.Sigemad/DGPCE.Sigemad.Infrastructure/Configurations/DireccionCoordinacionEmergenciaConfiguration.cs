

using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;

public class DireccionCoordinacionEmergenciaConfiguration : IEntityTypeConfiguration<DireccionCoordinacionEmergencia>
{
    public void Configure(EntityTypeBuilder<DireccionCoordinacionEmergencia> builder)
    {
        builder.ToTable("DireccionCoordinacionEmergencia");

        builder.HasKey(d => d.Id);

        // Relación uno a uno con Suceso
        builder.HasOne(d => d.Suceso)
            .WithOne(s => s.DireccionCoordinacionEmergencia)
            .HasForeignKey<DireccionCoordinacionEmergencia>(d => d.IdSuceso)
            .OnDelete(DeleteBehavior.Restrict); // Evita eliminación en cascada
    }
}
