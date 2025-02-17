using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos;

public class Suceso: BaseDomainModel<int>
{
    public Suceso()
    {
        Incendios = new();
        Documentaciones = new();
        OtraInformaciones = new();
        SucesoRelacionados = new();
        ActuacionesRelevantes = new();
    }

    public int IdTipo { get; set; }
    public virtual TipoSuceso TipoSuceso { get; set; } = null!;


    public virtual List<Incendio> Incendios { get; set; }


    // Datos del suceso
    public virtual Evolucion Evolucion { get; set; }
    public virtual DireccionCoordinacionEmergencia DireccionCoordinacionEmergencia { get; set; }
    public virtual List<Documentacion> Documentaciones { get; set; }
    public virtual List<OtraInformacion> OtraInformaciones { get; set; }
    public virtual List<SucesoRelacionado> SucesoRelacionados { get; set; }
    public virtual List<ActuacionRelevanteDGPCE> ActuacionesRelevantes { get; set; }
}
