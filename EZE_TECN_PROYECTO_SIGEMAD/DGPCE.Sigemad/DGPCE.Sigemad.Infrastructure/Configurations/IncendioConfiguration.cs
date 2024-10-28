using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;

internal class IncendioConfiguration : IEntityTypeConfiguration<Incendio>
{
    public void Configure(EntityTypeBuilder<Incendio> builder)
    {
        builder.HasKey(e => e.Id).HasName("Sucesos_PK");

        builder.ToTable("Incendio");

        builder.HasIndex(e => e.Denominacion, "IX_Incendio");

        builder.HasIndex(e => e.IdSuceso, "IX_Incendio_1");

        builder.HasIndex(e => new { e.FechaCreacion, e.FechaModificacion }, "IX_Incendio_3");

        builder.Property(e => e.Id)
            .UseIdentityColumn();

        builder.Property(e => e.CreadoPor)
            .HasMaxLength(500)
            .IsUnicode(false);
        builder.Property(e => e.Denominacion)
            .HasMaxLength(255)
            .IsUnicode(false);
        builder.Property(e => e.FechaCreacion).HasColumnType("datetime");
        builder.Property(e => e.FechaInicio).HasColumnType("datetime");
        builder.Property(e => e.FechaModificacion).HasColumnType("datetime");
        builder.Property(e => e.GeoPosicion).HasColumnType("geometry");
        builder.Property(e => e.RutaMapaRiesgo).HasColumnType("text");

        builder.Property(e => e.ModificadoPor)
            .HasMaxLength(500)
            .IsUnicode(false);
        builder.Property(e => e.UtmX).HasColumnName("UTM_X");
        builder.Property(e => e.UtmY).HasColumnName("UTM_Y");

        builder.HasOne(d => d.ClaseSuceso).WithMany()
            .HasForeignKey(d => d.IdClaseSuceso)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("ClaseSucesoIncendio");

        builder.HasOne(d => d.Suceso).WithMany()
            .HasForeignKey(d => d.IdSuceso)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("SucesoIncendio");

        builder.HasOne(d => d.Territorio).WithMany()
            .HasForeignKey(d => d.IdTerritorio)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("IncendioTerritorio");

        builder.HasOne(d => d.EstadoSuceso).WithMany()
            .HasForeignKey(d => d.IdEstadoSuceso)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Incendio_EstadoSuceso");

        // Relación uno a uno con IncendioNacional
        builder.HasOne(i => i.IncendioNacional)
               .WithOne(n => n.Incendio)
               .HasForeignKey<IncendioNacional>(n => n.IdIncendio)
               .IsRequired(false)
               .OnDelete(DeleteBehavior.Restrict);

        // Relación uno a uno con IncendioExtranjero
        builder.HasOne(i => i.IncendioExtranjero)
               .WithOne(e => e.Incendio)
               .HasForeignKey<IncendioExtranjero>(e => e.IdIncendio)
               .IsRequired(false)
               .OnDelete(DeleteBehavior.Restrict);


    }
}