using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.Evoluciones;
using DGPCE.Sigemad.Application.Specifications.RegistrosActualizaciones;
using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Evoluciones.Commands.DeleteEvolucionesByIdRegistro;
public class DeleteEvolucionByIdRegistroCommandHandler : IRequestHandler<DeleteEvolucionByIdRegistroCommand>
{
    private readonly ILogger<DeleteEvolucionByIdRegistroCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly List<int> _idsEstadosCreados = new()
    {
        (int)EstadoRegistroEnum.Creado,
        (int)EstadoRegistroEnum.CreadoYModificado
    };

    public DeleteEvolucionByIdRegistroCommandHandler(
        ILogger<DeleteEvolucionByIdRegistroCommandHandler> logger,
        IUnitOfWork unitOfWork
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteEvolucionByIdRegistroCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"BEGIN - {nameof(DeleteEvolucionByIdRegistroCommandHandler)}");

        RegistroActualizacion registro = await ObtenerRegistroActualizacion(request.IdRegistroActualizacion);
        Evolucion evolucion = await ObtenerEvolucion(registro.IdSuceso);

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            // Eliminar intervencion de medios
            List<int> idsIntervencionAEliminar = registro.DetallesRegistro
                .Where(dr => dr.IdApartadoRegistro == (int)ApartadoRegistroEnum.IntervencionMedios && _idsEstadosCreados.Contains((int)dr.IdEstadoRegistro))
                .Select(dr => dr.IdReferencia)
                .ToList();

            var intervencionesEliminar = evolucion.IntervencionMedios.Where(item => idsIntervencionAEliminar.Contains(item.Id)).ToList();

            foreach (IntervencionMedio intervencion in intervencionesEliminar)
            {
                foreach (DetalleIntervencionMedio detalle in intervencion.DetalleIntervencionMedios)
                {
                    _unitOfWork.Repository<DetalleIntervencionMedio>().DeleteEntity(detalle);
                }
                _unitOfWork.Repository<IntervencionMedio>().DeleteEntity(intervencion);
            }

            // Eliminar Consecuencia/Actuacion
            List<int> idsConsecuenciaActuacionAEliminar = registro.DetallesRegistro
                .Where(dr => dr.IdApartadoRegistro == (int)ApartadoRegistroEnum.ConsecuenciaActuacion && _idsEstadosCreados.Contains((int)dr.IdEstadoRegistro))
                .Select(dr => dr.IdReferencia)
                .ToList();

            var consecuenciasEliminar = evolucion.Impactos.Where(item => idsConsecuenciaActuacionAEliminar.Contains(item.Id)).ToList();

            foreach (ImpactoEvolucion consecuencia in consecuenciasEliminar)
            {
                _unitOfWork.Repository<ImpactoEvolucion>().DeleteEntity(consecuencia);
            }


            // Eliminar area afectada
            List<int> idsAreaAfectadaAEliminar = registro.DetallesRegistro
                .Where(dr => dr.IdApartadoRegistro == (int)ApartadoRegistroEnum.AreaAfectada && _idsEstadosCreados.Contains((int)dr.IdEstadoRegistro))
                .Select(dr => dr.IdReferencia)
                .ToList();

            var areasEliminar = evolucion.AreaAfectadas.Where(item => idsAreaAfectadaAEliminar.Contains(item.Id)).ToList();

            foreach (var area in areasEliminar)
            {
                _unitOfWork.Repository<AreaAfectada>().DeleteEntity(area);
            }


            // Eliminar parametro
            List<int> idsParametroAEliminar = registro.DetallesRegistro
                .Where(dr => dr.IdApartadoRegistro == (int)ApartadoRegistroEnum.Parametro && _idsEstadosCreados.Contains((int)dr.IdEstadoRegistro))
                .Select(dr => dr.IdReferencia)
                .ToList();

            var parametrosEliminar = evolucion.Parametros.Where(item => idsParametroAEliminar.Contains(item.Id)).ToList();
            foreach (var parametro in parametrosEliminar)
            {
                _unitOfWork.Repository<Parametro>().DeleteEntity(parametro);
            }


            // Eliminar dato principal
            List<int> idsDatoPrincipalAEliminar = registro.DetallesRegistro
                .Where(dr => dr.IdApartadoRegistro == (int)ApartadoRegistroEnum.DatoPrincipal && _idsEstadosCreados.Contains((int)dr.IdEstadoRegistro))
                .Select(dr => dr.IdReferencia)
                .ToList();

            if (idsDatoPrincipalAEliminar.Contains(evolucion.DatoPrincipal.Id))
            {
                _unitOfWork.Repository<DatoPrincipal>().DeleteEntity(evolucion.DatoPrincipal);
            }


            // Eliminar registro
            List<int> idsRegistroAEliminar = registro.DetallesRegistro
                .Where(dr => dr.IdApartadoRegistro == (int)ApartadoRegistroEnum.Registro && _idsEstadosCreados.Contains((int)dr.IdEstadoRegistro))
                .Select(dr => dr.IdReferencia)
                .ToList();

            if (idsRegistroAEliminar.Contains(evolucion.Registro.Id))
            {
                _unitOfWork.Repository<Registro>().DeleteEntity(evolucion.Registro);
            }


            _unitOfWork.Repository<Evolucion>().UpdateEntity(evolucion);

            if (await _unitOfWork.Complete() <= 0)
                throw new Exception("No se pudo insertar/actualizar la Evolucion");


            // -----------------------------------------------------------

            // Eliminar detalles de actualización
            foreach (var detalle in registro.DetallesRegistro)
            {
                await _unitOfWork.Repository<DetalleRegistroActualizacion>().DeleteAsync(detalle);
            }

            // Eliminar el registro de actualización
            await _unitOfWork.Repository<RegistroActualizacion>().DeleteAsync(registro);


            await _unitOfWork.CommitAsync();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Error en la transacción de CreateOrUpdateDireccionCommandHandler");
            throw;
        }





        _logger.LogInformation($"END - {nameof(DeleteEvolucionByIdRegistroCommandHandler)}");

        return Unit.Value;
    }

    private async Task<RegistroActualizacion> ObtenerRegistroActualizacion(int idRegistroActualizacion)
    {
        var spec = new RegistroActualizacionSpecification(new RegistroActualizacionSpecificationParams { Id = idRegistroActualizacion });
        var registro = await _unitOfWork.Repository<RegistroActualizacion>().GetByIdWithSpec(spec);

        if (registro is null || registro.Borrado || registro.IdTipoRegistroActualizacion != (int)TipoRegistroActualizacionEnum.Evolucion)
        {
            _logger.LogWarning($"RegistroActualizacion no encontrado o inválido | IdRegistroActualizacion: {idRegistroActualizacion}");
            throw new NotFoundException(nameof(RegistroActualizacion), idRegistroActualizacion);
        }

        return registro;
    }

    private async Task<Evolucion> ObtenerEvolucion(int idSuceso)
    {
        var spec = new EvolucionSpecification(new EvolucionSpecificationParams { IdSuceso = idSuceso });
        var evolucion = await _unitOfWork.Repository<Evolucion>().GetByIdWithSpec(spec);

        if (evolucion is null || evolucion.Borrado)
        {
            _logger.LogWarning($"Evolución no encontrada o inválida | IdSuceso: {idSuceso}");
            throw new NotFoundException(nameof(Evolucion), idSuceso);
        }

        return evolucion;
    }

}
