

using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations
{
    public class EvolucionProcedenciaDestinoConfiguration : IEntityTypeConfiguration<EvolucionProcedenciaDestino>
    {
        public void Configure(EntityTypeBuilder<EvolucionProcedenciaDestino> builder)
        {
       
                builder.ToTable("Evolucion_ProcedenciaDestino");

                builder.HasKey(e => e.Id);

                builder.Property(e => e.Id)
                      .ValueGeneratedOnAdd();

                builder.HasOne(e => e.Evolucion)
                       .WithMany(e => e.EvolucionProcedenciaDestinos)
                      .HasForeignKey(e => e.IdEvolucion)
                      .OnDelete(DeleteBehavior.Cascade);

                builder.HasOne(e => e.ProcedenciaDestino)
                      .WithMany()
                      .HasForeignKey(e => e.IdProcedenciaDestino)
                      .OnDelete(DeleteBehavior.Cascade);
            
        }
    }
}
