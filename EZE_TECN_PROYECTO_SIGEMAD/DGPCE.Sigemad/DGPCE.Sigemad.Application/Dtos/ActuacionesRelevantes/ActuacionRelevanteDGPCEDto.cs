using DGPCE.Sigemad.Application.Dtos.ActivacionesPlanes;
using DGPCE.Sigemad.Application.Dtos.ActivacionSistema;
using DGPCE.Sigemad.Application.Dtos.Common;
using DGPCE.Sigemad.Application.Dtos.ConvocatoriasCECOD;
using DGPCE.Sigemad.Application.Dtos.DeclaracionesZAGEP;
using DGPCE.Sigemad.Application.Dtos.EmergenciasNacionales;
using DGPCE.Sigemad.Application.Dtos.NotificacionesEmergencias;
using DGPCE.Sigemad.Domain.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Dtos.ActuacionesRelevantes;
public class ActuacionRelevanteDGPCEDto : BaseDto<int>
{
    public int IdSuceso { get; set; }

    public virtual EmergenciaNacionalDto? EmergenciaNacional { get; set; }
    public virtual List<ActivacionPlanEmergenciaDto> ActivacionPlanEmergencias { get; set; } = new();
    public virtual List<DeclaracionZAGEPDto>? DeclaracionesZAGEP { get; set; } = null;

    public virtual List<ActivacionSistemaDto>? ActivacionSistemas { get; set; } = null;

    public virtual List<ConvocatoriaCECODDto>? ConvocatoriasCECOD { get; set; } = null;

    public virtual List<NotificacionEmergenciaDto>? NotificacionesEmergencias { get; set; } = null;
}
