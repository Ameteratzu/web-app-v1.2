

using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations
{
    public class EvolucionProcedenciaDestinoConfiguration : IEntityTypeConfiguration<RegistroProcedenciaDestino>
    {
        public void Configure(EntityTypeBuilder<RegistroProcedenciaDestino> builder)
        {
       
                builder.ToTable("Registro_ProcedenciaDestino");

                builder.HasKey(e => e.Id);

                builder.Property(e => e.Id)
                      .ValueGeneratedOnAdd();

                builder.HasOne(e => e.Registro)
                       .WithMany(e => e.RegistrosProcedenciasDestinos)
                      .HasForeignKey(e => e.IdRegistroEvolucion)
                      .OnDelete(DeleteBehavior.Cascade);

                builder.HasOne(e => e.ProcedenciaDestino)
                      .WithMany()
                      .HasForeignKey(e => e.IdProcedenciaDestino)
                      .OnDelete(DeleteBehavior.Cascade);
            
        }
    }
}
