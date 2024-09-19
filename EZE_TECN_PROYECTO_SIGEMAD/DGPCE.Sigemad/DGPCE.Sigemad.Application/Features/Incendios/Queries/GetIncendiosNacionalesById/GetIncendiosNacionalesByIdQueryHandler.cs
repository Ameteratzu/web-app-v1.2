using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Specifications.Incendios;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.Incendios.Queries.GetIncendiosNacionalesById
{
    public class GetIncendiosNacionalesByIdQueryHandler : IRequestHandler<GetIncendiosNacionalesByIdQuery, Incendio>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetIncendiosNacionalesByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Incendio> Handle(GetIncendiosNacionalesByIdQuery request, CancellationToken cancellationToken)
        {
            var incendioParams = new IncendiosSpecificationParams
            {
               Id = request.Id,
               IdTerritorio =  1,
            };

            var spec = new IncendiosSpecification(incendioParams);
            return await _unitOfWork.Repository<Incendio>().GetByIdWithSpec(spec);
        }
    }
}
