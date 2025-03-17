using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpePuertos;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;
using Microsoft.Extensions.Logging;


namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpePuertos.Queries.GetOpePuertoById
{
    public class GetOpePuertoByIdQueryHandler : IRequestHandler<GetOpePuertoByIdQuery, OpePuerto>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetOpePuertoByIdQueryHandler> _logger;

        public GetOpePuertoByIdQueryHandler(
            IUnitOfWork unitOfWork, IMapper mapper,
            ILogger<GetOpePuertoByIdQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<OpePuerto> Handle(GetOpePuertoByIdQuery request, CancellationToken cancellationToken)
        {
            var periodoParams = new OpePuertosSpecificationParams
            {
                Id = request.Id,
            };

            var spec = new OpePuertosSpecification(periodoParams);
            var periodo = await _unitOfWork.Repository<OpePuerto>().GetByIdWithSpec(spec);

            if (periodo == null)
            {
                _logger.LogWarning($"No se encontro periodo con id: {request.Id}");
                throw new NotFoundException(nameof(OpePuerto), request.Id);
            }

            return periodo;
        }
    }
}
