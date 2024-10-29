using AutoMapper;
using DGPCE.Sigemad.Application.Features.ActividadesPlanesEmergencia.Vms;
using DGPCE.Sigemad.Application.Features.Alertas.Commands.CreateAlertas;
using DGPCE.Sigemad.Application.Features.Alertas.Commands.UpdateAlertas;
using DGPCE.Sigemad.Application.Features.Alertas.Vms;
using DGPCE.Sigemad.Application.Features.ApplicationUsers.Vms;
using DGPCE.Sigemad.Application.Features.AreasAfectadas.Vms;
using DGPCE.Sigemad.Application.Features.CCAA.Vms;
using DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Vms;
using DGPCE.Sigemad.Application.Features.Distritos.Vms;
using DGPCE.Sigemad.Application.Features.EntidadesMenores.Vms;
using DGPCE.Sigemad.Application.Features.EstadosAlertas.Commands.CreateAlertas;
using DGPCE.Sigemad.Application.Features.EstadosAlertas.Commands.UpdateAlertas;
using DGPCE.Sigemad.Application.Features.EstadosAlertas.Vms;
using DGPCE.Sigemad.Application.Features.Evoluciones.Commands.UpdateEvoluciones;
using DGPCE.Sigemad.Application.Features.Evoluciones.Vms;
using DGPCE.Sigemad.Application.Features.ImpactosClasificados.Vms;
using DGPCE.Sigemad.Application.Features.ImpactosEvoluciones.Commands.CreateImpactoEvoluciones;
using DGPCE.Sigemad.Application.Features.ImpactosEvoluciones.Commands.UpdateImpactoEvoluciones;
using DGPCE.Sigemad.Application.Features.Incendios.Commands.UpdateIncendios;
using DGPCE.Sigemad.Application.Features.Incendios.Vms;
using DGPCE.Sigemad.Application.Features.IntervencionesMedios.Commands.CreateIntervencionMedios;
using DGPCE.Sigemad.Application.Features.IntervencionesMedios.Commands.UpdateIntervencionMedios;
using DGPCE.Sigemad.Application.Features.Menus.Vms;
using DGPCE.Sigemad.Application.Features.Municipios.Vms;
using DGPCE.Sigemad.Application.Features.Provincias.Vms;
using DGPCE.Sigemad.Application.Features.TipoIntervencionMedios.Vms;
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


            CreateMap<UpdateIncendioCommand, Incendio>();
            CreateMap<Incendio, IncendioVm>();
            CreateMap<Evolucion, EvolucionVm>()
              .ForMember(dest => dest.ProcedenciaDestinos, opt => opt.MapFrom(src => src.EvolucionProcedenciaDestinos != null ? src.EvolucionProcedenciaDestinos.Select(epd => epd.ProcedenciaDestino).ToList() : new List<ProcedenciaDestino>()));

            CreateMap<UpdateEvolucionCommand, Evolucion>();
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
        }
    }
}
