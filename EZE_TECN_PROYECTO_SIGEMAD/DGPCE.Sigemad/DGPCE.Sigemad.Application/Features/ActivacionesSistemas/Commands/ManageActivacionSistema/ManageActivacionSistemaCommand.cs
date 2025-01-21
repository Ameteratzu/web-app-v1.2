using DGPCE.Sigemad.Application.Dtos.ActivacionesPlanes;
using DGPCE.Sigemad.Application.Dtos.ActivacionSistema;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Features.ActivacionesSistemas.Commands.ManageActivacionSistema;
public class ManageActivacionSistemaCommand : IRequest<ManageActivacionSistemaCommand>
{
    public int? IdActuacionRelevante { get; set; }
    public int IdSuceso { get; set; }

    public List<ManageActivacionSistemaDto> ActivacionSistemas { get; set; } = new();
}
