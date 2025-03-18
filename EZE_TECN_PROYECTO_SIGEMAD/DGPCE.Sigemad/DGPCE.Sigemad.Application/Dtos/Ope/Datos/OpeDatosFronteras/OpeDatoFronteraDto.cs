namespace DGPCE.Sigemad.Application.Dtos.Ope.Datos.OpeDatosFronteras;
public class OpeDatoFronteraDto
{
    public int Id { get; set; }

    public int IdOpeFrontera { get; set; }
    public DateTime FechaHoraInicioIntervalo { get; set; }
    public DateTime FechaHoraFinIntervalo { get; set; }
    public int NumeroVehiculos { get; set; }
    public string Afluencia { get; set; }


    public bool EsEliminable { get; set; }
}
