using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.Documentaciones.Vms;
using DGPCE.Sigemad.Application.Features.Documentciones.Queries.GetDocumentacionesByIdIncendioList;
using DGPCE.Sigemad.Application.Specifications.Documentos;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.Documentaciones.Queries.GetDocumentacionesByIdIncendioList;
    public class GetDocumentacionesByIdIncendioListQueryHandler : IRequestHandler<GetDocumentacionesByIdIncendioListQuery, IReadOnlyList<DocumentacionVm>>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetDocumentacionesByIdIncendioListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }
        public async Task<IReadOnlyList<DocumentacionVm>> Handle(GetDocumentacionesByIdIncendioListQuery request, CancellationToken cancellationToken)
        {

            var documentosParams = new DocumentoSpecificationParams
            {
                IdIncendio = request.IdIncendio
            };

            var spec = new DocumentoSpecification(documentosParams);
            var documentaciones = await _unitOfWork.Repository<Documentacion>()
            .GetAllWithSpec(spec);

            var documentacionesVm = _mapper.Map<IReadOnlyList<Documentacion>, IReadOnlyList<DocumentacionVm>>(documentaciones);
            return documentacionesVm;

        }
    }

