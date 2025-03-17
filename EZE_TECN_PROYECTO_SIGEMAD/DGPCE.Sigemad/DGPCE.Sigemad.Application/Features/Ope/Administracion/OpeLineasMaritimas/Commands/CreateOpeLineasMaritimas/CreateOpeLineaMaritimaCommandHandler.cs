﻿using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeLineasMaritimas.Commands.CreateOpeLineasMaritimas;

public class CreateOpeLineaMaritimaCommandHandler : IRequestHandler<CreateOpeLineaMaritimaCommand, CreateOpeLineaMaritimaResponse>
{
    private readonly ILogger<CreateOpeLineaMaritimaCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateOpeLineaMaritimaCommandHandler(
        ILogger<CreateOpeLineaMaritimaCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CreateOpeLineaMaritimaResponse> Handle(CreateOpeLineaMaritimaCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(nameof(CreateOpeLineaMaritimaCommandHandler) + " - BEGIN");

        var opeLineaMaritimaEntity = _mapper.Map<OpeLineaMaritima>(request);
        _unitOfWork.Repository<OpeLineaMaritima>().AddEntity(opeLineaMaritimaEntity);

        var result = await _unitOfWork.Complete();
        if (result <= 0)
        {
            throw new Exception("No se pudo insertar nueva ope línea marítima");
        }

        _logger.LogInformation($"El ope periodo {opeLineaMaritimaEntity.Id} fue creado correctamente");

        _logger.LogInformation(nameof(CreateOpeLineaMaritimaCommandHandler) + " - END");
        return new CreateOpeLineaMaritimaResponse { Id = opeLineaMaritimaEntity.Id };
    }
}
