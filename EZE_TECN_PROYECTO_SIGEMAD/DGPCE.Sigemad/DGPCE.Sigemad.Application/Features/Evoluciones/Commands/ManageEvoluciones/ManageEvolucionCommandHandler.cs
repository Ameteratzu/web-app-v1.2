using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.Parametros.Commands;
using DGPCE.Sigemad.Application.Features.Registros.Command.CreateRegistros;
using DGPCE.Sigemad.Application.Specifications.Evoluciones;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Evoluciones.Commands.ManageEvoluciones;


public class ManageEvolucionCommandHandler : IRequestHandler<ManageEvolucionCommand, ManageEvolucionResponse>
{
    private readonly ILogger<ManageEvolucionCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;


    public ManageEvolucionCommandHandler(
        ILogger<ManageEvolucionCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ManageEvolucionResponse> Handle(ManageEvolucionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(nameof(ManageEvolucionCommandHandler) + " - BEGIN");

        if (request.Registro != null)
        {
            await ComprobarRegistro(request.Registro);
        }

        if (request.Parametro != null)
        {
            await ComprobarParametros(request.Parametro);
        }

        if (request.Registro != null && request.Registro.RegistroProcedenciasDestinos != null)
        {
            await ValidarProcedenciasDestinos(request.Registro.RegistroProcedenciasDestinos);
        }

        var idEvolucion = await ProcesarEvolucion(request);

        _logger.LogInformation($"{nameof(ManageEvolucionCommandHandler)} - END");
        return new ManageEvolucionResponse { Id = idEvolucion };
    }



    private async Task ComprobarRegistro(CreateRegistroCommand request)
    {
        if (request.IdMedio.HasValue)
        {
            var medio = await _unitOfWork.Repository<Medio>().GetByIdAsync(request.IdMedio.Value);
            if (medio is null)
            {
                _logger.LogWarning($"request.IdMedio: {request.IdMedio}, no encontrado");
                throw new NotFoundException(nameof(Medio), request.IdMedio);
            }
        }

        if (request.IdEntradaSalida.HasValue)
        {
            var entradaSalida = await _unitOfWork.Repository<EntradaSalida>().GetByIdAsync(request.IdEntradaSalida.Value);
            if (entradaSalida is null)
            {
                _logger.LogWarning($"request.IdEntradaSalida: {request.IdEntradaSalida}, no encontrado");
                throw new NotFoundException(nameof(EntradaSalida), request.IdEntradaSalida);
            }
        }
    }

    private async Task ComprobarParametros(CreateParametroCommand request)
    {

        var estadoIncendio = await _unitOfWork.Repository<EstadoIncendio>().GetByIdAsync(request.IdEstadoIncendio);
        if (estadoIncendio is null || estadoIncendio.Obsoleto)
        {
            _logger.LogWarning($"request.IdEstadoIncendio: {request.IdEstadoIncendio}, no encontrado");
            throw new NotFoundException(nameof(EstadoIncendio), request.IdEstadoIncendio);
        }

        if (request.IdFaseEmergencia.HasValue)
        {
            var fase = await _unitOfWork.Repository<FaseEmergencia>().GetByIdAsync(request.IdFaseEmergencia.Value);
            if (fase is null)
            {
                _logger.LogWarning($"request.IdFase: {request.IdFaseEmergencia}, no encontrado");
                throw new NotFoundException(nameof(FaseEmergencia), request.IdFaseEmergencia);
            }
        }


        if (request.IdPlanSituacion.HasValue)
        {
            var situacionOperativa = await _unitOfWork.Repository<PlanSituacion>().GetByIdAsync(request.IdPlanSituacion.Value);
            if (situacionOperativa is null)
            {
                _logger.LogWarning($"request.IdPlanSituacion: {request.IdPlanSituacion}, no encontrado");
                throw new NotFoundException(nameof(PlanSituacion), request.IdPlanSituacion);
            }
        }

        if(request.IdSituacionEquivalente.HasValue)
        {
            var situacionEquivalente = await _unitOfWork.Repository<SituacionEquivalente>().GetByIdAsync(request.IdSituacionEquivalente.Value);
            if (situacionEquivalente is null || situacionEquivalente.Obsoleto)
            {
                _logger.LogWarning($"request.IdSituacionEquivalente: {request.IdSituacionEquivalente}, no encontrado");
                throw new NotFoundException(nameof(SituacionEquivalente), request.IdSituacionEquivalente);
            }
        }
    }

    private async Task ValidarProcedenciasDestinos(List<int> registroProcedenciasDestinos)
    {
        var idsEvolucionProcedenciaDestinos = registroProcedenciasDestinos.Distinct();
        var evolucionProcedenciaDestinosExistentes = await _unitOfWork.Repository<ProcedenciaDestino>().GetAsync(p => idsEvolucionProcedenciaDestinos.Contains(p.Id));

        if (evolucionProcedenciaDestinosExistentes.Count() != idsEvolucionProcedenciaDestinos.Count())
        {
            var idsEvolucionProcedenciaDestinoExistentes = evolucionProcedenciaDestinosExistentes.Select(p => p.Id).ToList();
            var idsEvolucionProcedenciaDestinosExistentesInvalidas = idsEvolucionProcedenciaDestinos.Except(idsEvolucionProcedenciaDestinoExistentes).ToList();

            if (idsEvolucionProcedenciaDestinosExistentesInvalidas.Any())
            {
                _logger.LogWarning($"Las siguientes Id's de procedencia destinos: {string.Join(", ", idsEvolucionProcedenciaDestinosExistentesInvalidas)}, no se encontraron");
                throw new NotFoundException(nameof(ProcedenciaDestino), string.Join(", ", idsEvolucionProcedenciaDestinosExistentesInvalidas));
            }
        }
    }

    private async Task<int> ProcesarEvolucion(ManageEvolucionCommand request)
    {
        Evolucion evolucion = null;
        var result = 0;

        if (request.IdEvolucion.HasValue && request.IdEvolucion.Value > 0)
        {
            evolucion = await ObtenerEvolucionExistente(request.IdEvolucion.Value);

            if (request.DatoPrincipal is null && request.Parametro is null && request.Registro is null)
            {
                _unitOfWork.Repository<Evolucion>().DeleteEntity(evolucion);
            }
            else
            {
                _mapper.Map(request, evolucion, typeof(CreateRegistroCommand), typeof(Evolucion));
                _unitOfWork.Repository<Evolucion>().UpdateEntity(evolucion);
            }

            result = await _unitOfWork.Complete();
            return evolucion.Id;
        }
        else
        {
            return await CrearNuevaEvolucion(request);
        }
    }

    private async Task<Evolucion> ObtenerEvolucionExistente(int idEvolucion)
    {
        var evolucionSpec = new EvolucionSpecificationParams() { Id = idEvolucion };
        var evolucion = await _unitOfWork.Repository<Evolucion>().GetByIdWithSpec(new EvolucionSpecification(evolucionSpec));

        if (evolucion is null)
        {
            _logger.LogWarning($"request.IdEvolucion: {idEvolucion}, no encontrado");
            throw new NotFoundException(nameof(Evolucion), idEvolucion);
        }

        return evolucion;
    }

    private async Task<int> CrearNuevaEvolucion(ManageEvolucionCommand request)
    {
        var suceso = await _unitOfWork.Repository<Suceso>().GetByIdAsync(request.IdSuceso);
        if (suceso is null || suceso.Borrado)
        {
            _logger.LogWarning($"request.IdSuceso: {request.IdSuceso}, no encontrado");
            throw new NotFoundException(nameof(Suceso), request.IdSuceso);
        }

        var evolucionEntity = _mapper.Map<Evolucion>(request);
        _unitOfWork.Repository<Evolucion>().AddEntity(evolucionEntity);
        var result = await _unitOfWork.Complete();

        if (result <= 0)
        {
            throw new Exception("No se pudo llevar a cabo la operacion para insertar, actualizar o eliminar los datos de la evolución");
        }

        return evolucionEntity.Id;
    }

}
