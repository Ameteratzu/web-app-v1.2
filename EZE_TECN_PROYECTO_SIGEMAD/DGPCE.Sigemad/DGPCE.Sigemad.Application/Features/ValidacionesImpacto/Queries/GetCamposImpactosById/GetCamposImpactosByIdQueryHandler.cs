using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.ValidacionesImpacto.Vms;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.ValidacionesImpacto.Queries.GetCamposImpactosById;
public class GetCamposImpactosByIdQueryHandler : IRequestHandler<GetCamposImpactosByIdQuery, IReadOnlyList<ValidacionImpactoClasificadoVm>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private const string TipoSelect = "Select";
    private const string Campo = "TipoDanio";

    public GetCamposImpactosByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<ValidacionImpactoClasificadoVm>> Handle(GetCamposImpactosByIdQuery request, CancellationToken cancellationToken)
    {
        IReadOnlyList<ValidacionImpactoClasificado> camposImpactos = (await _unitOfWork.Repository<ValidacionImpactoClasificado>().GetAsync(m => m.IdImpactoClasificado == request.Id))
            .OrderBy(m => m.Id)
            .ToList()
            .AsReadOnly();

        var camposImpactosVm = _mapper.Map<IReadOnlyList<ValidacionImpactoClasificado>, IReadOnlyList<ValidacionImpactoClasificadoVm>>(camposImpactos);

        var campo = camposImpactosVm.FirstOrDefault(c => c.TipoCampo == TipoSelect && c.Campo == Campo);
        if (campo != null)
        {
            var opciones = await _unitOfWork.Repository<TipoDanio>().GetAllAsync();
            campo.Options = opciones.Select(o => new OptionVm
            {
                Id = o.Id.ToString(),
                Description = o.Descripcion
            }).ToList();
        }

        return camposImpactosVm;
    }
}
