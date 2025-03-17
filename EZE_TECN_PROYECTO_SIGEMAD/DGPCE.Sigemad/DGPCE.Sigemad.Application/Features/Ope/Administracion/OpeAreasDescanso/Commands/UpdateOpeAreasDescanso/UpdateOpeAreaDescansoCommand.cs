using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeAreasDescanso.Commands.UpdateOpeAreasDescanso;

public class UpdateOpeAreaDescansoCommand : IRequest
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public int IdOpeAreaDescansoTipo { get; set; }
    public int IdCcaa { get; set; }
    public int IdProvincia { get; set; }
    public int IdMunicipio { get; set; }
    public string CarreteraPK { get; set; } = null!;
    public string CoordenadaUTM_X { get; set; } = null!;
    public string CoordenadaUTM_Y { get; set; } = null!;
    public int Capacidad { get; set; }
    public int IdOpeEstadoOcupacion { get; set; }

}
