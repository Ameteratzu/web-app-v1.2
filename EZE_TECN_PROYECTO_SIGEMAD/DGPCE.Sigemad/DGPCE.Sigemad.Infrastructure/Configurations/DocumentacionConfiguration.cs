using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Infrastructure.Configurations
{
    public class DocumentacionConfiguration : IEntityTypeConfiguration<Documentacion>
    {
        public void Configure(EntityTypeBuilder<Documentacion> builder)
        {
            builder.ToTable("Documentacion");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.FechaHora)
                   .IsRequired();

            builder.Property(e => e.FechaHoraSolicitud)
                   .IsRequired();

            builder.Property(e => e.Descripcion)
                   .IsRequired()
                   .HasMaxLength(255);

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

            builder.HasOne(d => d.TipoDocumento)
                   .WithMany()
                   .HasForeignKey(d => d.IdTipoDocumento);

            builder.HasOne(d => d.Incendio)
                   .WithMany()
                   .HasForeignKey(d => d.IdIncendio);

            builder.HasOne(d => d.Archivo)
                   .WithMany()
                   .HasForeignKey(d => d.IdArchivo);
        }
    }
}