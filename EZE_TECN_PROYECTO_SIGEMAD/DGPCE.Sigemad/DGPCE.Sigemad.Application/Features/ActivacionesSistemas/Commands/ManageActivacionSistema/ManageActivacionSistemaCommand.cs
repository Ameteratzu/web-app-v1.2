using DGPCE.Sigemad.Application.Dtos.ActivacionesPlanes;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Features.ActivacionesSistemas.Commands.ManageActivacionSistema;
public class ManageActivacionSistemaCommand : IRequest<ManageActivacionPlanEmergenciaResponse>
{
    public int? IdActuacionRelevante { get; set; }
    public int IdSuceso { get; set; }
}
