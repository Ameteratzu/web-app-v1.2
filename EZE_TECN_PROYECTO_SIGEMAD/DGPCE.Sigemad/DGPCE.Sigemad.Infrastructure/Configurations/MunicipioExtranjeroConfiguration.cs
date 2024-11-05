using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;

class MunicipioExtranjeroConfiguration : IEntityTypeConfiguration<MunicipioExtranjero>
{
    public void Configure(EntityTypeBuilder<MunicipioExtranjero> builder)
    {
        builder.ToTable(nameof(MunicipioExtranjero));

        builder.HasKey(e => e.Id);
        builder.HasOne(e => e.Distrito)
                           .WithMany()
                           .HasForeignKey(e => e.IdDistrito)
                           .OnDelete(DeleteBehavior.Restrict);
    }
}
