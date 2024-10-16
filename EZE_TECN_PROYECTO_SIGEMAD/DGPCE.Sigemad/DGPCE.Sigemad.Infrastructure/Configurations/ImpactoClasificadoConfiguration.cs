using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class ImpactoClasificadoConfiguration : IEntityTypeConfiguration<ImpactoClasificado>
{
    public void Configure(EntityTypeBuilder<ImpactoClasificado> builder)
    {
        builder.HasKey(x => x.Id);
        builder.ToTable("ImpactoClasificado");
    }
}
