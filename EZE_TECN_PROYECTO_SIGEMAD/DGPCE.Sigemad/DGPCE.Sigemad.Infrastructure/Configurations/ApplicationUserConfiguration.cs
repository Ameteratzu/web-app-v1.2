using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK_ApplicationUsers");

            builder.ToTable("ApplicationUsers");

            builder.Property(e => e.Nombre)
                .IsRequired();

            builder.Property(e => e.Apellidos)
                .IsRequired();

            builder.Property(e => e.Email)
                .IsRequired();

            builder.Property(e => e.Telefono)
                .IsRequired();

            builder.Property(e => e.FechaCreacion)
                .HasColumnName("CreatedDate");

            builder.Property(e => e.CreadoPor)
             .HasColumnName("CreatedBy");

            builder.Property(e => e.FechaModificacion)
             .HasColumnName("LastModifiedDate");

            builder.Property(e => e.ModificadoPor)
             .HasColumnName("LastModifiedBy");

        }
    }
}
