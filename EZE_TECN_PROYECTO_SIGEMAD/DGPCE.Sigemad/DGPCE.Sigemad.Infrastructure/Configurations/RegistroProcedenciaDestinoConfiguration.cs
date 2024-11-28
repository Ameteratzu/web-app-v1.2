

using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations;

public class RegistroProcedenciaDestinoConfiguration : IEntityTypeConfiguration<RegistroProcedenciaDestino>
{
    public void Configure(EntityTypeBuilder<RegistroProcedenciaDestino> builder)
    {

        builder.ToTable("Registro_ProcedenciaDestino");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
              .ValueGeneratedOnAdd();

        builder.HasOne(rpd => rpd.Registro)
            .WithMany(r => r.ProcedenciaDestinos)
            .HasForeignKey(rpd => rpd.IdRegistro)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(rpd => rpd.ProcedenciaDestino)
            .WithMany()
            .HasForeignKey(rpd => rpd.IdProcedenciaDestino)
            .OnDelete(DeleteBehavior.Restrict);

    }
}
