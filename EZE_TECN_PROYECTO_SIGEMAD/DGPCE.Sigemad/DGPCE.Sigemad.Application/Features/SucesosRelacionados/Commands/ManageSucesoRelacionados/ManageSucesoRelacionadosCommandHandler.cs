using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Contracts.RegistrosActualizacion;
using DGPCE.Sigemad.Application.Dtos.SucesoRelacionados;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.Direcciones.Commands.CreateDirecciones;
using DGPCE.Sigemad.Application.Features.Evoluciones.Vms;
using DGPCE.Sigemad.Application.Specifications.SucesosRelacionados;
using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.SucesosRelacionados.Commands.ManageSucesoRelacionados;
public class ManageSucesoRelacionadosCommandHandler : IRequestHandler<ManageSucesoRelacionadosCommand, ManageSucesoRelacionadoResponse>
{
    private readonly ILogger<ManageSucesoRelacionadosCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IRegistroActualizacionService _registroActualizacionService;

    public ManageSucesoRelacionadosCommandHandler(
        ILogger<ManageSucesoRelacionadosCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IRegistroActualizacionService registroActualizacionService
    )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _registroActualizacionService = registroActualizacionService;
    }

    public async Task<ManageSucesoRelacionadoResponse> Handle(ManageSucesoRelacionadosCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(ManageSucesoRelacionadosCommandHandler)} - BEGIN");

        await _registroActualizacionService.ValidarSuceso(request.IdSuceso);
        await ValidateProcedenciasDestinosAsync( request);

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            RegistroActualizacion registroActualizacion = await _registroActualizacionService.GetOrCreateRegistroActualizacion<OtraInformacion>(
                request.IdRegistroActualizacion, request.IdSuceso, TipoRegistroActualizacionEnum.SucesosRelacionados);

            SucesoRelacionado sucesoRelacionado = await GetOrCreateSucesoRelacionado(request, registroActualizacion);

            var detalleSucesosRelacionadosOriginales = sucesoRelacionado.DetalleSucesoRelacionados.ToDictionary(d => d.IdCabeceraSuceso, d => _mapper.Map<DetalleSucesoRelacionado>(d));
         
            var direccionesParaEliminar = MapAndSaveAndDeleteSucesosRelacionados(request, sucesoRelacionado);
            await SaveSucesoRelacionado(sucesoRelacionado);

            MapAndSaveRegistroActualizacion(registroActualizacion, sucesoRelacionado, request);

            await _unitOfWork.CommitAsync();

            _logger.LogInformation($"{nameof(CreateOrUpdateDireccionCommandHandler)} - END");

            return new ManageSucesoRelacionadoResponse
            {
                IdSucesoRelacionado = sucesoRelacionado.Id,
                IdRegistroActualizacion = registroActualizacion.Id
            };

        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Error en la transacción de ManageSucesoRelacionadosCommandHandler");
            throw;
        }


    }

    private void MapAndSaveRegistroActualizacion(RegistroActualizacion registroActualizacion,SucesoRelacionado sucesoRelacionado,ManageSucesoRelacionadosCommand originalSucesoRelacionado)
   {
        registroActualizacion.IdReferencia = sucesoRelacionado.Id;


        // Agregar registro de suceso
        DetalleRegistroActualizacion detalleRegistro = new()
        {
            IdApartadoRegistro = (int)ApartadoRegistroEnum.SucesosRelacionados,
            IdReferencia = sucesoRelacionado.Id,
        };

        var copiaNuevoSuceso = _mapper.Map<ManageSucesoRelacionadosCommand>(sucesoRelacionado);
        if (originalSucesoRelacionado == null && copiaNuevoSuceso != null)
        {
            detalleRegistro.IdEstadoRegistro = EstadoRegistroEnum.Creado;
        }
        else if (originalSucesoRelacionado.Equals(copiaNuevoSuceso))
        {
            detalleRegistro.IdEstadoRegistro = EstadoRegistroEnum.Modificado;
        }
        else
        {
            detalleRegistro.IdEstadoRegistro = EstadoRegistroEnum.Permanente;
        }

        var detallePrevioRegistro = registroActualizacion.DetallesRegistro
            .FirstOrDefault(d => d.IdReferencia == sucesoRelacionado.Id && d.IdApartadoRegistro == (int)ApartadoRegistroEnum.Registro);

        if (detallePrevioRegistro != null)
        {
            if (!originalSucesoRelacionado.Equals(copiaNuevoSuceso))
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
    }

    private async Task SaveSucesoRelacionado(SucesoRelacionado sucesoRelacionado)
    {
        if (sucesoRelacionado.Id > 0)
        {
            _unitOfWork.Repository<SucesoRelacionado>().UpdateEntity(sucesoRelacionado);
        }
        else
        {
            _unitOfWork.Repository<SucesoRelacionado>().AddEntity(sucesoRelacionado);
        }

        if (await _unitOfWork.Complete() <= 0)
            throw new Exception("No se pudo insertar/actualizar el Suceso Relacionado");
    }

    private List<int> MapAndSaveAndDeleteSucesosRelacionados(ManageSucesoRelacionadosCommand request, SucesoRelacionado sucesoRelacionado)
    {
        // Manejo de DetalleSucesoRelacionado
        var idsExistentes = sucesoRelacionado.DetalleSucesoRelacionados
            .Select(d => d.IdSucesoAsociado)
            .ToList();

        var idsNuevos = request.IdsSucesosAsociados.Except(idsExistentes).ToList();
        var idsAEliminar = idsExistentes.Except(request.IdsSucesosAsociados).ToList();

        // Agregar nuevos sucesos asociados
        foreach (var idNuevo in idsNuevos)
        {
            sucesoRelacionado.DetalleSucesoRelacionados.Add(new DetalleSucesoRelacionado
            {
                IdSucesoAsociado = idNuevo
            });
        }

        // Eliminar los que no se enviaron en el listado
        foreach (var idEliminar in idsAEliminar)
        {
            var detalle = sucesoRelacionado.DetalleSucesoRelacionados
                .FirstOrDefault(d => d.IdSucesoAsociado == idEliminar);

            if (detalle != null)
            {
                sucesoRelacionado.DetalleSucesoRelacionados.Remove(detalle);
            }
        }

        return idsAEliminar;
    }

    private async Task<SucesoRelacionado> GetOrCreateSucesoRelacionado(ManageSucesoRelacionadosCommand request, RegistroActualizacion registroActualizacion)
    {
        if (registroActualizacion.IdReferencia > 0)
        {
            List<int> idsReferencias = new List<int>();
            List<int> idsSucesosRelacioneados = new List<int>();

            // Separar IdReferencia según su tipo
            foreach (var detalle in registroActualizacion.DetallesRegistro)
            {
                switch (detalle.IdApartadoRegistro)
                {
                    case (int)ApartadoRegistroEnum.SucesosRelacionados:
                        idsSucesosRelacioneados.Add(detalle.IdReferencia);
                        break;
                    default:
                        idsReferencias.Add(detalle.IdReferencia);
                        break;
                }
            }

            // Buscar el suceso relacionado por IdReferencia
            var sucesoRelacionado = await _unitOfWork.Repository<SucesoRelacionado>()
                .GetByIdWithSpec(new SucesosRelacionadosActiveByIdPrincipalSpecification(registroActualizacion.IdReferencia, idsSucesosRelacioneados));

            if (sucesoRelacionado is null || sucesoRelacionado.Borrado)
                throw new BadRequestException($"El registro de actualización con Id [{registroActualizacion.Id}] no tiene registro de Otra Informacion");

            return sucesoRelacionado;
        }

        // Validar si ya existe un registro de Dirección y Coordinación de Emergencia para este suceso
        var specSuceso = new SucesosRelacionadosWithDetails(new SucesoRelacionadoParams { IdSucesoPrincipal = request.IdSuceso });
        var sucesoRelacionadoExistente = await _unitOfWork.Repository<SucesoRelacionado>().GetByIdWithSpec(specSuceso);

        return sucesoRelacionadoExistente ?? new SucesoRelacionado { IdSucesoPrincipal = request.IdSuceso };
    }

    private async Task ValidateProcedenciasDestinosAsync(ManageSucesoRelacionadosCommand request)
    {
        if (request.IdsSucesosAsociados != null && request.IdsSucesosAsociados.Count > 0)
        {
            var idsSucesosAsociados = request.IdsSucesosAsociados
                .Distinct();

            var sucesosAsociadosExistentes = await _unitOfWork.Repository<Suceso>().GetAsync(ic => idsSucesosAsociados.Contains(ic.Id));

            if (sucesosAsociadosExistentes.Count() != idsSucesosAsociados.Count())
            {
                var idsSucesosAsiciadosExistentes = sucesosAsociadosExistentes.Select(ic => ic.Id).ToList();
                var idsSucesosAsociadosInvalidos = idsSucesosAsociados.Except(idsSucesosAsiciadosExistentes).ToList();

                if (idsSucesosAsociadosInvalidos.Any())
                {
                    _logger.LogWarning($"Los siguientes Id's de sucesos asociados: {string.Join(", ", idsSucesosAsociadosInvalidos)}, no se encontraron");
                    throw new NotFoundException(nameof(Suceso), string.Join(", ", idsSucesosAsociadosInvalidos));
                }
            }
        }
    }



}
