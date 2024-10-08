using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.Provincias.Vms;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Provincias.Queries.GetProvinciasByIdCCAAList
{
    public class GetProvinciasByIdCCAAListQueryHandler : IRequestHandler<GetProvinciasByIdCCAAListQuery, IReadOnlyList<ProvinciaSinMunicipiosVm>>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetProvinciasByIdCCAAListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }
        public async Task<IReadOnlyList<ProvinciaSinMunicipiosVm>> Handle(GetProvinciasByIdCCAAListQuery request, CancellationToken cancellationToken)
        {

            IReadOnlyList<Provincia> provinciasListado = (await _unitOfWork.Repository<Provincia>().GetAsync(c => c.IdCcaa == request.IdCcaa))
               .OrderBy(m => m.Descripcion)
               .ToList()
               .AsReadOnly();

            var provincias = _mapper.Map<IReadOnlyList<Provincia>, IReadOnlyList<ProvinciaSinMunicipiosVm>>(provinciasListado);

            return provincias;

        }
    }


}


