using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.Provincias.Vms;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Provincias.Queries.GetProvinciasList
{
    public class GetProvinciasListQueryHandler : IRequestHandler<GetProvinciasListQuery, IReadOnlyList<ProvinciaSinMunicipiosConIdComunidadVm>>
    {


        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetProvinciasListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<ProvinciaSinMunicipiosConIdComunidadVm>> Handle(GetProvinciasListQuery request, CancellationToken cancellationToken)
        {
            var provincias = (await _unitOfWork.Repository<Provincia>().GetAllAsync())
             .OrderBy(m => m.Descripcion)
             .ToList()
             .AsReadOnly(); ;

            var provinciasSinMunicipios = _mapper.Map<IReadOnlyList<Provincia>, IReadOnlyList<ProvinciaSinMunicipiosConIdComunidadVm>>(provincias);
            return provinciasSinMunicipios;

        }
    }
}
