using DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Vms;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Quereis.DireccionCoordinacionEmergenciasById;

public class GetDireccionCoordinacionEmergenciasById : IRequest<DireccionCoordinacionEmergenciaVm>
{
    public int Id { get; set; }

    public GetDireccionCoordinacionEmergenciasById(int id)
    {
        Id = id;
    }
}