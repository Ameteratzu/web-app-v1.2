using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpePuertos.Queries.GetOpePuertoById
{
    public class GetOpePuertoByIdQuery : IRequest<OpePuerto>
    {
        public int Id { get; set; }

        public GetOpePuertoByIdQuery(int id)
        {
            Id = id;
        }
    }
}
