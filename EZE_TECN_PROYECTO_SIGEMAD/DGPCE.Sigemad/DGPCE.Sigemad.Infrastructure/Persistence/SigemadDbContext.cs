using DGPCE.Sigemad.Application.Contracts.Identity;
using DGPCE.Sigemad.Domain.Common;
using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection;

namespace DGPCE.Sigemad.Infrastructure.Persistence
{
    public class SigemadDbContext : DbContext
    {
        private readonly IAuthService _authService;

        public SigemadDbContext(DbContextOptions<SigemadDbContext> options, IAuthService authService) : base(options)
        {
            _authService = authService;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.FechaCreacion = DateTime.Now;
                        entry.Entity.CreadoPor = _authService.GetCurrentUserId();
                        break;

                    case EntityState.Modified:
                        entry.Entity.FechaModificacion = DateTime.Now;
                        entry.Entity.ModificadoPor = _authService.GetCurrentUserId();
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.Entity.Borrado = true;
                        entry.Entity.FechaEliminacion = DateTime.Now;
                        entry.Entity.EliminadoPor = _authService.GetCurrentUserId();
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);

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
            modelBuilder.Entity<TipoDocumento>().ToTable(nameof(TipoDocumento));
            modelBuilder.Entity<AmbitoPlan>().ToTable(nameof(AmbitoPlan));
            modelBuilder.Entity<SituacionEquivalente>().ToTable(nameof(SituacionEquivalente));
            modelBuilder.Entity<EstadoMovilizacion>().ToTable(nameof(EstadoMovilizacion));
            modelBuilder.Entity<TipoCapacidad>().ToTable(nameof(TipoCapacidad));
            modelBuilder.Entity<GrupoMedio>().ToTable(nameof(GrupoMedio));

            // PCD
            modelBuilder.Entity<OpePeriodo>().ToTable(nameof(OpePeriodo));
            // FIN PCD
        }

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
        public DbSet<AreaAfectada> AreaAfectadas { get; set; }
        public DbSet<IntervencionMedio> IntervencionMedios { get; set; }
        public DbSet<RegistroProcedenciaDestino> RegistroProcedenciasDestinos { get; set; }
        public DbSet<CaracterMedio> CaracterMedios { get; set; }
        public DbSet<ClasificacionMedio> ClasificacionMedios { get; set; }
        public DbSet<TitularidadMedio> TitularidadMedios { get; set; }
        public DbSet<TipoEntidadTitularidadMedio> tipoEntidadTitularidadMedios { get; set; }
        public DbSet<TipoIntervencionMedio> TipoIntervencionMedios { get; set; }
        public DbSet<TipoDireccionEmergencia> TipoDireccionEmergencias { get; set; }

        public DbSet<TipoPlan> TipoPlanes { get; set; }
        public DbSet<ActivacionPlanEmergencia> ActivacionPlanesEmergencias { get; set; }
        public DbSet<DireccionCoordinacionEmergencia>  DireccionCoordinacionEmergencias { get; set; }
        public DbSet<Direccion> Direccions { get; set; }
        public DbSet<CoordinacionCecopi> CoordinacionCecopis { get; set; }
        public DbSet<CoordinacionPMA> CoordinacionPMAs { get; set; }

        public DbSet<ValidacionImpactoClasificado> ValidacionImpactoClasificados { get; set; }

        public DbSet<TipoDanio> TipoDanios { get; set; }

        public DbSet<OtraInformacion> OtrasInformaciones { get; set; }
        public DbSet<DetalleOtraInformacion> DetallesOtraInformacion { get; set; }
        public DbSet<DetalleOtraInformacion_ProcedenciaDestino> DetallesOtraInformacion_ProcedenciaDestinos { get; set; }

        public DbSet<SucesoRelacionado> SucesosRelacionados { get; set; }
        public DbSet<FaseEmergencia> FasesEmergencia { get; set; }
        public DbSet<Registro> Registros { get; set; }

        public DbSet<Parametro> Parametro { get; set; }
        public DbSet<DatoPrincipal> DatoPrincipal { get; set; }

        public DbSet<TipoDocumento> TipoDocumentos { get; set; }
        public DbSet<Documentacion> Documentaciones { get; set; }
        public DbSet<DetalleDocumentacion> DetalleDocumentaciones { get; set; }
        public DbSet<DocumentacionProcedenciaDestino> DocumentacionProcedenciaDestinos { get; set; }
        public DbSet<AmbitoPlan> AmbitoPlanes { get; set; }
        public DbSet<TipoRiesgo> TipoRiesgos { get; set; }
        public DbSet<PlanEmergencia> PlanesEmergencias { get; set; }
        public DbSet<ModoActivacion> ModosActivacion { get; set; }

        public DbSet<TipoSistemaEmergencia> TiposSistemasEmergencias { get; set; }

        public DbSet<PlanSituacion> PlanesSituaciones { get; set; }

        public DbSet<ActuacionRelevanteDGPCE> ActuacionesRelevantesDGPCE { get; set; }

        public DbSet<EmergenciaNacional> EmergenciasNacionales { get; set; }
        public DbSet<SituacionEquivalente> SituacionEquivalentes { get; set; }

        public DbSet<DeclaracionZAGEP> DeclaracionesZAGEP { get; set; }

        public DbSet<ActivacionSistema> ActivacionesSistemas { get; set; }

        public DbSet<ConvocatoriaCECOD> ConvocatoriasCECOD { get; set; }

        public DbSet<NotificacionEmergencia> NotificacionesEmergencias { get; set; }

        public DbSet<TipoNotificacion> TiposNotificaciones { get; set; }
        public DbSet<TipoRegistroActualizacion> TipoRegistroActualizaciones { get; set; }
        public DbSet<ApartadoRegistro> ApartadosRegistro { get; set; }
        public DbSet<RegistroActualizacion> RegistrosActualizacion { get; set; }
        public DbSet<RegistroApartado> RegistrosApartados { get; set; }
        public DbSet<DetalleRegistroActualizacion> DetallesRegistroActualizacion { get; set; }
        public DbSet<HistorialCambios> HistorialCambios { get; set; }
        public DbSet<MediosCapacidad> MediosCapacidads { get; set; }



        // PCD
        public DbSet<OpePeriodo> OpePeriodos { get; set; }
        // FIN PCD
    }
}
