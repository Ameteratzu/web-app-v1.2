using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.CCAA.Quereis.GetComunidadesAutonomasList;
using DGPCE.Sigemad.Application.Features.CCAA.Quereis.Vms;
using DGPCE.Sigemad.Application.Features.Provincias.Vms;
using DGPCE.Sigemad.Application.Features.Territorios.Queries.GetTerritoriosList;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Features.Provincias.Quereis.GetProvinciasList
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
            var provincias = await _unitOfWork.Repository<Provincia>().GetAllAsync();

            var provinciaSinMunicipiosVm = _mapper.Map<IReadOnlyList<Provincia>, IReadOnlyList<ProvinciaSinMunicipiosConIdComunidadVm>>(provincias);
            return provinciaSinMunicipiosVm;
     
        }
    }
}
