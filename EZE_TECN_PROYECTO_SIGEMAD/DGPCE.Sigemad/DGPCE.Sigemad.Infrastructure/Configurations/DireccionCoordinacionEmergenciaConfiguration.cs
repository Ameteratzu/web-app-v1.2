

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

        builder.HasOne(d => d.Suceso)
            .WithMany(i => i.DireccionCoordinacionEmergencias)
            .HasForeignKey(d => d.IdSuceso)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
