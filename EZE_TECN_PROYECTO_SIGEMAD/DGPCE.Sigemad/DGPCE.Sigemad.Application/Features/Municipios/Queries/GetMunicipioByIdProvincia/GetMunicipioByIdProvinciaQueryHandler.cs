using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.Municipios.Vms;
using DGPCE.Sigemad.Application.Specifications.Municipios;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Municipios.Queries.GetMunicipioByIdProvincia
{
    public class GetMunicipioByIdProvinciaQueryHandler : IRequestHandler<GetMunicipioByIdProvinciaQuery, IReadOnlyList<MunicipioSinIdProvinciaVm>>
    {
        private readonly ILogger<GetMunicipioByIdProvinciaQueryHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetMunicipioByIdProvinciaQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetMunicipioByIdProvinciaQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IReadOnlyList<MunicipioSinIdProvinciaVm>> Handle(GetMunicipioByIdProvinciaQuery request, CancellationToken cancellationToken)
        {

            var provincia = await _unitOfWork.Repository<Provincia>().GetByIdAsync(request.IdProvincia);
            if (provincia is null)
            {
                _logger.LogWarning($"request.IdProvincia: {request.IdProvincia}, no encontrado");
                throw new NotFoundException(nameof(Provincia), request.IdProvincia);
            }

            var municipioParams = new MunicipiosSpecificationParams
            {
                IdProvincia = request.IdProvincia
            };

            var spec = new MunicipiosSpecification(municipioParams);
            var municipiosListado = await _unitOfWork.Repository<Municipio>()
            .GetAllWithSpec(spec);

            var municipiosSinIdProvincia = _mapper.Map<IReadOnlyList<Municipio>, IReadOnlyList<MunicipioSinIdProvinciaVm>>(municipiosListado);

            return municipiosSinIdProvincia;

        }
    }
}
