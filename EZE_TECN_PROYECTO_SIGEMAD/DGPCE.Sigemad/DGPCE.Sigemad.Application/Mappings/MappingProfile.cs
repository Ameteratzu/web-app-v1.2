using AutoMapper;
using DGPCE.Sigemad.Application.Dtos.AreasAfectadas;
using DGPCE.Sigemad.Application.Dtos.CoordinacionCecopis;
using DGPCE.Sigemad.Application.Dtos.CoordinacionesPMA;
using DGPCE.Sigemad.Application.Dtos.DetallesDocumentaciones;
using DGPCE.Sigemad.Application.Dtos.DireccionCoordinaciones;
using DGPCE.Sigemad.Application.Dtos.Direcciones;
using DGPCE.Sigemad.Application.Dtos.Documentaciones;
using DGPCE.Sigemad.Application.Dtos.EntidadesMenor;
using DGPCE.Sigemad.Application.Dtos.Evoluciones;
using DGPCE.Sigemad.Application.Dtos.Impactos;
using DGPCE.Sigemad.Application.Dtos.Municipios;
using DGPCE.Sigemad.Application.Dtos.OtraInformaciones;
using DGPCE.Sigemad.Application.Dtos.ProcedenciasDestinos;
using DGPCE.Sigemad.Application.Dtos.Provincias;
using DGPCE.Sigemad.Application.Features.ActividadesPlanesEmergencia.Vms;
using DGPCE.Sigemad.Application.Features.Alertas.Commands.CreateAlertas;
using DGPCE.Sigemad.Application.Features.Alertas.Commands.UpdateAlertas;
using DGPCE.Sigemad.Application.Features.Alertas.Vms;
using DGPCE.Sigemad.Application.Features.ApplicationUsers.Vms;
using DGPCE.Sigemad.Application.Features.Archivos.Commands.CreateFile;
using DGPCE.Sigemad.Application.Features.AreasAfectadas.Commands.UpdateAreasAfectadas;
using DGPCE.Sigemad.Application.Features.CCAA.Vms;
using DGPCE.Sigemad.Application.Features.DatosPrincipales.Commands;
using DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Commands.Create;
using DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Commands.Update;
using DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Vms;
using DGPCE.Sigemad.Application.Features.Distritos.Vms;
using DGPCE.Sigemad.Application.Features.Documentaciones.ManageDocumentaciones;
using DGPCE.Sigemad.Application.Features.Documentaciones.Vms;
using DGPCE.Sigemad.Application.Features.EntidadesMenores.Vms;
using DGPCE.Sigemad.Application.Features.EstadosAlertas.Commands.CreateAlertas;
using DGPCE.Sigemad.Application.Features.EstadosAlertas.Commands.UpdateAlertas;
using DGPCE.Sigemad.Application.Features.EstadosAlertas.Vms;
using DGPCE.Sigemad.Application.Features.Evoluciones.Commands.ManageEvoluciones;
using DGPCE.Sigemad.Application.Features.Evoluciones.Vms;
using DGPCE.Sigemad.Application.Features.EvolucionProcedenciaDestinos.Vms;
using DGPCE.Sigemad.Application.Features.ImpactosClasificados.Vms;
using DGPCE.Sigemad.Application.Features.ImpactosEvoluciones.Commands.CreateImpactoEvoluciones;
using DGPCE.Sigemad.Application.Features.ImpactosEvoluciones.Commands.UpdateImpactoEvoluciones;
using DGPCE.Sigemad.Application.Features.Incendios.Commands.CreateIncendios;
using DGPCE.Sigemad.Application.Features.Incendios.Commands.UpdateIncendios;
using DGPCE.Sigemad.Application.Features.Incendios.Vms;
using DGPCE.Sigemad.Application.Features.IntervencionesMedios.Commands.CreateIntervencionMedios;
using DGPCE.Sigemad.Application.Features.IntervencionesMedios.Commands.UpdateIntervencionMedios;
using DGPCE.Sigemad.Application.Features.Menus.Vms;
using DGPCE.Sigemad.Application.Features.Municipios.Vms;
using DGPCE.Sigemad.Application.Features.OtrasInformaciones.Commands.CreateOtrasInformaciones;
using DGPCE.Sigemad.Application.Features.OtrasInformaciones.Vms;
using DGPCE.Sigemad.Application.Features.Parametros.Commands;
using DGPCE.Sigemad.Application.Features.Provincias.Vms;
using DGPCE.Sigemad.Application.Features.Registros.Command.CreateRegistros;
using DGPCE.Sigemad.Application.Features.Sucesos.Vms;
using DGPCE.Sigemad.Application.Features.SucesosRelacionados.Commands.CreateSucesosRelacionados;
using DGPCE.Sigemad.Application.Features.SucesosRelacionados.Vms;
using DGPCE.Sigemad.Application.Features.Territorios.Vms;
using DGPCE.Sigemad.Application.Features.TipoIntervencionMedios.Vms;
using DGPCE.Sigemad.Application.Features.ValidacionesImpacto.Vms;
using DGPCE.Sigemad.Application.Specifications.Incendios;
using DGPCE.Sigemad.Application.Specifications.Sucesos;
using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateAlertaCommand, Alerta>();
        CreateMap<UpdateAlertaCommand, Alerta>();
        CreateMap<CreateEstadoAlertaCommand, EstadoAlerta>();
        CreateMap<UpdateEstadoAlertaCommand, EstadoAlerta>();

        CreateMap<Alerta, AlertaVm>();
        CreateMap<EstadoAlerta, EstadosAlertasVm>();

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

        CreateMap<CreateIntervencionMedioCommand, IntervencionMedio>();
        CreateMap<UpdateIntervencionMedioCommand, IntervencionMedio>();

        CreateMap<Distrito, DistritoVm>();

        CreateMap<EntidadMenor, EntidadMenorVm>();
        CreateMap<EntidadMenor, EntidadMenorDto>();


        // Direccion y Coordinacion de Emergencia
        CreateMap<DireccionCoordinacionEmergencia, DireccionCoordinacionEmergenciaVm>();
        CreateMap<DireccionCoordinacionEmergencia, DireccionCoordinacionEmergenciaDto>();
        CreateMap<CreateOrUpdateDireccionDto, Direccion>();
        CreateMap<CreateOrUpdateCoordinacionCecopiDto, CoordinacionCecopi>();
        CreateMap<CreateOrUpdateCoordinacionPmaDto, CoordinacionPMA>();
        CreateMap<ActivacionPlanEmergencia, ActivacionPlanEmergenciaVm>();
        CreateMap<Direccion, DireccionDto>();
        CreateMap<CoordinacionCecopi, CoordinacionCecopiDto>();
        CreateMap<CoordinacionPMA, CoordinacionPMADto>();




        CreateMap<CreateOrUpdateAreaAfectadaDto, AreaAfectada>();
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
            .ForMember(dest => dest.IdIncendio, opt => opt.MapFrom(src => src.IdIncendio));

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

        CreateMap<SucesoRelacionado, SucesoRelacionadoVm>();
        CreateMap<CreateSucesoRelacionadoCommand, SucesoRelacionado>();
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
             .ForMember(dest => dest.DetallesDocumentacion, opt => opt.MapFrom(src => src.DetallesDocumentacion));

        CreateMap<DetalleDocumentacionDto, DetalleDocumentacion>()
           .ForMember(dest => dest.DocumentacionProcedenciaDestinos, opt => opt.MapFrom(src => src.DocumentacionProcedenciasDestinos.Select(id => new DocumentacionProcedenciaDestino { IdProcedenciaDestino = id }).ToList()));

        CreateMap<DetalleDocumentacion, DetalleDocumentacionBusquedaDto>()
                .ForMember(dest => dest.ProcedenciaDestinos, opt => opt.MapFrom(src => src.DocumentacionProcedenciaDestinos.Select(p => p.ProcedenciaDestino)));

        CreateMap<SucesosSpecificationParams, IncendiosSpecificationParams>()
             .ForMember(dest => dest.Search, opt => opt.MapFrom(src => src.Denominacion));

        CreateMap<Incendio, SucesosBusquedaVm>()
            .ForMember(dest => dest.FechaHora, opt => opt.MapFrom(src => src.FechaInicio))
            .ForMember(dest => dest.TipoSuceso, opt => opt.MapFrom(src => src.Suceso.TipoSuceso.Descripcion))
            .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.EstadoSuceso.Descripcion))
            .ForMember(dest => dest.Denominacion, opt => opt.MapFrom(src => src.Denominacion));

    }

    /*
    private ICollection<RegistroProcedenciaDestino> MapEvolucionProcedenciaDestinos(ICollection<int>? source)
    {
        if (source == null)
        {
            return new List<RegistroProcedenciaDestino>();
        }

        return source.Select(id => new RegistroProcedenciaDestino { IdProcedenciaDestino = id }).ToList();
    }
    */
}
