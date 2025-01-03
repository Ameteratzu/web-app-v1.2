using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos;

public class Suceso: BaseDomainModel<int>
{
    public int IdTipo { get; set; }

    public virtual TipoSuceso TipoSuceso { get; set; } = null!;

    public virtual List<Incendio> Incendios { get; set; } = new();
}
