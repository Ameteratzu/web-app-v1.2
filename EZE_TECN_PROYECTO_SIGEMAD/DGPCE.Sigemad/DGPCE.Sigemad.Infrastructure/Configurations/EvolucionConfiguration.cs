using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;

public class EvolucionConfiguration : IEntityTypeConfiguration<Evolucion>
{
    public void Configure(EntityTypeBuilder<Evolucion> builder)
    {

        builder.ToTable("Evolucion");

        builder.Property(e => e.IdIncendio)
         .IsRequired();

        builder.HasKey(e => e.Id);

        builder.Property(e => e.FechaCreacion)
            .HasColumnType("datetime");

        builder.Property(e => e.FechaModificacion)
            .HasColumnType("datetime");

        builder.Property(e => e.FechaModificacion)
            .HasColumnType("datetime");

        builder.Property(e => e.CreadoPor)
          .HasMaxLength(500)
          .IsUnicode(false);

        builder.Property(e => e.ModificadoPor)
          .HasMaxLength(500)
          .IsUnicode(false);

        builder.Property(e => e.EliminadoPor)
        .HasMaxLength(500)
        .IsUnicode(false);

        // Configurar relación uno a uno con Registro
        builder.HasOne(e => e.Registro)
            .WithOne(r => r.Evolucion)
            .HasForeignKey<Registro>(r => r.Id) // El Id de Registro es también la clave foránea
            .OnDelete(DeleteBehavior.Cascade); // Configurar comportamiento de eliminación en cascada


        builder.HasOne(d => d.Incendio)
                .WithMany(i => i.Evoluciones)
                .HasForeignKey(d => d.IdIncendio)
                .OnDelete(DeleteBehavior.Restrict);

    }
}


