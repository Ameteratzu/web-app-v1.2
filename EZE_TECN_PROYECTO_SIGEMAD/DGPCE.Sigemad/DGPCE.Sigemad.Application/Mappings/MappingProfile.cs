using AutoMapper;
using DGPCE.Sigemad.Application.Features.ActividadesPlanesEmergencia.Vms;
using DGPCE.Sigemad.Application.Features.Alertas.Commands.CreateAlertas;
using DGPCE.Sigemad.Application.Features.Alertas.Commands.UpdateAlertas;
using DGPCE.Sigemad.Application.Features.Alertas.Vms;
using DGPCE.Sigemad.Application.Features.ApplicationUsers.Vms;
using DGPCE.Sigemad.Application.Features.Archivos.Commands.CreateFile;
using DGPCE.Sigemad.Application.Features.AreasAfectadas.Commands.CreateAreasAfectadas;
using DGPCE.Sigemad.Application.Features.AreasAfectadas.Commands.UpdateAreasAfectadas;
using DGPCE.Sigemad.Application.Features.AreasAfectadas.Vms;
using DGPCE.Sigemad.Application.Features.CCAA.Vms;
using DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Commands.Create;
using DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Commands.Update;
using DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Vms;
using DGPCE.Sigemad.Application.Features.Distritos.Vms;
using DGPCE.Sigemad.Application.Features.EntidadesMenores.Vms;
using DGPCE.Sigemad.Application.Features.EstadosAlertas.Commands.CreateAlertas;
using DGPCE.Sigemad.Application.Features.EstadosAlertas.Commands.UpdateAlertas;
using DGPCE.Sigemad.Application.Features.EstadosAlertas.Vms;
using DGPCE.Sigemad.Application.Features.Evoluciones.Commands.CreateEvoluciones;
using DGPCE.Sigemad.Application.Features.Evoluciones.Commands.UpdateEvoluciones;
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
using DGPCE.Sigemad.Application.Features.Provincias.Vms;
using DGPCE.Sigemad.Application.Features.SucesosRelacionados.Commands.CreateSucesosRelacionados;
using DGPCE.Sigemad.Application.Features.SucesosRelacionados.Vms;
using DGPCE.Sigemad.Application.Features.Territorios.Vms;
using DGPCE.Sigemad.Application.Features.TipoIntervencionMedios.Vms;
using DGPCE.Sigemad.Application.Features.ValidacionesImpacto.Vms;
using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Mappings
{
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
            CreateMap<Municipio, MunicipioSinIdProvinciaVm>();
            CreateMap<Municipio, MunicipioConIdProvincia>();

            CreateMap<CreateIncendioCommand, Incendio>();
            
            CreateMap<UpdateIncendioCommand, Incendio>()
               .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Incendio, IncendioVm>();
            CreateMap<Evolucion, EvolucionVm>();

            CreateMap<UpdateEvolucionCommand, Evolucion>()
              .ForMember(dest => dest.EvolucionProcedenciaDestinos, opt => opt.MapFrom(src => MapEvolucionProcedenciaDestinos(src.EvolucionProcedenciaDestinos)));

            CreateMap<CreateEvolucionCommand, Evolucion>()
              .ForMember(dest => dest.EvolucionProcedenciaDestinos, opt => opt.MapFrom(src => MapEvolucionProcedenciaDestinos(src.EvolucionProcedenciaDestinos)));

            CreateMap<ApplicationUser, ApplicationUserVm>();

            CreateMap<CreateImpactoEvolucionCommand, ImpactoEvolucion>();
            CreateMap<UpdateImpactoEvolucionCommand, ImpactoEvolucion>();

            CreateMap<ImpactoClasificado, ImpactoClasificadoDescripcionVm>();

            CreateMap<TipoIntervencionMedio, TipoIntervencionMedioVm>();

            CreateMap<CreateIntervencionMedioCommand, IntervencionMedio>();
            CreateMap<UpdateIntervencionMedioCommand, IntervencionMedio>();

            CreateMap<Distrito, DistritoVm>();
            CreateMap<EntidadMenor, EntidadMenorVm>();
            CreateMap<DireccionCoordinacionEmergencia, DireccionCoordinacionEmergenciaVm>();
            CreateMap<ActivacionPlanEmergencia, ActivacionPlanEmergenciaVm>();
            CreateMap<AreaAfectada, AreaAfectadaVm>();
            CreateMap<UpdateAreaAfectadaCommand, AreaAfectada>();
            CreateMap<CreateAreaAfectadaCommand, AreaAfectada>();
            CreateMap<ValidacionImpactoClasificado, ValidacionImpactoClasificadoVm>()
                .ForMember(dest => dest.Label, opt => opt.MapFrom(src => src.Etiqueta));

            CreateMap<Territorio, TerritorioVm>();
            CreateMap<CreateDireccionCoordinacionEmergenciasCommand, DireccionCoordinacionEmergencia>();
            CreateMap<UpdateDireccionCoordinacionEmergenciaCommand, DireccionCoordinacionEmergencia>();
            CreateMap<DireccionCoordinacionEmergencia, CreateDireccionCoordinacionEmergenciasCommand>()
            .ForMember(dest => dest.IdTipoDireccionEmergencia, opt => opt.MapFrom(src => (TipoDireccionEmergenciaEnum)src.IdTipoDireccionEmergencia));

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

            CreateMap<EvolucionProcedenciaDestino, EvolucionProcedenciaDestinoVm>();

            CreateMap<SucesoRelacionado, SucesoRelacionadoVm>();
            CreateMap<CreateSucesoRelacionadoCommand, SucesoRelacionado>();
            CreateMap<CreateFileCommand, Archivo>();
        }

        private ICollection<EvolucionProcedenciaDestino> MapEvolucionProcedenciaDestinos(ICollection<int>? source)
        {
            if (source == null)
            {
                return new List<EvolucionProcedenciaDestino>();
            }

            return source.Select(id => new EvolucionProcedenciaDestino { IdProcedenciaDestino = id }).ToList();
        }
    }
}
