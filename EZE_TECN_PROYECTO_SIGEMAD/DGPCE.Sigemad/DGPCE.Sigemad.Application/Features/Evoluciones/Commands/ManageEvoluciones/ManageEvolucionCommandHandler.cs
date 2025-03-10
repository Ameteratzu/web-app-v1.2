using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Contracts.RegistrosActualizacion;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.DatosPrincipales.Commands;
using DGPCE.Sigemad.Application.Features.Direcciones.Commands.CreateDirecciones;
using DGPCE.Sigemad.Application.Features.Parametros.Commands;
using DGPCE.Sigemad.Application.Features.Registros.Command.CreateRegistros;
using DGPCE.Sigemad.Application.Specifications.Evoluciones;
using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Evoluciones.Commands.ManageEvoluciones;


public class ManageEvolucionCommandHandler : IRequestHandler<ManageEvolucionCommand, ManageEvolucionResponse>
{
    private readonly ILogger<ManageEvolucionCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IRegistroActualizacionService _registroActualizacionService;


    public ManageEvolucionCommandHandler(
        ILogger<ManageEvolucionCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IRegistroActualizacionService registroActualizacionService)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _registroActualizacionService = registroActualizacionService;
    }

    public async Task<ManageEvolucionResponse> Handle(ManageEvolucionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(nameof(ManageEvolucionCommandHandler) + " - BEGIN");

        await _registroActualizacionService.ValidarSuceso(request.IdSuceso);
        await ComprobarRegistro(request.Registro);
        await ValidarProcedenciasDestinos(request.Registro.RegistroProcedenciasDestinos);
        await ComprobarParametros(request.Parametro);

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            RegistroActualizacion registroActualizacion = await _registroActualizacionService.GetOrCreateRegistroActualizacion<Evolucion>(
                request.IdRegistroActualizacion, request.IdSuceso, TipoRegistroActualizacionEnum.Evolucion);

            var evolucion = await GetOrCreateEvolucion(request, registroActualizacion);

            var parametrosOriginales = evolucion.Parametros.ToDictionary(d => d.Id, d => _mapper.Map<CreateParametroCommand>(d));
            var registrosOriginales = evolucion.Registros.ToDictionary(d => d.Id, d => _mapper.Map<CreateRegistroCommand>(d));
            var datosPrincipalesOriginales = evolucion.DatosPrincipales.ToDictionary(d => d.Id, d => _mapper.Map<CreateDatoPrincipalCommand>(d));
            //var registroOriginal = _mapper.Map<CreateRegistroCommand>(evolucion.Registro);
            //var datoPrincipalOriginal = _mapper.Map<CreateDatoPrincipalCommand>(evolucion.DatoPrincipal);

            MapAndSaveEvolucion(request, evolucion, registroActualizacion);

            //No hay listas para eliminar objeto
            await SaveEvolucion(evolucion);

            //MapAndSaveRegistroActualizacion(registroActualizacion, evolucion, registroOriginal, datoPrincipalOriginal);

            await _registroActualizacionService.SaveRegistroActualizacion<
                Evolucion, Registro, CreateRegistroCommand>(
                registroActualizacion,
                evolucion,
                ApartadoRegistroEnum.Registro,
                new List<int>(), registrosOriginales);

            await _registroActualizacionService.SaveRegistroActualizacion<
                Evolucion, DatoPrincipal, CreateDatoPrincipalCommand>(
                registroActualizacion,
                evolucion,
                ApartadoRegistroEnum.DatoPrincipal,
                new List<int>(), datosPrincipalesOriginales);


            await _registroActualizacionService.SaveRegistroActualizacion<
                Evolucion, Parametro, CreateParametroCommand>(
                registroActualizacion,
                evolucion,
                ApartadoRegistroEnum.Parametro,
                new List<int>(), parametrosOriginales);

            await _unitOfWork.CommitAsync();

            _logger.LogInformation($"{nameof(CreateOrUpdateDireccionCommandHandler)} - END");
            return new ManageEvolucionResponse
            {
                Id = evolucion.Id,
                IdRegistroActualizacion = registroActualizacion.Id
            };

        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Error en la transacción de CreateOrUpdateDireccionCommandHandler");
            throw;
        }
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

        if (request.IdSituacionEquivalente.HasValue)
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

    private async Task<Evolucion> GetOrCreateEvolucion(ManageEvolucionCommand request, RegistroActualizacion registroActualizacion)
    {
        if (registroActualizacion.IdReferencia > 0)
        {
            List<int> idsRegistro = new List<int>();
            List<int> idsDatoPrincipal = new List<int>();
            List<int> idsParametro = new List<int>();
            List<int> idsAreaAfectada = new List<int>();
            List<int> idsConsecuenciaActuacion = new List<int>();
            List<int> idsIntervencionMedio = new List<int>();

            bool includeRegistro = false;
            bool includeDatoPrincipal = false;

            foreach (var detalle in registroActualizacion.DetallesRegistro)
            {
                if (detalle.IdApartadoRegistro == (int)ApartadoRegistroEnum.Registro)
                {
                    idsRegistro.Add(detalle.IdReferencia);
                    includeRegistro = true;
                } 
                else if (detalle.IdApartadoRegistro == (int)ApartadoRegistroEnum.DatoPrincipal)
                {
                    idsDatoPrincipal.Add(detalle.IdReferencia);
                    includeDatoPrincipal = true;
                }
                else if (detalle.IdApartadoRegistro == (int)ApartadoRegistroEnum.Parametro)
                {
                    idsParametro.Add(detalle.IdReferencia);
                }
            }

            // Buscar la Evolucion por IdReferencia
            var evolucion = await _unitOfWork.Repository<Evolucion>()
                .GetByIdWithSpec(new EvolucionWithFilteredDataSpecification(
                    registroActualizacion.IdReferencia, 
                    idsRegistro,
                    idsDatoPrincipal,
                    idsParametro, 
                    idsAreaAfectada,
                    idsConsecuenciaActuacion, 
                    idsIntervencionMedio,
                    //includeRegistro,
                    //includeDatoPrincipal,
                    esFoto: false
                ));

            if (evolucion is null || evolucion.Borrado)
                throw new BadRequestException($"El registro de actualización con Id [{registroActualizacion.Id}] no tiene registro de Evolucion");

            return evolucion;
        }

        // Validar si ya existe un registro de Evolucion para este suceso
        var spec = new EvolucionWithRegistroSpecification(new EvolucionSpecificationParams { IdSuceso = request.IdSuceso });
        var evolucionExistente = await _unitOfWork.Repository<Evolucion>().GetByIdWithSpec(spec);

        return evolucionExistente ?? new Evolucion { IdSuceso = request.IdSuceso, EsFoto = false };
    }

    private void MapAndSaveEvolucion(ManageEvolucionCommand request, Evolucion evolucion, RegistroActualizacion registroActualizacion)
    {
        if (request.Registro != null)
        {
            Registro newRegistro = _mapper.Map<Registro>(request.Registro);
            newRegistro.Id = 0;

            if (registroActualizacion.Id > 0)
            {
                int? idRegistro = registroActualizacion.DetallesRegistro
                    .Where(d => d.IdApartadoRegistro == (int)ApartadoRegistroEnum.Registro)
                    .Select(d => d.IdReferencia)
                    .FirstOrDefault();

                if(idRegistro.HasValue && idRegistro.Value > 0)
                {
                    //Actualizar el registro
                    Registro? registroExistente = evolucion.Registros.FirstOrDefault(p => p.Id == idRegistro);
                    if (registroExistente != null)
                    {
                        var copiaOriginal = _mapper.Map<CreateRegistroCommand>(registroExistente);
                        var copiaNueva = _mapper.Map<CreateRegistroCommand>(request.Registro);
                        if (!copiaOriginal.Equals(copiaNueva))
                        {
                            _mapper.Map(request.Registro, registroExistente);
                            evolucion.Borrado = false;
                        }
                    }
                    else
                    {
                        evolucion.Registros.Add(newRegistro);
                    }
                }
                else
                {
                    evolucion.Registros.Add(newRegistro);
                }

            }
            else
            {
                evolucion.Registros.Add(newRegistro);
            }
        }

        //if (request.DatoPrincipal != null)
        //{
        //    if (evolucion.DatoPrincipal != null)
        //    {
        //        var copiaOriginal = _mapper.Map<CreateDatoPrincipalCommand>(evolucion.DatoPrincipal);
        //        var copiaNueva = _mapper.Map<CreateDatoPrincipalCommand>(request.DatoPrincipal);
        //        if (!copiaOriginal.Equals(copiaNueva))
        //        {
        //            _mapper.Map(request.DatoPrincipal, evolucion.DatoPrincipal);
        //        }
        //    }
        //    else
        //    {
        //        evolucion.DatoPrincipal = _mapper.Map<DatoPrincipal>(request.DatoPrincipal);
        //    }
        //}

        if (request.DatoPrincipal != null)
        {
            DatoPrincipal newDato = _mapper.Map<DatoPrincipal>(request.DatoPrincipal);
            newDato.Id = 0;

            if (registroActualizacion.Id > 0)
            {
                int? idDato = registroActualizacion.DetallesRegistro
                    .Where(d => d.IdApartadoRegistro == (int)ApartadoRegistroEnum.DatoPrincipal)
                    .Select(d => d.IdReferencia)
                    .FirstOrDefault();

                if (idDato.HasValue && idDato.Value > 0)
                {
                    //Actualizar el registro
                    DatoPrincipal? datoExistente = evolucion.DatosPrincipales.FirstOrDefault(p => p.Id == idDato);
                    if (datoExistente != null)
                    {
                        var copiaOriginal = _mapper.Map<CreateDatoPrincipalCommand>(datoExistente);
                        var copiaNueva = _mapper.Map<CreateDatoPrincipalCommand>(request.DatoPrincipal);
                        if (!copiaOriginal.Equals(copiaNueva))
                        {
                            _mapper.Map(request.DatoPrincipal, datoExistente);
                            evolucion.Borrado = false;
                        }
                    }
                    else
                    {
                        evolucion.DatosPrincipales.Add(newDato);
                    }
                }
                else
                {
                    evolucion.DatosPrincipales.Add(newDato);
                }

            }
            else
            {
                evolucion.DatosPrincipales.Add(newDato);
            }
        }

        // ****************

        if (request.Parametro != null)
        {
            Parametro newParametro = _mapper.Map<Parametro>(request.Parametro);
            newParametro.Id = 0;

            if (registroActualizacion.Id > 0)
            {
                int? idParametro = registroActualizacion.DetallesRegistro
                    .Where(d => d.IdApartadoRegistro == (int)ApartadoRegistroEnum.Parametro)
                    .Select(d => d.IdReferencia)
                    .FirstOrDefault();

                if (idParametro.HasValue && idParametro.Value > 0)
                {
                    //Actualizar el registro
                    Parametro? parametroExistente = evolucion.Parametros.FirstOrDefault(p => p.Id == idParametro);
                    if (parametroExistente != null)
                    {
                        var copiaOriginal = _mapper.Map<CreateParametroCommand>(parametroExistente);
                        var copiaNueva = _mapper.Map<CreateParametroCommand>(request.Parametro);
                        if (!copiaOriginal.Equals(copiaNueva))
                        {
                            _mapper.Map(request.Parametro, parametroExistente);
                            evolucion.Borrado = false;
                        }
                    }
                    else
                    {
                        evolucion.Parametros.Add(newParametro);
                    }
                }
                else
                {
                    evolucion.Parametros.Add(newParametro);
                }
            }
            else
            {
                evolucion.Parametros.Add(newParametro);
            }
        }
        
        /*
        if (request.Parametro != null)
        {
            if (registroActualizacion.Id > 0)
            {
                //Actualizar el parametro
                var parametroExistente = evolucion.Parametros.FirstOrDefault(p => p.Id == request.Parametro.Id);
                if (parametroExistente != null)
                {
                    _mapper.Map(request.Parametro, parametroExistente);
                }
                else
                {
                    //Agregar el parametro
                    Parametro newParametro = _mapper.Map<Parametro>(request.Parametro);
                    newParametro.Id = 0;
                    evolucion.Parametros.Add(newParametro);
                }

            }
            else
            {
                //Agregar el parametro
                Parametro newParametro = _mapper.Map<Parametro>(request.Parametro);
                newParametro.Id = 0;
                evolucion.Parametros.Add(newParametro);
            }
        }
        */
    }

    private async Task SaveEvolucion(Evolucion evolucion)
    {
        if (evolucion.Id > 0)
        {
            _unitOfWork.Repository<Evolucion>().UpdateEntity(evolucion);
        }
        else
        {
            _unitOfWork.Repository<Evolucion>().AddEntity(evolucion);
        }

        if (await _unitOfWork.Complete() <= 0)
            throw new Exception("No se pudo insertar/actualizar la Evolucion");
    }

    /*
    private void MapAndSaveRegistroActualizacion(
        RegistroActualizacion registroActualizacion,
        Evolucion evolucion,
        CreateRegistroCommand originalRegistro,
        CreateDatoPrincipalCommand originalDatoPrincipal)
    {
        registroActualizacion.IdReferencia = evolucion.Id;


        // Agregar registro de registro
        DetalleRegistroActualizacion detalleRegistro = new()
        {
            IdApartadoRegistro = (int)ApartadoRegistroEnum.Registro,
            IdReferencia = evolucion.Registro.Id,
        };

        var copiaNuevoRegistro = _mapper.Map<CreateRegistroCommand>(evolucion.Registro);
        if (originalRegistro == null && copiaNuevoRegistro != null)
        {
            detalleRegistro.IdEstadoRegistro = EstadoRegistroEnum.Creado;
        }
        else if (originalRegistro.Equals(copiaNuevoRegistro))
        {
            detalleRegistro.IdEstadoRegistro = EstadoRegistroEnum.Modificado;
        }
        else
        {
            detalleRegistro.IdEstadoRegistro = EstadoRegistroEnum.Permanente;
        }

        var detallePrevioRegistro = registroActualizacion.DetallesRegistro
            .FirstOrDefault(d => d.IdReferencia == evolucion.Registro.Id && d.IdApartadoRegistro == (int)ApartadoRegistroEnum.Registro);

        if (detallePrevioRegistro != null)
        {
            if (!originalRegistro.Equals(copiaNuevoRegistro))
            {
                if (detallePrevioRegistro.IdEstadoRegistro == EstadoRegistroEnum.Creado ||
                    detallePrevioRegistro.IdEstadoRegistro == EstadoRegistroEnum.CreadoYModificado)
                {
                    detallePrevioRegistro.IdEstadoRegistro = EstadoRegistroEnum.CreadoYModificado;
                }

                detallePrevioRegistro.IdEstadoRegistro = EstadoRegistroEnum.Modificado;
            }
            detallePrevioRegistro.IdEstadoRegistro = EstadoRegistroEnum.Permanente;
        }
        else
        {
            registroActualizacion.DetallesRegistro.Add(detalleRegistro);
        }

        //--------------------------------------------------------------------------------------
        // Agregar registro de dato principal
        DetalleRegistroActualizacion detalleRegistroDato = new()
        {
            IdApartadoRegistro = (int)ApartadoRegistroEnum.DatoPrincipal,
            IdReferencia = evolucion.DatoPrincipal.Id,
        };

        var copiaNuevoDato = _mapper.Map<CreateDatoPrincipalCommand>(evolucion.DatoPrincipal);
        if (originalDatoPrincipal == null && copiaNuevoDato != null)
        {
            detalleRegistroDato.IdEstadoRegistro = EstadoRegistroEnum.Creado;
        }
        else if (originalDatoPrincipal.Equals(copiaNuevoDato))
        {
            detalleRegistroDato.IdEstadoRegistro = EstadoRegistroEnum.Modificado;
        }
        else
        {
            detalleRegistroDato.IdEstadoRegistro = EstadoRegistroEnum.Permanente;
        }

        var detallePrevioDato = registroActualizacion.DetallesRegistro
            .FirstOrDefault(d => d.IdReferencia == evolucion.DatoPrincipal.Id && d.IdApartadoRegistro == (int)ApartadoRegistroEnum.DatoPrincipal);

        if (detallePrevioDato != null)
        {
            if (!originalDatoPrincipal.Equals(copiaNuevoDato))
            {
                if (detallePrevioDato.IdEstadoRegistro == EstadoRegistroEnum.Creado ||
                    detallePrevioDato.IdEstadoRegistro == EstadoRegistroEnum.CreadoYModificado)
                {
                    detallePrevioDato.IdEstadoRegistro = EstadoRegistroEnum.CreadoYModificado;
                }

                detallePrevioDato.IdEstadoRegistro = EstadoRegistroEnum.Modificado;
            }
            detallePrevioDato.IdEstadoRegistro = EstadoRegistroEnum.Permanente;
        }
        else
        {
            registroActualizacion.DetallesRegistro.Add(detalleRegistroDato);
        }
    }
    */
}