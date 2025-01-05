using DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Vms;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Quereis.GetDireccionCoordinacionEmergenciasByIdIncendioList;
public class GetDCEByIdSucesoListQuery : IRequest<IReadOnlyList<DireccionCoordinacionEmergenciaVm>>
{
    public int IdSuceso { get; set; }


    public GetDCEByIdSucesoListQuery(int id)
    {
        IdSuceso = id;
    }
}

