using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Dtos.NotificacionesEmergencias;
public class ManageNotificacionEmergenciaDto
{
    public int? Id { get; set; }
    public int IdTipoNotificacion { get; set; }
    public DateTime FechaHoraNotificacion { get; set; }
    public string OrganosNotificados { get; set; }
    public string? UCPM { get; set; }
    public string? OrganismoInternacional { get; set; }
    public string? OtrosPaises { get; set; }
    public string? Observaciones { get; set; }
}
