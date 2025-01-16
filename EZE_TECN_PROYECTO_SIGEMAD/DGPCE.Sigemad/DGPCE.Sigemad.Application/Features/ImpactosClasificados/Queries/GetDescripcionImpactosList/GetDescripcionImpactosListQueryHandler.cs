using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.ImpactosClasificados.Vms;
using DGPCE.Sigemad.Application.Specifications.Impactos;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.ImpactosClasificados.Queries.GetDescripcionImpactosList;
public class GetDescripcionImpactosListQueryHandler : IRequestHandler<GetDescripcionImpactosListQuery, IReadOnlyList<ImpactoVm>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetDescripcionImpactosListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<ImpactoVm>> Handle(GetDescripcionImpactosListQuery request, CancellationToken cancellationToken)
    {
        var spec = new ImpactosClasificadosSpecification(request);
        var impactos = await _unitOfWork.Repository<ImpactoClasificado>().GetAllWithSpec(spec);

        var lista = impactos.Select(i => new ImpactoVm
        {
            Id = i.Id,
            Descripcion = i.Descripcion,
        }).ToList();

        return lista;
    }
}
