using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGPCE.Sigemad.Infrastructure.Configurations.Ope.Administracion
{
    public class OpeAreaDescansoConfiguration : IEntityTypeConfiguration<OpeAreaDescanso>
    {
        public void Configure(EntityTypeBuilder<OpeAreaDescanso> builder)
        {

            builder.HasKey(e => e.Id);

            builder.ToTable("OPE_AreaDescanso");

            builder.HasOne(opeAreaDescanso => opeAreaDescanso.OpeAreaDescansoTipo)
                  .WithMany()
                  .HasForeignKey(opeAreaDescanso => opeAreaDescanso.IdOpeAreaDescansoTipo)
                  .OnDelete(DeleteBehavior.Restrict)
                  .HasConstraintName("FK_OpeAreaDescanso_OpeAreaDescansoTipo");

            builder.HasOne(opeAreaDescanso => opeAreaDescanso.OpeEstadoOcupacion)
                  .WithMany()
                  .HasForeignKey(opeAreaDescanso => opeAreaDescanso.IdOpeEstadoOcupacion)
                  .OnDelete(DeleteBehavior.Restrict)
                  .HasConstraintName("FK_OpeAreaDescanso_OpeEstadoOcupacion");

            builder.HasOne(d => d.Ccaa).WithMany()
             .HasForeignKey(d => d.IdCcaa)
             .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.Provincia).WithMany()
           .HasForeignKey(d => d.IdProvincia)
           .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.Municipio).WithMany()
                .HasForeignKey(d => d.IdMunicipio)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
