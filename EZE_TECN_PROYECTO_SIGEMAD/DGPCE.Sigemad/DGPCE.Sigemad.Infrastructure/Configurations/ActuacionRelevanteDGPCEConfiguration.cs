using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class ActuacionRelevanteDGPCEConfiguration : IEntityTypeConfiguration<ActuacionRelevanteDGPCE>
{
    public void Configure(EntityTypeBuilder<ActuacionRelevanteDGPCE> builder)
    {
        builder.ToTable(nameof(ActuacionRelevanteDGPCE));
        builder.HasKey(c => c.Id);

        builder.HasOne(s => s.Suceso)
            .WithMany()
            .HasForeignKey(s => s.IdSuceso)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(e => e.CreadoPor)
              .HasMaxLength(500)
              .IsUnicode(false);

        builder.Property(e => e.FechaCreacion).HasColumnType("datetime");
        builder.Property(e => e.FechaModificacion).HasColumnType("datetime");
        builder.Property(e => e.FechaEliminacion).HasColumnType("datetime");
        builder.Property(e => e.ModificadoPor)
            .HasMaxLength(500)
            .IsUnicode(false);

        builder.Property(e => e.EliminadoPor)
            .HasMaxLength(500)
            .IsUnicode(false);
    }
}
