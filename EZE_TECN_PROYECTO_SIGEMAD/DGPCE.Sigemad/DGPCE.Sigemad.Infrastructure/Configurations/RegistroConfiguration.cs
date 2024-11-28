using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace DGPCE.Sigemad.Infrastructure.Configurations;
internal class RegistroConfiguration : IEntityTypeConfiguration<Registro>
{
    public void Configure(EntityTypeBuilder<Registro> builder)
    {

        builder.ToTable("Registro");

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

        builder.HasOne(d => d.Medio)
           .WithMany()
           .HasForeignKey(d => d.IdMedio)
           .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.EntradaSalida)
            .WithMany()
            .HasForeignKey(d => d.IdEntradaSalida)
            .OnDelete(DeleteBehavior.Restrict);

        // Configurar relación uno a uno con Evolucion
        builder.HasOne(r => r.Evolucion)
            .WithOne(e => e.Registro)
            .HasForeignKey<Registro>(r => r.Id); // El Id de Registro es clave primaria y foránea

        builder.HasMany(r => r.ProcedenciaDestinos)
            .WithOne(rpd => rpd.Registro)
            .HasForeignKey(rpd => rpd.IdRegistro)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
