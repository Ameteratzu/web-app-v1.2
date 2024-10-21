using AutoMapper;
using DGPCE.Sigemad.Application.Features.Alertas.Commands.CreateAlertas;
using DGPCE.Sigemad.Application.Features.Alertas.Commands.UpdateAlertas;
using DGPCE.Sigemad.Application.Features.Alertas.Queries.Vms;
using DGPCE.Sigemad.Application.Features.ApplicationUsers.Vms;
using DGPCE.Sigemad.Application.Features.CCAA.Queries.Vms;
using DGPCE.Sigemad.Application.Features.EstadosAlertas.Commands.CreateAlertas;
using DGPCE.Sigemad.Application.Features.EstadosAlertas.Commands.UpdateAlertas;
using DGPCE.Sigemad.Application.Features.EstadosAlertas.Queries.Vms;
using DGPCE.Sigemad.Application.Features.Evoluciones.Commands.UpdateEvoluciones;
using DGPCE.Sigemad.Application.Features.Evoluciones.Vms;
using DGPCE.Sigemad.Application.Features.ImpactosClasificados.Queries.Vms;
using DGPCE.Sigemad.Application.Features.ImpactosEvoluciones.Commands.CreateImpactoEvoluciones;
using DGPCE.Sigemad.Application.Features.ImpactosEvoluciones.Commands.UpdateImpactoEvoluciones;
using DGPCE.Sigemad.Application.Features.Incendios.Commands.UpdateIncendios;
using DGPCE.Sigemad.Application.Features.Incendios.Vms;
using DGPCE.Sigemad.Application.Features.Menus.Queries.Vms;
using DGPCE.Sigemad.Application.Features.Municipios.Vms;
using DGPCE.Sigemad.Application.Features.Provincias.Vms;
using DGPCE.Sigemad.Application.Features.TipoIntervencionMedios.Quereis.Vms;
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
        }
    }
}
