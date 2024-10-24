using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.ImpactosClasificados.Vms;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.ImpactosClasificados.Queries.GetDescripcionImpactosList;
public class GetDescripcionImpactosListQueryHandler : IRequestHandler<GetDescripcionImpactosListQuery, IReadOnlyList<TipoImpactoVm>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetDescripcionImpactosListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<TipoImpactoVm>> Handle(GetDescripcionImpactosListQuery request, CancellationToken cancellationToken)
    {
        var impactos = await _unitOfWork.Repository<ImpactoClasificado>().GetAllAsync();

        var resultadoAgrupado = impactos
        .GroupBy(i => i.TipoImpacto)
        .Select(tipoImpactoGroup => new TipoImpactoVm
        {
            Descripcion = tipoImpactoGroup.Key,
            Grupos = tipoImpactoGroup
                .GroupBy(g => g.GrupoImpacto)
                .Select(grupoImpactoGroup => new GrupoImpactoVm
                {
                    Descripcion = grupoImpactoGroup.Key,
                    Subgrupos = grupoImpactoGroup
                        .GroupBy(s => s.SubgrupoImpacto)
                        .Select(subgrupoImpactoGroup => new SubgrupoImpactoVm
                        {
                            Descripcion = subgrupoImpactoGroup.Key,
                            Clases = subgrupoImpactoGroup
                                .GroupBy(c => c.ClaseImpacto)
                                .Select(claseImpactoGroup => new ClaseImpactoVm
                                {
                                    Descripcion = claseImpactoGroup.Key,
                                    Impactos = claseImpactoGroup
                                        .Select(i => new ImpactoVm
                                        {
                                            Id = i.Id,
                                            Descripcion = i.Descripcion
                                        })
                                        .ToList()
                                })
                                .ToList()
                        })
                        .ToList()
                })
                .ToList()
        })
        .ToList();

        return resultadoAgrupado;
    }
}
