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
        builder.HasQueryFilter(r => r.Borrado == false);

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

        // 🔹 Relación con Evolucion
        builder.HasOne(r => r.Evolucion)
            .WithMany(e => e.Registros)
            .HasForeignKey(r => r.IdEvolucion)
            .OnDelete(DeleteBehavior.Restrict);

        //builder.HasIndex(e => e.IdEvolucion)
        //    .IsUnique()
        //    .HasFilter("[Borrado] = 0");

        builder.HasMany(r => r.ProcedenciaDestinos)
            .WithOne(rpd => rpd.Registro)
            .HasForeignKey(rpd => rpd.IdRegistro)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
