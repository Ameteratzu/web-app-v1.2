using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Infrastructure.Configurations
{
    public class CCAAConfiguration : IEntityTypeConfiguration<Ccaa>
    {
        public void Configure(EntityTypeBuilder<Ccaa> builder)
        {

            builder.HasKey(e => e.Id).HasName("CCAA_PK");

            builder.ToTable("CCAA");

            builder.HasMany(c => c.Provincia).WithOne(p => p.IdCcaaNavigation)
             .HasForeignKey(p => p.IdCcaa);

        }
    }
}
