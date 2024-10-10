using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DGPCE.Sigemad.Infrastructure.Configurations
{
    internal class DistritoConfiguration : IEntityTypeConfiguration<Distrito>
    {

       

        public void Configure(EntityTypeBuilder<Distrito> builder)
        {
            builder.HasKey(e => e.Id).HasName("Distrito_PK");

            builder.ToTable("Distrito");

            builder.Property(e => e.IdPais)
                .IsRequired();

            builder.HasOne(e => e.Pais)
                               .WithMany()
                               .HasForeignKey(e => e.IdPais)
                               .OnDelete(DeleteBehavior.Restrict)
                               .HasConstraintName("PaisDistrito");
        }
    }
}
