using DGPCE.Sigemad.Domain.Common;
using System.Text.Json.Serialization;

namespace DGPCE.Sigemad.Domain.Modelos;
public class OtraInformacion : BaseDomainModel<int>
{
    public OtraInformacion() {
        DetallesOtraInformacion = new List<DetalleOtraInformacion>();
    }

    public int IdIncendio { get; set; }

    public ICollection<DetalleOtraInformacion> DetallesOtraInformacion { get; set; }

    public virtual Incendio Incendio { get; set; }
}
