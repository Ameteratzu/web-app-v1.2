using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Dtos.Ope.Datos.OpeDatosFronteras;
using DGPCE.Sigemad.Domain.Modelos;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosFronteras.Commands.CreateOpeDatosFronteras;

public class ManageOpeDatoFronteraCommandHandler : IRequestHandler<ManageOpeDatoFronteraCommand, ManageOpeDatoFronteraResponse>
{
    private readonly ILogger<ManageOpeDatoFronteraCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ManageOpeDatoFronteraCommandHandler(
        ILogger<ManageOpeDatoFronteraCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ManageOpeDatoFronteraResponse> Handle(ManageOpeDatoFronteraCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(nameof(ManageOpeDatoFronteraCommandHandler) + " - BEGIN");

        await _unitOfWork.BeginTransactionAsync();

        try {

            // Comprobar si existe id de frontera
            var opeFrontera = await _unitOfWork.Repository<OpeFrontera>().GetByIdAsync(request.IdOpeFrontera);
            if (opeFrontera is  null || opeFrontera.Borrado)
            {
                throw new Exception("No existe la ope frontera");
            }

            // Mapeamos cada objeto de la lista a entidad
            var opeDatosFronteraEntities = request.Lista
                .Select(dato => _mapper.Map<OpeDatoFrontera>(dato))
                .ToList();

            foreach (var opeDatoFrontera in opeDatosFronteraEntities)
            {
                await SaveOpeDatoFrontera(opeDatoFrontera);
            }

            var result = await _unitOfWork.Complete();
            if (result <= 0)
            {
                throw new Exception("No se pudo insertar nuevo ope dato de frontera");
            }


            //return new CreateOpeDatoFronteraResponse { Id = opeDatoFronteraEntity.Id };
            return new ManageOpeDatoFronteraResponse
            {
                IdOpeFrontera = opeDatosFronteraEntities.FirstOrDefault()?.IdOpeFrontera ?? 0
            };
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Error en la transacción de CreateOpeDatoFronteraCommandHandler");
            throw;
        }
    }

    private async Task SaveOpeDatoFrontera(OpeDatoFrontera opeDatoFrontera)
    {
        if (opeDatoFrontera.Id > 0)
        {
            _unitOfWork.Repository<OpeDatoFrontera>().UpdateEntity(opeDatoFrontera);
        }
        else
        {
            _unitOfWork.Repository<OpeDatoFrontera>().AddEntity(opeDatoFrontera);
        }

        if (await _unitOfWork.Complete() <= 0)
            throw new Exception("No se pudo insertar/actualizar el dato de frontera de OPE");
    }
}
