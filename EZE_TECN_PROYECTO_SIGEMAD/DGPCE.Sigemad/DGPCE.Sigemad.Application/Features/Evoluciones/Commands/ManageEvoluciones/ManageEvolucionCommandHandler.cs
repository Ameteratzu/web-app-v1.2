using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.Evoluciones.Commands.UpdateEvoluciones;
using DGPCE.Sigemad.Application.Features.ImpactosClasificados.Vms;
using DGPCE.Sigemad.Application.Features.Parametros.Commands;
using DGPCE.Sigemad.Application.Features.Registros.Command.CreateRegistros;
using DGPCE.Sigemad.Application.Specifications.Evoluciones;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;



namespace DGPCE.Sigemad.Application.Features.Evoluciones.Commands.ManageEvoluciones
{

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

            var idEvolucion = 0;
            var result = 0;

            if (request.Registro != null)
            {
                await comprobarRegistro(request.Registro);
            }

            if (request.Parametro != null)
            {
                await comprobarParametros(request.Parametro);
            }


            if (request.RegistroProcedenciasDestinos != null)
            {
                var idsEvolucionProcedenciaDestinos = request.RegistroProcedenciasDestinos.Select(a => a).Distinct();
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


            Evolucion evolucion = null;
            if (request.IdEvolucion.HasValue)
            {

                var evolucionSpec = new EvolucionSpecificationParams() { Id = request.IdEvolucion };
                evolucion = await _unitOfWork.Repository<Evolucion>().GetByIdWithSpec(new EvolucionSpecification(evolucionSpec));

                if (evolucion is null)
                {
                    _logger.LogWarning($"request.IdEvolucion: {request.IdEvolucion}, no encontrado");
                    throw new NotFoundException(nameof(Evolucion), request.IdEvolucion);
                }

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
                idEvolucion = evolucion.Id;

            }
            else
            {
                var incendio = await _unitOfWork.Repository<Incendio>().GetByIdAsync(request.IdIncendio);
                if (incendio is null || incendio.Borrado)
                {
                    _logger.LogWarning($"request.IdIncendio: {request.IdIncendio}, no encontrado");
                    throw new NotFoundException(nameof(Incendio), request.IdIncendio);
                }

                var evolucionEntity = _mapper.Map<Evolucion>(request);

                _unitOfWork.Repository<Evolucion>().AddEntity(evolucionEntity);
                result = await _unitOfWork.Complete();
                idEvolucion = evolucionEntity.Id;
            }


            if (result <= 0)
            {
                throw new Exception("No se pudo llevar a cabo la operacion para insertar, actualizar o eliminar los datops de la evolución");
            }

            _logger.LogInformation(nameof(ManageEvolucionCommandHandler) + " - END");
            return new ManageEvolucionResponse { Id = idEvolucion };

        }



        private async Task comprobarRegistro(CreateRegistroCommand request)
        {
            if (request.IdMedio != null)
            {
                var medio = await _unitOfWork.Repository<Medio>().GetByIdAsync((int)request.IdMedio);
                if (medio is null)
                {
                    _logger.LogWarning($"request.IdMedio: {request.IdMedio}, no encontrado");
                    throw new NotFoundException(nameof(Medio), request.IdMedio);
                }
            }

            if (request.IdEntradaSalida != null)
            {
                var entradaSalida = await _unitOfWork.Repository<EntradaSalida>().GetByIdAsync((int)request.IdEntradaSalida);
                if (entradaSalida is null)
                {
                    _logger.LogWarning($"request.IdEntradaSalida: {request.IdEntradaSalida}, no encontrado");
                    throw new NotFoundException(nameof(EntradaSalida), request.IdEntradaSalida);
                }
            }

        }

        private async Task comprobarParametros(CreateParametroCommand request)
        {

            var estadoIncendio = await _unitOfWork.Repository<EstadoIncendio>().GetByIdAsync(request.IdEstadoIncendio);
            if (estadoIncendio is null || estadoIncendio.Obsoleto)
            {
                _logger.LogWarning($"request.IdEstadoIncendio: {request.IdEstadoIncendio}, no encontrado");
                throw new NotFoundException(nameof(EstadoIncendio), request.IdEstadoIncendio);
            }

            if (request.IdFase != null)
            {
                var fase = await _unitOfWork.Repository<Fase>().GetByIdAsync((int)request.IdFase);
                if (fase is null)
                {
                    _logger.LogWarning($"request.IdFase: {request.IdFase}, no encontrado");
                    throw new NotFoundException(nameof(Fase), request.IdFase);
                }
            }


            if (request.IdSituacionOperativa != null)
            {
                var situacionOperativa = await _unitOfWork.Repository<SituacionOperativa>().GetByIdAsync((int)request.IdSituacionOperativa);
                if (situacionOperativa is null)
                {
                    _logger.LogWarning($"request.IdFase: {request.IdSituacionOperativa}, no encontrado");
                    throw new NotFoundException(nameof(SituacionOperativa), request.IdSituacionOperativa);
                }
            }

            if (request.IdSituacionEquivalente != null)
            {
                var situacionEquivalente = await _unitOfWork.Repository<SituacionOperativa>().GetByIdAsync((int)request.IdSituacionEquivalente);
                if (situacionEquivalente is null)
                {
                    _logger.LogWarning($"request.IdSituacionEquivalente: {request.IdSituacionEquivalente}, no encontrado");
                    throw new NotFoundException(nameof(SituacionOperativa), request.IdSituacionEquivalente);
                }
            }


        }


    }
}
