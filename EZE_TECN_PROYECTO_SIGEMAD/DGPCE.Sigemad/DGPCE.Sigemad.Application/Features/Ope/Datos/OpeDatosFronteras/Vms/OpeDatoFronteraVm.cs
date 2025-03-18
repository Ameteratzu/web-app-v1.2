namespace DGPCE.Sigemad.Application.Features.OtrasInformaciones.Vms;
public class OpeDatoFronteraVm
{
    public int IdOpeDatoFrontera { get; set; }
    public int IdOpeFrontera { get; set; }
    public DateTime FechaHoraInicioIntervalo { get; set; }
    public DateTime FechaHoraFinIntervalo { get; set; }
 
    public int NumeroVehiculos { get; set; }
    public string Afluencia { get; set; }

}
