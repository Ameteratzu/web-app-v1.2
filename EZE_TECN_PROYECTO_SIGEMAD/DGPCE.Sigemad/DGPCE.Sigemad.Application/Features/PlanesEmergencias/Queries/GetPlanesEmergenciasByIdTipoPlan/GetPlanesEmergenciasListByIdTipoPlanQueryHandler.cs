using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Dtos.Registros;
using DGPCE.Sigemad.Application.Features.Evoluciones.Vms;
using DGPCE.Sigemad.Application.Features.Incendios.Queries.GetIncendiosNacionalesById;
using DGPCE.Sigemad.Application.Features.PlanesEmergencias.Vms;
using DGPCE.Sigemad.Application.Specifications.Incendios;
using DGPCE.Sigemad.Application.Specifications.PlanesEmergencias;
using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Features.PlanesEmergencias.Queries.GetPlanesEmergenciasByIdTipoPlan;

    public class GetPlanesEmergenciasListByIdTipoPlanQueryHandler: IRequestHandler<GetPlanesEmergenciasListByIdTipoPlanQuery, IReadOnlyList<PlanEmergenciaVm>>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetPlanesEmergenciasListByIdTipoPlanQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<PlanEmergenciaVm>> Handle(GetPlanesEmergenciasListByIdTipoPlanQuery request, CancellationToken cancellationToken)
        {
            var planEmergenciaParams = new PlanesEmegenciasParams
            {
                IdTipoPlan = request.IdTipoPlan,
            };
    
            var spec = new PlanesEmergenciasSpecification(planEmergenciaParams);
            var planesEmergencias = await _unitOfWork.Repository<PlanEmergencia>().GetAllWithSpec(spec);
            
            var planesEmergenciaVm = _mapper.Map<IReadOnlyList<PlanEmergencia>, IReadOnlyList<PlanEmergenciaVm>>(planesEmergencias);
            return planesEmergenciaVm;
    }
    }

