using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Infrastructure.Configurations
{
    public class ProvinciaConfiguration : IEntityTypeConfiguration<Provincia>
    {
        public void Configure(EntityTypeBuilder<Provincia> builder)
        {
            builder.HasKey(e => e.Id).HasName("Provincias_PK");

            builder.ToTable("Provincia");

            builder.HasMany(p => p.Municipios).WithOne(m => m.Provincia)
             .HasForeignKey(m => m.IdProvincia);

        }
    }
}
