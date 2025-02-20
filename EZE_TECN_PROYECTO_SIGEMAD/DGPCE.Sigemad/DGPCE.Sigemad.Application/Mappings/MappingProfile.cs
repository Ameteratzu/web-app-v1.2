using AutoMapper;
using DGPCE.Sigemad.Application.Dtos.ActivacionesPlanes;
using DGPCE.Sigemad.Application.Dtos.ActivacionSistema;
using DGPCE.Sigemad.Application.Dtos.ActuacionesRelevantes;
using DGPCE.Sigemad.Application.Dtos.Archivos;
using DGPCE.Sigemad.Application.Dtos.AreasAfectadas;
using DGPCE.Sigemad.Application.Dtos.CaracterMedios;
using DGPCE.Sigemad.Application.Dtos.ConvocatoriasCECOD;
using DGPCE.Sigemad.Application.Dtos.CoordinacionCecopis;
using DGPCE.Sigemad.Application.Dtos.CoordinacionesPMA;
using DGPCE.Sigemad.Application.Dtos.DeclaracionesZAGEP;
using DGPCE.Sigemad.Application.Dtos.DetallesDocumentaciones;
using DGPCE.Sigemad.Application.Dtos.DireccionCoordinaciones;
using DGPCE.Sigemad.Application.Dtos.Direcciones;
using DGPCE.Sigemad.Application.Dtos.Documentaciones;
using DGPCE.Sigemad.Application.Dtos.EmergenciasNacionales;
using DGPCE.Sigemad.Application.Dtos.EntidadesMenor;
using DGPCE.Sigemad.Application.Dtos.Evoluciones;
using DGPCE.Sigemad.Application.Dtos.Impactos;
using DGPCE.Sigemad.Application.Dtos.IntervencionMedios;
using DGPCE.Sigemad.Application.Dtos.MovilizacionesMedios;
using DGPCE.Sigemad.Application.Dtos.MovilizacionesMedios.Pasos;
using DGPCE.Sigemad.Application.Dtos.Municipios;
using DGPCE.Sigemad.Application.Dtos.NotificacionesEmergencias;
using DGPCE.Sigemad.Application.Dtos.OtraInformaciones;
using DGPCE.Sigemad.Application.Dtos.ProcedenciasDestinos;
using DGPCE.Sigemad.Application.Dtos.Provincias;
using DGPCE.Sigemad.Application.Dtos.SituacionesEquivalentes;
using DGPCE.Sigemad.Application.Features.ActivacionesPlanesEmergencia.Vms;
using DGPCE.Sigemad.Application.Features.ActivacionesSistemas.Commands.ManageActivacionSistema;
using DGPCE.Sigemad.Application.Features.ApplicationUsers.Vms;
using DGPCE.Sigemad.Application.Features.Archivos.Commands.CreateFile;
using DGPCE.Sigemad.Application.Features.AreasAfectadas.Commands.UpdateAreasAfectadas;
using DGPCE.Sigemad.Application.Features.CCAA.Vms;
using DGPCE.Sigemad.Application.Features.ConvocatoriasCECOD.Commands;
using DGPCE.Sigemad.Application.Features.DatosPrincipales.Commands;
using DGPCE.Sigemad.Application.Features.DeclaracionesZAGEP.Commands.ManageDeclaracionesZAGEP;
using DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Commands.Create;
using DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Commands.Update;
using DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Vms;
using DGPCE.Sigemad.Application.Features.Distritos.Vms;
using DGPCE.Sigemad.Application.Features.Documentaciones.Vms;
using DGPCE.Sigemad.Application.Features.EmergenciasNacionales.Commands.ManageEmergenciasNacionales;
using DGPCE.Sigemad.Application.Features.EntidadesMenores.Vms;
using DGPCE.Sigemad.Application.Features.Evoluciones.Commands.ManageEvoluciones;
using DGPCE.Sigemad.Application.Features.Evoluciones.Vms;
using DGPCE.Sigemad.Application.Features.EvolucionProcedenciaDestinos.Vms;
using DGPCE.Sigemad.Application.Features.Fases.Vms;
using DGPCE.Sigemad.Application.Features.ImpactosClasificados.Vms;
using DGPCE.Sigemad.Application.Features.ImpactosEvoluciones.Commands.CreateImpactoEvoluciones;
using DGPCE.Sigemad.Application.Features.ImpactosEvoluciones.Commands.UpdateImpactoEvoluciones;
using DGPCE.Sigemad.Application.Features.Incendios.Commands.CreateIncendios;
using DGPCE.Sigemad.Application.Features.Incendios.Commands.UpdateIncendios;
using DGPCE.Sigemad.Application.Features.Incendios.Vms;
using DGPCE.Sigemad.Application.Features.Menus.Vms;
using DGPCE.Sigemad.Application.Features.Municipios.Vms;
using DGPCE.Sigemad.Application.Features.NotificacionesEmergencias.Commands.ManageNotificacionEmergencia;
using DGPCE.Sigemad.Application.Features.OtrasInformaciones.Commands.CreateOtrasInformaciones;
using DGPCE.Sigemad.Application.Features.OtrasInformaciones.Vms;
using DGPCE.Sigemad.Application.Features.Parametros.Commands;
using DGPCE.Sigemad.Application.Features.PlanesEmergencias.Vms;
using DGPCE.Sigemad.Application.Features.PlanesSituaciones.Vms;
using DGPCE.Sigemad.Application.Features.Provincias.Vms;
using DGPCE.Sigemad.Application.Features.Registros.Command.CreateRegistros;
using DGPCE.Sigemad.Application.Features.Sucesos.Vms;
using DGPCE.Sigemad.Application.Features.SucesosRelacionados.Vms;
using DGPCE.Sigemad.Application.Features.Territorios.Vms;
using DGPCE.Sigemad.Application.Features.TipoIntervencionMedios.Vms;
using DGPCE.Sigemad.Application.Features.ValidacionesImpacto.Vms;
using DGPCE.Sigemad.Application.Mappings.Resolvers;
using DGPCE.Sigemad.Application.Specifications.Incendios;
using DGPCE.Sigemad.Application.Specifications.Sucesos;
using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Menu, MenuItemVm>();

        CreateMap<Ccaa, ComunidadesAutonomasSinProvinciasVm>();
        CreateMap<Ccaa, ComunidadesAutonomasVm>()
                .ForMember(dest => dest.Provincia, opt => opt.MapFrom(src => src.Provincia.ToList()));

        CreateMap<Provincia, ProvinciaSinMunicipiosVm>();
        CreateMap<Provincia, ProvinciaSinMunicipiosConIdComunidadVm>();
        CreateMap<Provincia, ProvinciaDto>();

        CreateMap<Municipio, MunicipioSinIdProvinciaVm>();
        CreateMap<Municipio, MunicipioConIdProvincia>();
        CreateMap<Municipio, MunicipioDto>();

        CreateMap<CreateIncendioCommand, Incendio>();

        CreateMap<UpdateIncendioCommand, Incendio>()
           .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<Incendio, IncendioVm>();

        // EVOLUCION
        CreateMap<Evolucion, EvolucionVm>();
        CreateMap<Evolucion, EvolucionDto>();
        CreateMap<Registro, RegistroEvolucionDto>()
            .ForMember(dest => dest.ProcedenciaDestinos, opt => opt.MapFrom(src => src.ProcedenciaDestinos.Select(p => p.ProcedenciaDestino)));
        CreateMap<ProcedenciaDestino, ProcedenciaDto>();
        CreateMap<DatoPrincipal, DatoPrincipalEvolucionDto>();
        CreateMap<Parametro, ParametroEvolucionDto>();


        //TODO: CORREGIR PORQUE SE CAMBIO TABLAS DE EVOLUCIONES
        //CreateMap<UpdateEvolucionCommand, Evolucion>()
        //  .ForMember(dest => dest.EvolucionProcedenciaDestinos, opt => opt.MapFrom(src => MapEvolucionProcedenciaDestinos(src.EvolucionProcedenciaDestinos)));


        CreateMap<ManageEvolucionCommand, Evolucion>();
        CreateMap<CreateRegistroCommand, Registro>()
            .ForMember(dest => dest.ProcedenciaDestinos,
            opt => opt.MapFrom(src => src.RegistroProcedenciasDestinos.Select(p => new RegistroProcedenciaDestino { IdProcedenciaDestino = p })));



        CreateMap<ApplicationUser, ApplicationUserVm>();

        CreateMap<CreateImpactoEvolucionCommand, ImpactoEvolucion>();
        CreateMap<ManageImpactoDto, ImpactoEvolucion>();
        CreateMap<UpdateImpactoEvolucionCommand, ImpactoEvolucion>();
        CreateMap<UpdateImpactoEvolucionDto, ImpactoEvolucion>();
        CreateMap<ImpactoEvolucion, ImpactoEvolucionDto>();
        CreateMap<ImpactoClasificado, ImpactoClasificadoDescripcionVm>();

        CreateMap<TipoIntervencionMedio, TipoIntervencionMedioVm>();

        CreateMap<Distrito, DistritoVm>();

        CreateMap<EntidadMenor, EntidadMenorVm>();
        CreateMap<EntidadMenor, EntidadMenorDto>();


        // Direccion y Coordinacion de Emergencia
        CreateMap<DireccionCoordinacionEmergencia, DireccionCoordinacionEmergenciaVm>();
        CreateMap<DireccionCoordinacionEmergencia, DireccionCoordinacionEmergenciaDto>();
        CreateMap<CreateOrUpdateDireccionDto, Direccion>();
        CreateMap<CreateOrUpdateDireccionDto, DireccionDto>();
        CreateMap<CreateOrUpdateCoordinacionCecopiDto, CoordinacionCecopi>();
        CreateMap<CreateOrUpdateCoordinacionPmaDto, CoordinacionPMA>();
        CreateMap<ActivacionPlanEmergencia, ActivacionPlanEmergenciaVm>();
        CreateMap<Direccion, DireccionDto>();
        CreateMap<Direccion, CreateOrUpdateDireccionDto>();


        CreateMap<CoordinacionCecopi, CoordinacionCecopiDto>();
        CreateMap<CoordinacionCecopi, CreateOrUpdateCoordinacionCecopiDto>();

        CreateMap<CoordinacionPMA, CoordinacionPMADto>();
        CreateMap<CoordinacionPMA, CreateOrUpdateCoordinacionPmaDto>();


        CreateMap<ManageIntervencionMedioDto, IntervencionMedio>()
            .ForMember(dest => dest.DetalleIntervencionMedios, opt => opt.MapFrom<CustomDetalleIntervencionMedioResolver>());

        CreateMap<ManageDetalleIntervencionMedioDto, DetalleIntervencionMedio>();

        CreateMap<IntervencionMedio, ManageIntervencionMedioDto>();
        CreateMap<DetalleIntervencionMedio, ManageDetalleIntervencionMedioDto>();


        CreateMap<CreateOrUpdateAreaAfectadaDto, AreaAfectada>();
        CreateMap<AreaAfectada, CreateOrUpdateAreaAfectadaDto>();

        CreateMap<UpdateAreaAfectadaDto, AreaAfectada>();
        CreateMap<AreaAfectada, AreaAfectadaDto>();
        CreateMap<UpdateAreaAfectadaCommand, AreaAfectada>();

        CreateMap<ValidacionImpactoClasificado, ValidacionImpactoClasificadoVm>()
            .ForMember(dest => dest.Label, opt => opt.MapFrom(src => src.Etiqueta));

        CreateMap<Territorio, TerritorioVm>();
        CreateMap<CreateDireccionCoordinacionEmergenciasCommand, DireccionCoordinacionEmergencia>();
        CreateMap<UpdateDireccionCoordinacionEmergenciaCommand, DireccionCoordinacionEmergencia>();

        // Otra informacion
        CreateMap<CreateOtraInformacionCommand, OtraInformacion>();
        CreateMap<OtraInformacion, OtraInformacionVm>()
            .ForMember(dest => dest.IdOtraInformacion, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.IdSuceso, opt => opt.MapFrom(src => src.IdSuceso));

        CreateMap<OtraInformacion, OtraInformacionDto>()
            .ForMember(dest => dest.Lista, opt => opt.MapFrom(src => src.DetallesOtraInformacion));

        CreateMap<DetalleOtraInformacion, DetalleOtraInformacionDto>()
            .ForMember(dest => dest.ProcedenciasDestinos, opt => opt.MapFrom(src => src.ProcedenciasDestinos.Select(pd => pd.ProcedenciaDestino)));


        CreateMap<DetalleOtraInformacion, OtraInformacionVm>()
            .ForMember(dest => dest.FechaHora, opt => opt.MapFrom(src => src.FechaHora))
            .ForMember(dest => dest.IdMedio, opt => opt.MapFrom(src => src.IdMedio))
            .ForMember(dest => dest.Asunto, opt => opt.MapFrom(src => src.Asunto))
            .ForMember(dest => dest.Observaciones, opt => opt.MapFrom(src => src.Observaciones))
            .ForMember(dest => dest.IdsProcedenciaDestino, opt => opt.MapFrom(src => src.ProcedenciasDestinos.Select(pd => pd.IdProcedenciaDestino).ToList()));

        CreateMap<CreateDetalleOtraInformacionDto, DetalleOtraInformacion>()
            .ForMember(dest => dest.ProcedenciasDestinos, opt => opt.MapFrom(src => src.IdsProcedenciasDestinos.Select(id => new DetalleOtraInformacion_ProcedenciaDestino { IdProcedenciaDestino = id }).ToList()));

        CreateMap<RegistroProcedenciaDestino, RegistroProcedenciaDestinoVm>();


        CreateMap<ManageEmergenciasNacionalesCommand, ActuacionRelevanteDGPCE>()
          .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdActuacionRelevante));

        CreateMap<ManageEmergenciaNacionalDto, EmergenciaNacional>();


        CreateMap<ManageDeclaracionesZAGEPCommand, ActuacionRelevanteDGPCE>()
       .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdActuacionRelevante))
       .ForMember(dest => dest.DeclaracionesZAGEP, opt => opt.MapFrom(src => src.Detalles));

        CreateMap<ManageDeclaracionZAGEPDto, DeclaracionZAGEP>();





        CreateMap<SucesoRelacionado, SucesoRelacionadoVm>();
        CreateMap<CreateFileCommand, Archivo>();
        CreateMap<CreateRegistroCommand, Registro>()
            .ForMember(dest => dest.ProcedenciaDestinos, opt => opt.Ignore())
            .AfterMap((src, dest) =>
            {
                var ids = src.RegistroProcedenciasDestinos.ToHashSet();

                // Eliminar los registros que no están en la lista de IDs
                dest.ProcedenciaDestinos.RemoveAll(pd => !ids.Contains(pd.IdProcedenciaDestino));

                // Actualizar o agregar los registros
                foreach (var id in ids)
                {
                    var existing = dest.ProcedenciaDestinos.FirstOrDefault(pd => pd.IdProcedenciaDestino == id);
                    if (existing == null)
                    {
                        dest.ProcedenciaDestinos.Add(new RegistroProcedenciaDestino { IdProcedenciaDestino = id });
                    }
                    else if (existing.Borrado)
                    {
                        existing.Borrado = false;
                    }
                }
            });


        CreateMap<CreateParametroCommand, Parametro>();

        CreateMap<CreateDatoPrincipalCommand, DatoPrincipal>();
        CreateMap<Documentacion, DocumentacionVm>()
           .ForMember(dest => dest.DetalleDocumentaciones, opt => opt.MapFrom(src => src.DetallesDocumentacion));

        CreateMap<Documentacion, DocumentacionDto>()
             .ForMember(dest => dest.Detalles, opt => opt.MapFrom(src => src.DetallesDocumentacion));


        CreateMap<DetalleDocumentacion, DetalleDocumentacionDto>()
                .ForMember(dest => dest.IdsProcedenciasDestinos, opt => opt.MapFrom(src => src.DocumentacionProcedenciaDestinos.Select(p => p.ProcedenciaDestino.Id))); ;

        CreateMap<DetalleDocumentacionDto, DetalleDocumentacion>()
            .ForMember(dest => dest.IdArchivo, opt => opt.Ignore())
            .ForMember(dest => dest.Archivo, opt => opt.Ignore())
            .ForMember(dest => dest.DocumentacionProcedenciaDestinos, opt => opt.Ignore())
            .AfterMap((src, dest, context) =>
            {
                var existingIds = dest.DocumentacionProcedenciaDestinos.Select(dpd => dpd.IdProcedenciaDestino).ToList();
                var newIds = src.IdsProcedenciasDestinos ?? new List<int>();

                // Eliminar los que no están en la nueva lista
                var toRemove = dest.DocumentacionProcedenciaDestinos.Where(dpd => !newIds.Contains(dpd.IdProcedenciaDestino)).ToList();
                foreach (var item in toRemove)
                {
                    dest.DocumentacionProcedenciaDestinos.Remove(item);
                }

                // Agregar o reactivar los nuevos que no están en la lista existente
                foreach (var id in newIds)
                {
                    var existing = dest.DocumentacionProcedenciaDestinos.FirstOrDefault(dpd => dpd.IdProcedenciaDestino == id);
                    if (existing == null)
                    {
                        dest.DocumentacionProcedenciaDestinos.Add(new DocumentacionProcedenciaDestino
                        {
                            IdProcedenciaDestino = id,
                            IdDetalleDocumentacion = dest.Id
                        });
                    }
                    else if (existing.Borrado)
                    {
                        existing.Borrado = false;
                    }
                }
            });



        CreateMap<DetalleDocumentacion, DetalleDocumentacionBusquedaDto>()
                .ForMember(dest => dest.ProcedenciaDestinos, opt => opt.MapFrom(src => src.DocumentacionProcedenciaDestinos.Select(p => p.ProcedenciaDestino)));

        CreateMap<DetalleDocumentacion, ItemDocumentacionDto>()
            .ForMember(dest => dest.ProcedenciaDestinos, opt => opt.MapFrom(src => src.DocumentacionProcedenciaDestinos.Where(p => p.Borrado == false).Select(p => new ProcedenciaDto { Id = p.ProcedenciaDestino.Id, Descripcion = p.ProcedenciaDestino.Descripcion })));

        CreateMap<SucesosSpecificationParams, IncendiosSpecificationParams>()
             .ForMember(dest => dest.Search, opt => opt.MapFrom(src => src.Denominacion));

        CreateMap<Incendio, SucesosBusquedaVm>()
            .ForMember(dest => dest.FechaHora, opt => opt.MapFrom(src => src.FechaInicio))
            .ForMember(dest => dest.TipoSuceso, opt => opt.MapFrom(src => src.Suceso.TipoSuceso.Descripcion))
            .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.EstadoSuceso.Descripcion))
            .ForMember(dest => dest.Denominacion, opt => opt.MapFrom(src => src.Denominacion));

        CreateMap<PlanEmergencia, PlanEmergenciaVm>();
        CreateMap<FaseEmergencia, FaseEmergenciaVm>();
        CreateMap<PlanSituacion, PlanSituacionVm>()
          .ForMember(dest => dest.NivelSituacion, opt => opt.MapFrom(src =>
              string.IsNullOrEmpty(src.Nivel) && string.IsNullOrEmpty(src.Situacion)
                  ? " / "
                  : (src.Nivel ?? string.Empty) + (src.Situacion ?? string.Empty)));


        CreateMap<Archivo, ArchivoDto>();

        CreateMap<SituacionEquivalente, SituacionEquivalenteDto>();

        CreateMap<CaracterMedio, CaracterMedioDto>();


        CreateMap<ManageConvocatoriaCECODCommand, ActuacionRelevanteDGPCE>()
           .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdActuacionRelevante))
           .ForMember(dest => dest.ConvocatoriasCECOD, opt => opt.MapFrom(src => src.Detalles));

        CreateMap<ManageConvocatoriaCECODDto, ConvocatoriaCECOD>();

        CreateMap<ConvocatoriaCECOD, ConvocatoriaCECODDto>();
        CreateMap<ActivacionSistema, ActivacionSistemaDto>();
        CreateMap<ActivacionPlanEmergencia, ActivacionPlanEmergenciaDto>();
        CreateMap<DeclaracionZAGEP, DeclaracionZAGEPDto>();
        CreateMap<EmergenciaNacional, EmergenciaNacionalDto>();

        CreateMap<ManageActivacionSistemaCommand, ActuacionRelevanteDGPCE>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdActuacionRelevante))
            .ForMember(dest => dest.ActivacionSistemas, opt => opt.MapFrom(src => src.Detalles));

        CreateMap<ManageActivacionSistemaDto, ActivacionSistema>();

        CreateMap<NotificacionEmergencia, NotificacionEmergenciaDto>();
        CreateMap<ManageNotificacionEmergenciaDto, NotificacionEmergencia>();

        CreateMap<ManageNotificacionEmergenciaCommand, ActuacionRelevanteDGPCE>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdActuacionRelevante))
            .ForMember(dest => dest.NotificacionesEmergencias, opt => opt.MapFrom(src => src.Detalles));

        CreateMap<ActuacionRelevanteDGPCE, ActuacionRelevanteDGPCEDto>();

        //Movilizaciones
        CreateMap<MovilizacionMedioDto, MovilizacionMedio>();

        CreateMap<MovilizacionMedio, MovilizacionMedioListaDto>()
            .ForMember(dest => dest.Pasos, opt => opt.Ignore())
            .AfterMap((src, dest, context) =>
            {
                // Filtrar los pasos que no estén marcados como borrados y mapearlos al destino
                dest.Pasos = src.Pasos
                    .Where(p => !p.Borrado) // Filtra los pasos con Borrado == false
                    .Select(p => context.Mapper.Map<EjecucionPasoDto>(p)) // Mapea cada paso al DTO correspondiente
                    .ToList();

            });


        CreateMap<EjecucionPaso, EjecucionPasoDto>();
        CreateMap<PasoMovilizacion, PasoMovilizacionDto>();

        CreateMap<ProcedenciaMedio, ProcedenciaMedioDto>();
        CreateMap<DestinoMedio, DestinoMedioDto>();

        CreateMap<ManageSolicitudMedioDto, SolicitudMedio>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.IdArchivo, opt => opt.Ignore())
            .ForMember(dest => dest.Archivo, opt => opt.Ignore());

        CreateMap<SolicitudMedio, SolicitudMedioDto>();
        CreateMap<TramitacionMedio, TramitacionMedioDto>();
        CreateMap<CancelacionMedio, CancelacionMedioDto>();
        CreateMap<AportacionMedio, AportacionMedioDto>();
        CreateMap<OfrecimientoMedio, OfrecimientoMedioDto>();
        CreateMap<DespliegueMedio, DespliegueMedioDto>();
        CreateMap<FinIntervencionMedio, FinIntervencionMedioDto>();
        CreateMap<LlegadaBaseMedio, LlegadaBaseMedioDto>();


        CreateMap<ManageTramitacionMedioDto, TramitacionMedio>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<TramitacionMedio, ManageTramitacionMedioDto>();

        CreateMap<ManageCancelacionMedioDto, CancelacionMedio>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<CancelacionMedio, ManageCancelacionMedioDto>();

        CreateMap<ManageOfrecimientoMedioDto, OfrecimientoMedio>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<OfrecimientoMedio, ManageOfrecimientoMedioDto>();

        CreateMap<ManageAportacionMedioDto, AportacionMedio>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<AportacionMedio, ManageAportacionMedioDto>();

        CreateMap<ManageDespliegueMedioDto, DespliegueMedio>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<DespliegueMedio, ManageDespliegueMedioDto>();

        CreateMap<ManageFinIntervencionMedioDto, FinIntervencionMedio>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<FinIntervencionMedio, ManageFinIntervencionMedioDto>();

        CreateMap<ManageLlegadaBaseMedioDto, LlegadaBaseMedio>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<LlegadaBaseMedio, ManageLlegadaBaseMedioDto>();

        CreateMap<TipoPlan, TipoPlanDto>();
        CreateMap<ManageActivacionPlanEmergenciaDto, ActivacionPlanEmergencia>()
            .ForMember(dest => dest.IdArchivo, opt => opt.Ignore())
            .ForMember(dest => dest.Archivo, opt => opt.Ignore());

        CreateMap<TipoAdministracion, TipoAdministracionDto>();
        CreateMap<Administracion, AdministracionDto>();
        CreateMap<Organismo, OrganismoDto>();
        CreateMap<Entidad, EntidadDto>();
        CreateMap<TipoCapacidad, TipoCapacidadDto>();
        CreateMap<Capacidad, CapacidadDto>();

    }

}
