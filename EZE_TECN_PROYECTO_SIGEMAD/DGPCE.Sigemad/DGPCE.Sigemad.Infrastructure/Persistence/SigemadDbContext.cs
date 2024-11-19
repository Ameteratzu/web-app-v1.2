using DGPCE.Sigemad.Domain.Common;
using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection;

namespace DGPCE.Sigemad.Infrastructure.Persistence
{
    public class SigemadDbContext : DbContext
    {
        public SigemadDbContext(DbContextOptions<SigemadDbContext> options) : base(options)
        {
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.FechaCreacion = DateTime.Now;
                        //entry.Entity.CreadoPor = "system";
                        break;

                    case EntityState.Modified:
                        entry.Entity.FechaModificacion = DateTime.Now;
                        //entry.Entity.ModificadoPor = "system";
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

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Alerta>().ToTable("Alerta");
            modelBuilder.Entity<EstadoAlerta>().ToTable("EstadoAlerta");
            modelBuilder.Entity<TipoSuceso>().ToTable("TipoSuceso");
            modelBuilder.Entity<ClaseSuceso>().ToTable("ClaseSuceso");
            modelBuilder.Entity<Menu>().ToTable("Menu");
            modelBuilder.Entity<Territorio>().ToTable("Territorio");
            modelBuilder.Entity<NivelGravedad>().ToTable("NivelGravedad");
            modelBuilder.Entity<EstadoIncendio>().ToTable("EstadoIncendio");
            modelBuilder.Entity<TipoMovimiento>().ToTable("TipoMovimiento");
            modelBuilder.Entity<ComparativaFecha>().ToTable("ComparativaFecha");
            modelBuilder.Entity<Medio>().ToTable("Medio");
            modelBuilder.Entity<EntradaSalida>().ToTable("EntradaSalida");
            modelBuilder.Entity<ProcedenciaDestino>().ToTable("ProcedenciaDestino");
            modelBuilder.Entity<Pais>().ToTable("Pais");
            modelBuilder.Entity<EstadoSuceso>().ToTable("EstadoSuceso");
            modelBuilder.Entity<TipoRegistro>().ToTable("TipoRegistro");
            modelBuilder.Entity<ImpactoClasificado>().ToTable("ImpactoClasificado");
            modelBuilder.Entity<CaracterMedio>().ToTable("CaracterMedio");
            modelBuilder.Entity<ClasificacionMedio>().ToTable("ClasificacionMedio");
            modelBuilder.Entity<TitularidadMedio>().ToTable("TitularidadMedio");
            modelBuilder.Entity<TipoEntidadTitularidadMedio>().ToTable("TipoEntidadTitularidadMedio");
            modelBuilder.Entity<TipoDireccionEmergencia>().ToTable("TipoDireccionEmergencia");
            modelBuilder.Entity<TipoPlan>().ToTable("TipoPlan");
            modelBuilder.Entity<ValidacionImpactoClasificado>().ToTable("ValidacionImpactoClasificado");
            modelBuilder.Entity<TipoDanio>().ToTable("TipoDanio");
            modelBuilder.Entity<SuperficieFiltro>().ToTable(nameof(SuperficieFiltro));
            modelBuilder.Entity<SituacionOperativa>().ToTable(nameof(SituacionOperativa));
            modelBuilder.Entity<SucesoRelacionado>().ToTable(nameof(SucesoRelacionado));
            modelBuilder.Entity<Archivo>().ToTable(nameof(Archivo));
        }


        public DbSet<Alerta>? Alertas { get; set; }
        public DbSet<EstadoAlerta>? EstadosAlertas { get; set; }
        public DbSet<TipoSuceso> TiposSuceso { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Ccaa>? CCAA { get; set; }
        public DbSet<Territorio>? Territorios { get; set; }
        public DbSet<Provincia>? Provincias { get; set; }
        public DbSet<Municipio>? Municipios { get; set; }
        public DbSet<Suceso> Sucesos { get; set; }

        public DbSet<ClaseSuceso> ClasesSucesos { get; set; }
        public DbSet<Incendio> Incendios { get; set; }
        public DbSet<NivelGravedad> NivelesGravedad { get; set; }
        public DbSet<EstadoIncendio> EstadosIncendio { get; set; }
        public DbSet<TipoMovimiento> TipoMovimientos { get; set; }
        public DbSet<ComparativaFecha> ComparativaFechas { get; set; }
        public DbSet<Medio> Medios { get; set; }
        public DbSet<Pais> Paises { get; set; }
        public DbSet<ProcedenciaDestino> ProcedenciaDestinos { get; set; }
        public DbSet<EstadoSuceso> EstadosSucesos { get; set; }

        public DbSet<EntradaSalida> EntradasSalidas { get; set; }
        public DbSet<ImpactoClasificado> ImpactosClasificados { get; set; }
        public DbSet<ImpactoEvolucion> ImpactosEvoluciones {  get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<TipoRegistro> TiposRegistros { get; set; }
        public DbSet<Evolucion> Evoluciones { get; set; }
        public DbSet<EvolucionProcedenciaDestino> EvolucionesProcedenciaDestinos { get; set; }
        public DbSet<CaracterMedio> CaracterMedios { get; set; }
        public DbSet<ClasificacionMedio> ClasificacionMedios { get; set; }
        public DbSet<TitularidadMedio> TitularidadMedios { get; set; }
        public DbSet<TipoEntidadTitularidadMedio> tipoEntidadTitularidadMedios { get; set; }
        public DbSet<TipoIntervencionMedio> TipoIntervencionMedios { get; set; }
        public DbSet<TipoDireccionEmergencia> TipoDireccionEmergencias { get; set; }

        public DbSet<TipoPlan> TipoPlanes { get; set; }
        public DbSet<ActivacionPlanEmergencia> ActivacionPlanesEmergencias { get; set; }
        public DbSet<DireccionCoordinacionEmergencia>  DireccionCoordinacionEmergencias { get; set; }
        public DbSet<ValidacionImpactoClasificado> ValidacionImpactoClasificados { get; set; }

        public DbSet<TipoDanio> TipoDanios { get; set; }

        public DbSet<OtraInformacion> OtrasInformaciones { get; set; }
        public DbSet<DetalleOtraInformacion> DetallesOtraInformacion { get; set; }
        public DbSet<DetalleOtraInformacion_ProcedenciaDestino> DetallesOtraInformacion_ProcedenciaDestinos { get; set; }

        public DbSet<SucesoRelacionado> SucesosRelacionados { get; set; }

    }
}
