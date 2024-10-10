

using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations
{
    public class EntidadMenorConfiguration : IEntityTypeConfiguration<EntidadMenor>
    {
       
          public void Configure(EntityTypeBuilder<EntidadMenor> builder)
        {
            builder.HasKey(e => e.Id).HasName("Entidadmenor_PK");

            builder.ToTable("EntidadMenor");


            builder.HasOne(e => e.Distrito)
                               .WithMany()
                               .HasForeignKey(e => e.IdDistrito)
                               .OnDelete(DeleteBehavior.Restrict)
                               .HasConstraintName("DistritoEntidadmenor");
        }
    }
}
