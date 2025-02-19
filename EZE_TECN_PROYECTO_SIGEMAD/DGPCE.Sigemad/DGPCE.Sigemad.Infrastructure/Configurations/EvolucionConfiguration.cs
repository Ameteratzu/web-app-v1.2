﻿using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;

public class EvolucionConfiguration : IEntityTypeConfiguration<Evolucion>
{
    public void Configure(EntityTypeBuilder<Evolucion> builder)
    {

        builder.ToTable("Evolucion");

        builder.Property(e => e.IdSuceso)
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

        // Configurar relación uno a uno con Parametro
        builder.HasOne(e => e.Parametro)
            .WithOne(r => r.Evolucion)
            .HasForeignKey<Parametro>(r => r.Id) // El Id de Registro es también la clave foránea
            .OnDelete(DeleteBehavior.Cascade); // Configurar comportamiento de eliminación en cascada

        // Configurar relación uno a uno con Dato Principal
        builder.HasOne(e => e.DatoPrincipal)
            .WithOne(r => r.Evolucion)
            .HasForeignKey<DatoPrincipal>(r => r.Id) // El Id de Registro es también la clave foránea
            .OnDelete(DeleteBehavior.Cascade); // Configurar comportamiento de eliminación en cascada


        builder.HasMany(e => e.AreaAfectadas)
            .WithOne(r => r.Evolucion)
            .HasForeignKey(r => r.IdEvolucion) // El Id de Registro es también la clave foránea
            .OnDelete(DeleteBehavior.Restrict); // Configurar comportamiento de eliminación en cascada

        builder.HasMany(e => e.IntervencionMedios)
            .WithOne(r => r.Evolucion)
            .HasForeignKey(r => r.IdEvolucion) // El Id de Registro es también la clave foránea
            .OnDelete(DeleteBehavior.Restrict); // Configurar comportamiento de eliminación en cascada


        // Relación uno a uno con Suceso
        builder.HasOne(d => d.Suceso)
            .WithOne(s => s.Evolucion)
            .HasForeignKey<Evolucion>(d => d.IdSuceso)
            .OnDelete(DeleteBehavior.Restrict); // Evita eliminación en cascada

    }
}


