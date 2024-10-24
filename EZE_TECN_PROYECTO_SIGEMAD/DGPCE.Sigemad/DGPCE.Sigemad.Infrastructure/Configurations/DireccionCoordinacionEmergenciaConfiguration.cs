

using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DGPCE.Sigemad.Infrastructure.Configurations
{
    public class DireccionCoordinacionEmergenciaConfiguration : IEntityTypeConfiguration<DireccionCoordinacionEmergencia>
    {
        public void Configure(EntityTypeBuilder<DireccionCoordinacionEmergencia> builder)
        {
            builder.ToTable("DireccionCoordinacionEmergencia");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.AutoridadQueDirige)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(d => d.LugarCECOPI)
                .HasMaxLength(255);

            builder.Property(d => d.LugarPMA)
                .HasMaxLength(255);

            builder.Property(e => e.GeoPosicionCECOPI)
                .HasColumnType("geometry");

            builder.Property(e => e.GeoPosicionPMA)
        .       HasColumnType("geometry");

            builder.HasOne(d => d.Incendio)
                .WithMany()
                .HasForeignKey(d => d.IdIncendio)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.TipoDireccionEmergencia)
                .WithMany()
                .HasForeignKey(d => d.IdTipoDireccionEmergencia)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.ProvinciaCECOPI)
                .WithMany()
                .HasForeignKey(d => d.IdProvinciaCECOPI)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.MunicipioCECOPI)
                .WithMany()
                .HasForeignKey(d => d.IdMunicipioCECOPI)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.ProvinciaPMA)
                .WithMany()
                .HasForeignKey(d => d.IdProvinciaPMA)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.MunicipioPMA)
                .WithMany()
                .HasForeignKey(d => d.IdMunicipioPMA)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.ActivacionPlanEmergencia)
                .WithOne(a => a.DireccionCoordinacionEmergencia)
                .HasForeignKey<ActivacionPlanEmergencia>(a => a.IdDireccionCoordinacionEmergencia)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
