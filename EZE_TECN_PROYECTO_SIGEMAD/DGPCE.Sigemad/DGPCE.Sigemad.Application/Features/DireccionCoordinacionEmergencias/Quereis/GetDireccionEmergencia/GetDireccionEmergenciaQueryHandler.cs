using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Dtos.DireccionCoordinaciones;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.DireccionCoordinacionEmergencias;
using DGPCE.Sigemad.Application.Specifications.RegistrosActualizaciones;
using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Quereis.GetDireccionEmergencia;
public class GetDireccionCoordinacionEmergenciaQueryHandler : IRequestHandler<GetDireccionCoordinacionEmergenciaQuery, DireccionCoordinacionEmergenciaDto>
{
    private readonly ILogger<GetDireccionCoordinacionEmergenciaQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetDireccionCoordinacionEmergenciaQueryHandler(
        ILogger<GetDireccionCoordinacionEmergenciaQueryHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DireccionCoordinacionEmergenciaDto> Handle(GetDireccionCoordinacionEmergenciaQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(GetDireccionCoordinacionEmergenciaQueryHandler)} - BEGIN");

        DireccionCoordinacionEmergencia direccionCoordinacionEmergencia;
        List<int> idsReferencias = new List<int>();
        List<int> idsDirecciones = new List<int>();
        List<int> idsCecopi = new List<int>();
        List<int> idsPma = new List<int>();
        List<int> idsDireccionesEliminables = new List<int>();
        List<int> idsCecopiEliminables = new List<int>();
        List<int> idsPmaEliminables = new List<int>();

        if (request.IdRegistroActualizacion.HasValue)
        {
            _logger.LogInformation($"Buscando Dirección y Coordinación de Emergencia para IdRegistroActualizacion: {request.IdRegistroActualizacion}");

            // Obtener RegistroActualizacion con IdReferencia (DireccionCoordinacionEmergencia.Id)
            var registroSpec = new RegistroActualizacionSpecificationParams
            {
                Id = request.IdRegistroActualizacion.Value
            };
            var registroActualizacion = await _unitOfWork.Repository<RegistroActualizacion>()
                .GetByIdWithSpec(new RegistroActualizacionSpecification(registroSpec));

            if (registroActualizacion == null)
            {
                _logger.LogWarning($"No se encontró RegistroActualizacion con Id: {request.IdRegistroActualizacion}");
                throw new NotFoundException(nameof(RegistroActualizacion), request.IdRegistroActualizacion);
            }

            // Separar IdReferencia según su tipo
            foreach (var detalle in registroActualizacion.DetallesRegistro)
            {
                switch (detalle.IdApartadoRegistro)
                {
                    case (int)ApartadoRegistroEnum.Direccion:
                        idsDirecciones.Add(detalle.IdReferencia);
                        if(IsEliminable(detalle.IdEstadoRegistro)) idsDireccionesEliminables.Add(detalle.IdReferencia);
                        break;
                    case (int)ApartadoRegistroEnum.CoordinacionCECOPI:
                        idsCecopi.Add(detalle.IdReferencia);
                        if (IsEliminable(detalle.IdEstadoRegistro)) idsCecopiEliminables.Add(detalle.IdReferencia);
                        break;
                    case (int)ApartadoRegistroEnum.CoordinacionPMA:
                        idsPma.Add(detalle.IdReferencia);
                        if (IsEliminable(detalle.IdEstadoRegistro)) idsPmaEliminables.Add(detalle.IdReferencia);
                        break;
                }
            }

            // Buscar la Dirección y Coordinación de Emergencia por IdReferencia
            direccionCoordinacionEmergencia = await _unitOfWork.Repository<DireccionCoordinacionEmergencia>()
                .GetByIdWithSpec(new DireccionCoordinacionEmergenciaWithFilteredData(registroActualizacion.IdReferencia, idsDirecciones, idsCecopi, idsPma));
        }
        else
        {
            _logger.LogInformation($"Buscando Dirección y Coordinación de Emergencia para IdSuceso: {request.IdSuceso}");

            // Buscar DireccionCoordinacionEmergencia por IdSuceso
            direccionCoordinacionEmergencia = await _unitOfWork.Repository<DireccionCoordinacionEmergencia>()
                .GetByIdWithSpec(new DireccionCoordinacionEmergenciaActiveByIdSpecification(new DireccionCoordinacionEmergenciaSpecificationParams { IdSuceso = request.IdSuceso}));
        }

        if (direccionCoordinacionEmergencia == null)
        {
            _logger.LogWarning($"No se encontró Dirección y Coordinación de Emergencia para Suceso: {request.IdSuceso}");
            throw new NotFoundException(nameof(DireccionCoordinacionEmergencia), request.IdSuceso);
        }

        // Mapear y devolver DTO con los datos completos
        var response = _mapper.Map<DireccionCoordinacionEmergenciaDto>(direccionCoordinacionEmergencia);
        response.Direcciones.ForEach(d => d.EsEliminable = idsDireccionesEliminables.Contains(d.Id));
        response.CoordinacionesCecopi.ForEach(c => c.EsEliminable = idsCecopiEliminables.Contains(c.Id));
        response.CoordinacionesPMA.ForEach(p => p.EsEliminable = idsPmaEliminables.Contains(p.Id));

        _logger.LogInformation($"{nameof(GetDireccionCoordinacionEmergenciaQueryHandler)} - END");
        return response;
    }

    private bool IsEliminable(EstadoRegistroEnum estado)
    {
        return estado == EstadoRegistroEnum.Creado ||
            estado == EstadoRegistroEnum.CreadoYModificado;
    }
}