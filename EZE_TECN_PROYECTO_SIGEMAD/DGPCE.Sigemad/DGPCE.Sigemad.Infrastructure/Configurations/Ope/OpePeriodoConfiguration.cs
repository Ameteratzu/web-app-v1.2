using DGPCE.Sigemad.Domain.Modelos;
using DGPCE.Sigemad.Domain.Modelos.Ope;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DGPCE.Sigemad.Infrastructure.Configurations
{
    public class OpePeriodoConfiguration : IEntityTypeConfiguration<OpePeriodo>
    {
        public void Configure(EntityTypeBuilder<OpePeriodo> builder)
        {

            builder.HasKey(e => e.Id).HasName("OpePeriodosTipos_PK");

            builder.ToTable("OPE_Periodo");

            builder.HasOne(opePeriodo => opePeriodo.OpePeriodoTipo)
                   .WithMany()
                   .HasForeignKey(opePeriodo => opePeriodo.IdOpePeriodoTipo) 
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_OpePeriodo_OpePeriodoTipo");  // El nombre de la restricción de la clave foránea
        }
    }
}
