using DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Vms;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Quereis.GetDireccionCoordinacionEmergenciasByIdIncendioList;
public class GetDCEByIdIncendioListQuery : IRequest<IReadOnlyList<DireccionCoordinacionEmergenciaVm>>
{
    public int IdIncendio { get; set; }


    public GetDCEByIdIncendioListQuery(int id)
    {
        IdIncendio = id;
    }
}

