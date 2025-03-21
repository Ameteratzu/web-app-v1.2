using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpeLineasMaritimas;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeLineasMaritimas.Commands.UpdateOpeLineasMaritimas;

public class UpdateOpeLineaMaritimaCommandHandler : IRequestHandler<UpdateOpeLineaMaritimaCommand>
{
    private readonly ILogger<UpdateOpeLineaMaritimaCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateOpeLineaMaritimaCommandHandler(
        ILogger<UpdateOpeLineaMaritimaCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateOpeLineaMaritimaCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(nameof(UpdateOpeLineaMaritimaCommandHandler) + " - BEGIN");

        var opeLineaMaritimaSpec = new OpeLineaMaritimaActiveByIdSpecification(request.Id);
        var opeLineaMaritimaToUpdate = await _unitOfWork.Repository<OpeLineaMaritima>().GetByIdWithSpec(opeLineaMaritimaSpec);


        if (opeLineaMaritimaToUpdate == null)
        {
            _logger.LogWarning($"No se encontro ope línea marítima con id: {request.Id}");
            throw new NotFoundException(nameof(OpeLineaMaritima), request.Id);
        }

        // TEST
        //Auditoria auditoria = new Auditoria("Ope_LineaMaritima", AuditoriaConstantes.OperacionModificacion, null);
        //auditoria.ValoresAntiguos = opeLineaMaritimaToUpdate.ToAuditoria();
        // FIN TEST

        _mapper.Map(request, opeLineaMaritimaToUpdate, typeof(UpdateOpeLineaMaritimaCommand), typeof(OpeLineaMaritima));

        // TEST
        //auditoria.ValoresNuevos = opeLineaMaritimaToUpdate.ToAuditoria();
        // FIN TEST

        _unitOfWork.Repository<OpeLineaMaritima>().UpdateEntity(opeLineaMaritimaToUpdate);
        await _unitOfWork.Complete();

        _logger.LogInformation($"Se actualizo correctamente la ope línea marítima con id: {request.Id}");
        _logger.LogInformation(nameof(UpdateOpeLineaMaritimaCommandHandler) + " - END");

        return Unit.Value;
    }
}
