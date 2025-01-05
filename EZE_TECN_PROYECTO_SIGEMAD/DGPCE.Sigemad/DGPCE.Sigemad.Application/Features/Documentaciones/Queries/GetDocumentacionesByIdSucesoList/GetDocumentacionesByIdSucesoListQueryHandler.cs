using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.Documentaciones.Vms;
using DGPCE.Sigemad.Application.Specifications.Documentos;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.Documentaciones.Queries.GetDocumentacionesByIdIncendioList;
public class GetDocumentacionesByIdSucesoListQueryHandler : IRequestHandler<GetDocumentacionesByIdSucesoListQuery, IReadOnlyList<DocumentacionVm>>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetDocumentacionesByIdSucesoListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;

    }
    public async Task<IReadOnlyList<DocumentacionVm>> Handle(GetDocumentacionesByIdSucesoListQuery request, CancellationToken cancellationToken)
    {
        var spec = new DocumentacionSpecificationByIdSuceso(request.IdSuceso);
        var documentaciones = await _unitOfWork.Repository<Documentacion>()
        .GetAllWithSpec(spec);

        var documentacionesVm = _mapper.Map<IReadOnlyList<Documentacion>, IReadOnlyList<DocumentacionVm>>(documentaciones);
        return documentacionesVm;

    }
}

