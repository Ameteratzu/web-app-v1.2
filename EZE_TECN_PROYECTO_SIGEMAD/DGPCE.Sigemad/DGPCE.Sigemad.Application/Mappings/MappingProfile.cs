using AutoMapper;
using DGPCE.Sigemad.Application.Features.Alertas.Commands.CreateAlertas;
using DGPCE.Sigemad.Application.Features.Alertas.Commands.UpdateAlertas;
using DGPCE.Sigemad.Application.Features.Alertas.Queries.Vms;
using DGPCE.Sigemad.Application.Features.EstadosAlertas.Commands.CreateAlertas;
using DGPCE.Sigemad.Application.Features.EstadosAlertas.Commands.UpdateAlertas;
using DGPCE.Sigemad.Application.Features.EstadosAlertas.Queries.Vms;
using DGPCE.Sigemad.Domain.Modelos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

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
        }
    }
}
