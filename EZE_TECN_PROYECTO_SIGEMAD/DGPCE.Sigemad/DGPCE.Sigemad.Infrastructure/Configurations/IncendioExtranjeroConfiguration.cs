using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;
internal class IncendioExtranjeroConfiguration : IEntityTypeConfiguration<IncendioExtranjero>
{
    public void Configure(EntityTypeBuilder<IncendioExtranjero> builder)
    {
        builder.ToTable(nameof(IncendioExtranjero));
        builder.HasKey(n => n.IdIncendio);
        builder.Property(e => e.IdPais).IsRequired();
        builder.Property(e => e.Ubicacion).IsRequired().HasMaxLength(255);

        builder.HasOne(d => d.Pais).WithMany()
            .HasForeignKey(d => d.IdPais)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.Distrito).WithMany()
            .HasForeignKey(d => d.IdDistrito)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.EntidadMenor).WithMany()
            .HasForeignKey(d => d.IdEntidadMenor)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
