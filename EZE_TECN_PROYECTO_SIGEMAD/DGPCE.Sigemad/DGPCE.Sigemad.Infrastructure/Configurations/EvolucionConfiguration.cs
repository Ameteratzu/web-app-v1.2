﻿using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;

public class EvolucionConfiguration : IEntityTypeConfiguration<Evolucion>
{
    public void Configure(EntityTypeBuilder<Evolucion> builder)
    {
        builder.HasKey(e => e.Id).HasName("PK__Evolucio__3214EC074C47BA56");

        builder.ToTable("Evolucion");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.FechaHoraEvolucion)
            .IsRequired();

        builder.Property(e => e.IdEntradaSalida)
            .IsRequired();

        builder.Property(e => e.IdMedio)
            .IsRequired();

        builder.Property(e => e.IdTecnico)
            .IsRequired();

        builder.Property(e => e.IdTipoRegistro)
            .IsRequired();

        builder.Property(e => e.IdProvinciaAfectada)
           .IsRequired();

        builder.Property(e => e.IdEntidadMenor)
            .IsRequired();

        builder.Property(e => e.IdMunicipioAfectado)
       .IsRequired();

        builder.Property(e => e.IdIncendio)
            .IsRequired();

        builder.Property(e => e.Resumen)
        .IsRequired();


        builder.Property(e => e.SuperficieAfectadaHectarea)
            .HasColumnType("decimal(10, 2)");

        builder.Property(e => e.FechaCreacion)
            .HasColumnType("datetime");

        builder.Property(e => e.FechaModificacion)
            .HasColumnType("datetime");

        builder.Property(e => e.CreadoPor)
          .HasMaxLength(500)
          .IsUnicode(false);


        builder.Property(e => e.Observaciones)
            .HasColumnType("text");

        builder.Property(e => e.Prevision)
            .HasColumnType("text");

        builder.Property(e => e.GeoPosicionAreaAfectada)
            .HasColumnType("geometry");

        builder.HasOne(d => d.EntradaSalida)
            .WithMany()
            .HasForeignKey(d => d.IdEntradaSalida)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Evolucion_EntradaSalida");

        builder.HasOne(d => d.Medio)
            .WithMany()
            .HasForeignKey(d => d.IdMedio)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Evolucion_Medio");

        builder.HasOne(d => d.Tecnico)
            .WithMany()
            .HasForeignKey(d => d.IdTecnico)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Evolucion_ApplicationUsers");

        builder.HasOne(d => d.Provincia)
        .WithMany()
        .HasForeignKey(d => d.IdProvinciaAfectada)
        .OnDelete(DeleteBehavior.Restrict)
        .HasConstraintName("FK_Evolucion_Provincia");

        builder.HasOne(d => d.Municipio)
            .WithMany()
            .HasForeignKey(d => d.IdMunicipioAfectado)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Evolucion_Municipio");

        builder.HasOne(d => d.Incendio)
           .WithMany()
           .HasForeignKey(d => d.IdIncendio)
           .OnDelete(DeleteBehavior.Restrict)
           .HasConstraintName("FK_Evolucion_Incendio");


        builder.HasOne(d => d.EstadoIncendio)
            .WithMany()
            .HasForeignKey(d => d.IdEstadoIncendio)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Evolucion_EstadoIncendio");


        builder.HasOne(d => d.EntidadMenor)
            .WithMany()
            .HasForeignKey(d => d.IdEntidadMenor)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Evolucion_EntidadMenor");

        builder.HasOne(d => d.TipoRegistro)
            .WithMany()
            .HasForeignKey(d => d.IdTipoRegistro)
            .OnDelete(DeleteBehavior.Restrict);


        builder.HasMany(e => e.EvolucionProcedenciaDestinos)
             .WithOne(epd => epd.Evolucion)
             .HasForeignKey(epd => epd.IdEvolucion)
             .OnDelete(DeleteBehavior.Cascade);
    }
}


