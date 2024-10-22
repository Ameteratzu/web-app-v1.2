using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.Alertas.Vms;
using DGPCE.Sigemad.Application.Features.EstadosAlertas.Commands.UpdateAlertas;
using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Application.Specifications.Alertas;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Features.Alertas.Queries.GetAlertaById
{
    public class GetAlertaByIdQueryHandler : IRequestHandler<GetAlertaByIdQuery, AlertaVm>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAlertaByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AlertaVm> Handle(GetAlertaByIdQuery request, CancellationToken cancellationToken)
        {

            var includes = new List<Expression<Func<Alerta, object>>>();
            includes.Add(p => p.EstadoAlerta!);

            var alerta = await _unitOfWork.Repository<Alerta>().GetAsync(
                b => b.Id == request.Id,
                null,
                includes,
                true
                );

            var data = _mapper.Map<IReadOnlyList<Alerta>, IReadOnlyList<AlertaVm>>(alerta);

            return data.FirstOrDefault();

         }
    }
}
