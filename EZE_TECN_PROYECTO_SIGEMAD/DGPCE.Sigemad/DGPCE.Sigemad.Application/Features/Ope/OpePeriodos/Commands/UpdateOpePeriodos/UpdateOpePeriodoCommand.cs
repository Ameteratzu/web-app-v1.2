using DGPCE.Sigemad.Domain.Enums;
using MediatR;
using NetTopologySuite.Geometries;

namespace DGPCE.Sigemad.Application.Features.Ope.OpePeriodos.Commands.UpdateOpePeriodos;

public class UpdateOpePeriodoCommand : IRequest
{
    public int Id { get; set; }

    public string Nombre { get; set; }

    public int IdOpePeriodoTipo { get; set; }

    public DateTime fechaInicioFaseSalida { get; set; }
    public DateTime fechaFinFaseSalida { get; set; }

    public DateTime fechaInicioFaseRetorno { get; set; }
    public DateTime fechaFinFaseRetorno { get; set; }

}
