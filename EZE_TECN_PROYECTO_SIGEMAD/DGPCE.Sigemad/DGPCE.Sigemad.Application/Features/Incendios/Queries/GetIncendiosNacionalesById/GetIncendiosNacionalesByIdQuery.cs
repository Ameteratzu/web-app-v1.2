
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.Incendios.Queries.GetIncendiosNacionalesById
{
    public class GetIncendiosNacionalesByIdQuery : IRequest<Incendio>
    {
        public int Id { get; set; }

        public int? Idterritorio { get; set; }


    }
}
