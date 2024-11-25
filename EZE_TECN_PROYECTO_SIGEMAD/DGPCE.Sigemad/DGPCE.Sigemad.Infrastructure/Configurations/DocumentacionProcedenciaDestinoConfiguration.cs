using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DGPCE.Sigemad.Infrastructure.Configurations;
public class DocumentacionProcedenciaDestinoConfiguration: IEntityTypeConfiguration<DocumentacionProcedenciaDestino>
{
    public void Configure(EntityTypeBuilder<DocumentacionProcedenciaDestino> builder)
    {
        builder.ToTable("DocumentacionProcedenciaDestino");
        builder.HasKey(e => e.Id);

        builder.HasOne(d => d.Documentacion)
               .WithMany()
               .HasForeignKey(d => d.IdDocumentacion);

        builder.HasOne(d => d.ProcedenciaDestino)
               .WithMany()
               .HasForeignKey(d => d.IdProcedenciaDestino);
    }
}
