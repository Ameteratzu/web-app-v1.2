
using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations
{
    public class IntervencionMedioConfigurations : IEntityTypeConfiguration<IntervencionMedio>
    {
        public void Configure(EntityTypeBuilder<IntervencionMedio> builder)
        {
            builder.ToTable("IntervencionMedio");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id)
                .ValueGeneratedOnAdd();

            builder.Property(t => t.Cantidad)
                .IsRequired();

            builder.Property(t => t.Unidad)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.GeoPosicion).HasColumnType("geometry");

            builder.HasOne(t => t.Evolucion)
                .WithMany()
                .HasForeignKey(t => t.IdEvolucion)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.TipoIntervencionMedio)
                .WithMany()
                .HasForeignKey(t => t.IdTipoIntervencionMedio)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.CaracterMedio)
                .WithMany()
                .HasForeignKey(t => t.IdCaracterMedio)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.ClasificacionMedio)
                .WithMany()
                .HasForeignKey(t => t.IdClasificacionMedio)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.TitularidadMedio)
                .WithMany()
                .HasForeignKey(t => t.IdTitularidadMedio)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.Municipio)
                .WithMany()
                .HasForeignKey(t => t.IdMunicipio)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
  }
