using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Specifications.DireccionCoordinacionEmergencias;
public class DireccionCoordinacionEmergenciaSpecificationParams
{
    public int? Id { get; set; }
    public int? IdIncendio { get; set; }
    public int? IdTipoDireccionEmergencia { get; set; }
    public int? IdProvinciaCECOPI { get; set; }
    public int? IdMunicipioCECOPI { get; set; }
    
    public int? IdProvinciaPMA { get; set; }
    public int? IdMunicipioPMA { get; set; }
}
