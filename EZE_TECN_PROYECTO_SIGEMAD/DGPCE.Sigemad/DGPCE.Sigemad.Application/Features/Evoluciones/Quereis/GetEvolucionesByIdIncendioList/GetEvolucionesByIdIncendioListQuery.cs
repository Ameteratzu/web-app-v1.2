
using DGPCE.Sigemad.Application.Features.Evoluciones.Vms;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.Evoluciones.Quereis.GetEvolucionesByIdIncendioList
{
    public class GetEvolucionesByIdIncendioListQuery : IRequest<IReadOnlyList<EvolucionVm>>
    {
        public int IdIncendio { get; set; }


        public GetEvolucionesByIdIncendioListQuery(int id)
        {
            IdIncendio = id;
        }
    }
}
