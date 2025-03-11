using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.OpePeriodos.Commands.DeleteOpePeriodos
{
    public class DeleteOpePeriodoCommand : IRequest
    {
        public int Id { get; set; }
    }
}
