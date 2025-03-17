using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeFronteras.Commands.CreateOpeFronteras;

public class CreateOpeFronteraCommand : IRequest<CreateOpeFronteraResponse>
{
    public string Nombre { get; set; } = null!;
    public int IdCcaa { get; set; }
    public int IdProvincia { get; set; }
    public int IdMunicipio { get; set; }
    public string CarreteraPK { get; set; } = null!;
    public string CoordenadaUTM_X { get; set; } = null!;
    public string CoordenadaUTM_Y { get; set; } = null!;
    public int TransitoMedioVehiculos { get; set; }
    public int TransitoAltoVehiculos { get; set; }

}
