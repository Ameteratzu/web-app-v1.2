using DGPCE.Sigemad.Application.Features.Evoluciones.Vms;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.Evoluciones.Quereis.GetEvolucionesById
{

     public class GetEvolucionesByIdQuery : IRequest<EvolucionVm>
    {
        public int Id { get; set; }

        public GetEvolucionesByIdQuery(int id)
        {
            Id = id;
        }
    }
}
