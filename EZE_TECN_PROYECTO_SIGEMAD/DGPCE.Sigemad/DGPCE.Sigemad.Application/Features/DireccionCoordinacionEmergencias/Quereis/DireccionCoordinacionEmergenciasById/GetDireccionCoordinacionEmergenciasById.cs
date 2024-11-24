using DGPCE.Sigemad.Application.Dtos.DireccionCoordinaciones;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Quereis.DireccionCoordinacionEmergenciasById;

public class GetDireccionCoordinacionEmergenciasById : IRequest<DireccionCoordinacionEmergenciaDto>
{
    public int Id { get; set; }

    public GetDireccionCoordinacionEmergenciasById(int id)
    {
        Id = id;
    }
}