﻿using DGPCE.Sigemad.Domain.Modelos;
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

        builder.HasIndex(e => e.IdMunicipio, "IX_Incendio_2");

        builder.HasIndex(e => new { e.FechaCreacion, e.FechaModificacion }, "IX_Incendio_3");

        builder.Property(e => e.Id)
            .UseIdentityColumn();
        builder.Property(e => e.Comentarios).HasColumnType("text");
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
        
        builder.Property(e => e.ModificadoPor)
            .HasMaxLength(500)
            .IsUnicode(false);
        builder.Property(e => e.UtmX).HasColumnName("UTM_X");
        builder.Property(e => e.UtmY).HasColumnName("UTM_Y");

        builder.HasOne(d => d.ClaseSuceso).WithMany()
            .HasForeignKey(d => d.IdClaseSuceso)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("ClaseSucesoIncendio");

        builder.HasOne(d => d.Municipio).WithMany()
            .HasForeignKey(d => d.IdMunicipio)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("MunicipioIncendio");

        builder.HasOne(d => d.NivelGravedad).WithMany()
            .HasForeignKey(d => d.IdPrevisionPeligroGravedad)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Incendio_NivelGravedad");

        builder.HasOne(d => d.Provincia).WithMany()
            .HasForeignKey(d => d.IdProvincia)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("ProvinciaIncendio");

        builder.HasOne(d => d.Suceso).WithMany()
            .HasForeignKey(d => d.IdSuceso)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("SucesoIncendio");

        builder.HasOne(d => d.Territorio).WithMany()
            .HasForeignKey(d => d.IdTerritorio)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("SucesoTerritorio");

        builder.HasOne(d => d.EstadoIncendio).WithMany()
            .HasForeignKey(d => d.IdEstado)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Incendio_EstadoIncendio");
    }
}