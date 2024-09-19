using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;

public class SucesoConfiguration : IEntityTypeConfiguration<Suceso>
{
    public void Configure(EntityTypeBuilder<Suceso> builder)
    {
        builder.HasKey(e => e.Id).HasName("Suceso_PK");

        builder.ToTable("Suceso");

        builder.Property(e => e.IdTipo).HasDefaultValue(1);

        builder.HasOne(d => d.TipoSuceso).WithMany()
            .HasForeignKey(d => d.IdTipo)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("TipoSucesoSuceso");
    }
}
