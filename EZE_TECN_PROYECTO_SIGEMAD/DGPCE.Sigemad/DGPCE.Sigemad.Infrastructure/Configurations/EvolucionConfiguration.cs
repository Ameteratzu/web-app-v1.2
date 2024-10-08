using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations
{
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

        builder.Property(e => e.Resumen)
            .IsRequired();


        builder.Property(e => e.SuperficieAfectadaHectarea)
            .HasColumnType("decimal(10, 2)");

        builder.Property(e => e.FechaCreacion)
            .HasColumnType("datetime");

        builder.Property(e => e.FechaModificacion)
            .HasColumnType("datetime");

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

        builder.HasOne(d => d.ProcedenciaDestino)
            .WithMany()
            .HasForeignKey(d => d.IdProcedenciaDestino)
            .HasConstraintName("FK_Evolucion_ProcedenciaDestino");

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


        builder.HasOne(d => d.EstadoEvolucion)
            .WithMany()
            .HasForeignKey(d => d.IdEstadoEvolucion)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Evolucion_EstadoEvolucion");
        }
        }
    }

