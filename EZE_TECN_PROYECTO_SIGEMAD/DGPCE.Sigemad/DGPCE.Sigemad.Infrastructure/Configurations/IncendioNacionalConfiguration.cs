using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;

class IncendioNacionalConfiguration : IEntityTypeConfiguration<IncendioNacional>
{
    public void Configure(EntityTypeBuilder<IncendioNacional> builder)
    {
        builder.ToTable(nameof(IncendioNacional));

        builder.HasKey(n => n.IdIncendio);
        builder.Property(n => n.IdProvincia).IsRequired();
        builder.Property(n => n.IdMunicipio).IsRequired();

        builder.HasOne(d => d.Provincia).WithMany()
            .HasForeignKey(d => d.IdProvincia)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.Municipio).WithMany()
            .HasForeignKey(d => d.IdMunicipio)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
