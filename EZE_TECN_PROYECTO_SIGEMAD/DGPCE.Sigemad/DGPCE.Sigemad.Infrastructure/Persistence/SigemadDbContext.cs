using DGPCE.Sigemad.Domain.Common;
using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NetTopologySuite.Geometries;

namespace DGPCE.Sigemad.Infrastructure.Persistence
{
    public class SigemadDbContext : DbContext
    {
        public SigemadDbContext(DbContextOptions<SigemadDbContext> options) : base(options)
        {
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseDomainModel>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.FechaCreacion = DateTime.Now;
                        entry.Entity.CreadoPor = "system";
                        break;

                    case EntityState.Modified:
                        entry.Entity.FechaModificacion = DateTime.Now;
                        entry.Entity.ModificadoPor = "system";
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Streamer>()
            //.HasMany(m => m.Videos)
            //    .WithOne(m => m.Streamer)
            //    .HasForeignKey(m => m.StreamerId)
            //    .IsRequired()
            //    .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<Director>()
            //.HasMany(v => v.Videos)
            //    .WithOne(d => d.Director)
            //    .HasForeignKey(d => d.DirectorId)
            //    .IsRequired()
            //    .OnDelete(DeleteBehavior.Restrict);


            //modelBuilder.Entity<Video>()
            //    .HasMany(a => a.Actores)
            //    .WithMany(v => v.Videos)
            //    .UsingEntity<VideoActor>(
            //         j => j
            //           .HasOne(p => p.Actor)
            //           .WithMany(p => p.VideoActors)
            //           .HasForeignKey(p => p.ActorId),
            //        j => j
            //            .HasOne(p => p.Video)
            //            .WithMany(p => p.VideoActors)
            //            .HasForeignKey(p => p.VideoId),
            //        j =>
            //        {
            //            j.HasKey(t => new { t.ActorId, t.VideoId });
            //        }
            //);

            //modelBuilder.Entity<VideoActor>().Ignore(va => va.Id);

            modelBuilder.Entity<Ccaa>()
            .HasMany(c => c.Provincia)
            .WithOne(p => p.IdCcaaNavigation)
            .HasForeignKey(p => p.IdCcaa);

            modelBuilder.Ignore<NetTopologySuite.Geometries.Coordinate>();
            modelBuilder.Ignore<NetTopologySuite.Geometries.Geometry>();
            base.OnModelCreating(modelBuilder);
        }


        public DbSet<Alerta>? Alertas { get; set; }
        public DbSet<EstadoAlerta>? EstadosAlertas { get; set; }
        public DbSet<TipoSuceso> TipoSuceso { get; set; }
        public DbSet<Menu> Menu { get; set; }
        public DbSet<Ccaa>? CCAA { get; set; }
        public DbSet<Territorio>? Territorio { get; set; }

    }
}
