using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;


namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class ActivacionPlanEmergenciaConfiguration : IEntityTypeConfiguration<ActivacionPlanEmergencia>
{
    public void Configure(EntityTypeBuilder<ActivacionPlanEmergencia> builder)
    {
        builder.ToTable("ActivacionPlanesEmergencia");

        builder.HasKey(a => a.IdDireccionCoordinacionEmergencia);

        builder.Property(a => a.NombrePlan)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(a => a.AutoridadQueLoActiva)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(a => a.RutaDocumentoActivacion)
            .HasMaxLength(255);

        builder.HasOne(a => a.DireccionCoordinacionEmergencia)
            .WithOne(d => d.ActivacionPlanEmergencia)
            .HasForeignKey<ActivacionPlanEmergencia>(a => a.IdDireccionCoordinacionEmergencia)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.TipoPlan)
            .WithMany()
            .HasForeignKey(a => a.IdTipoPlan)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
