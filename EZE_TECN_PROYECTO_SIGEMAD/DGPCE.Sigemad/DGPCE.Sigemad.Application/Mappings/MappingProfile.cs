﻿using AutoMapper;
using DGPCE.Sigemad.Application.Dtos.AreasAfectadas;
using DGPCE.Sigemad.Application.Dtos.Direcciones;
using DGPCE.Sigemad.Application.Dtos.EntidadesMenor;
using DGPCE.Sigemad.Application.Dtos.Impactos;
using DGPCE.Sigemad.Application.Dtos.Municipios;
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
using DGPCE.Sigemad.Application.Features.EntidadesMenores.Vms;
using DGPCE.Sigemad.Application.Features.EstadosAlertas.Commands.CreateAlertas;
using DGPCE.Sigemad.Application.Features.EstadosAlertas.Commands.UpdateAlertas;
using DGPCE.Sigemad.Application.Features.EstadosAlertas.Vms;
using DGPCE.Sigemad.Application.Features.Evoluciones.Commands.CreateEvoluciones;
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
using DGPCE.Sigemad.Application.Features.SucesosRelacionados.Commands.CreateSucesosRelacionados;
using DGPCE.Sigemad.Application.Features.SucesosRelacionados.Vms;
using DGPCE.Sigemad.Application.Features.Territorios.Vms;
using DGPCE.Sigemad.Application.Features.TipoIntervencionMedios.Vms;
using DGPCE.Sigemad.Application.Features.ValidacionesImpacto.Vms;
using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        //CreateMap<Video, VideosVm>();

        //CreateMap<Video, VideosWithIncludesVm>()
        //    .ForMember(p => p.DirectorNombreCompleto, x => x.MapFrom(a => a.Director!.NombreCompleto))
        //    .ForMember(p => p.StreamerNombre, x => x.MapFrom(a => a.Streamer!.Nombre))
        //    .ForMember(p => p.Actores, x => x.MapFrom(a => a.Actores));


        //CreateMap<Actor, ActorVm>();
        //CreateMap<Director, DirectorVm>();
        //CreateMap<Streamer, StreamersVm>();
        //CreateMap<CreateStreamerCommand, Streamer>();
        //CreateMap<UpdateStreamerCommand, Streamer>();
        //CreateMap<CreateDirectorCommand, Director>();

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
        CreateMap<Evolucion, EvolucionVm>();

        //TODO: CORREGIR PORQUE SE CAMBIO TABLAS DE EVOLUCIONES
        //CreateMap<UpdateEvolucionCommand, Evolucion>()
        //  .ForMember(dest => dest.EvolucionProcedenciaDestinos, opt => opt.MapFrom(src => MapEvolucionProcedenciaDestinos(src.EvolucionProcedenciaDestinos)));

        CreateMap<CreateEvolucionCommand, Evolucion>()
            .ForMember(dest => dest.RegistroProcedenciasDestinos, opt => opt.MapFrom(src => MapEvolucionProcedenciaDestinos(src.RegistroProcedenciasDestinos)));

        CreateMap<ApplicationUser, ApplicationUserVm>();

        CreateMap<CreateImpactoEvolucionCommand, ImpactoEvolucion>();
        CreateMap<CreateImpactoEvolucionDto, ImpactoEvolucion>();
        CreateMap<UpdateImpactoEvolucionCommand, ImpactoEvolucion>();
        CreateMap<UpdateImpactoEvolucionDto, ImpactoEvolucion>();
        CreateMap<ImpactoClasificado, ImpactoClasificadoDescripcionVm>();

        CreateMap<TipoIntervencionMedio, TipoIntervencionMedioVm>();

        CreateMap<CreateIntervencionMedioCommand, IntervencionMedio>();
        CreateMap<UpdateIntervencionMedioCommand, IntervencionMedio>();

        CreateMap<Distrito, DistritoVm>();

        CreateMap<EntidadMenor, EntidadMenorVm>();
        CreateMap<EntidadMenor, EntidadMenorDto>();


        // Direccion y Coordinacion de Emergencia
        CreateMap<DireccionCoordinacionEmergencia, DireccionCoordinacionEmergenciaVm>();
        CreateMap<CreateOrUpdateDireccionDto, Direccion>();
        CreateMap<ActivacionPlanEmergencia, ActivacionPlanEmergenciaVm>();


        CreateMap<CreateAreaAfectadaDto, AreaAfectada>();
        CreateMap<UpdateAreaAfectadaDto, AreaAfectada>();
        CreateMap<AreaAfectada, AreaAfectadaDto>();
        CreateMap<UpdateAreaAfectadaCommand, AreaAfectada>();

        CreateMap<ValidacionImpactoClasificado, ValidacionImpactoClasificadoVm>()
            .ForMember(dest => dest.Label, opt => opt.MapFrom(src => src.Etiqueta));

        CreateMap<Territorio, TerritorioVm>();
        CreateMap<CreateDireccionCoordinacionEmergenciasCommand, DireccionCoordinacionEmergencia>();
        CreateMap<UpdateDireccionCoordinacionEmergenciaCommand, DireccionCoordinacionEmergencia>();

        CreateMap<CreateOtraInformacionCommand, OtraInformacion>();
        CreateMap<OtraInformacion, OtraInformacionVm>()
            .ForMember(dest => dest.IdOtraInformacion, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.IdIncendio, opt => opt.MapFrom(src => src.IdIncendio));

        CreateMap<DetalleOtraInformacion, OtraInformacionVm>()
            .ForMember(dest => dest.FechaHora, opt => opt.MapFrom(src => src.FechaHora))
            .ForMember(dest => dest.IdMedio, opt => opt.MapFrom(src => src.IdMedio))
            .ForMember(dest => dest.Asunto, opt => opt.MapFrom(src => src.Asunto))
            .ForMember(dest => dest.Observaciones, opt => opt.MapFrom(src => src.Observaciones))
            .ForMember(dest => dest.IdsProcedenciaDestino, opt => opt.MapFrom(src => src.ProcedenciasDestinos.Select(pd => pd.IdProcedenciaDestino).ToList()));

        CreateMap<RegistroProcedenciaDestino, RegistroProcedenciaDestinoVm>();

        CreateMap<SucesoRelacionado, SucesoRelacionadoVm>();
        CreateMap<CreateSucesoRelacionadoCommand, SucesoRelacionado>();
        CreateMap<CreateFileCommand, Archivo>();
        CreateMap<CreateRegistroCommand, Registro>();

        CreateMap<CreateParametroCommand, Parametro>();

        CreateMap<CreateDatoPrincipalCommand, DatoPrincipal>();


    }

    private ICollection<RegistroProcedenciaDestino> MapEvolucionProcedenciaDestinos(ICollection<int>? source)
    {
        if (source == null)
        {
            return new List<RegistroProcedenciaDestino>();
        }

        return source.Select(id => new RegistroProcedenciaDestino { IdProcedenciaDestino = id }).ToList();
    }
}
