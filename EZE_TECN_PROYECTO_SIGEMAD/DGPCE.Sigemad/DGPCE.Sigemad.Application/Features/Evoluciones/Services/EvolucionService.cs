using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.EstadosIncendio.Enumerations;
using DGPCE.Sigemad.Application.Features.EstadosSucesos.Enumerations;
using DGPCE.Sigemad.Application.Features.Evoluciones.Commands.DeleteEvoluciones;
using DGPCE.Sigemad.Application.Features.Evoluciones.Services;
using DGPCE.Sigemad.Domain.Modelos;

using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Evoluciones.Helpers
{
    public class EvolucionService : IEvolucionService
    {

        private readonly ILogger<EvolucionService> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public EvolucionService(ILogger<EvolucionService> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task CambiarEstadoSucesoIncendioEvolucion(int estadoIncendio, int IdIncendio)
        {

            _logger.LogInformation($"Comprobando estado del incendio: {IdIncendio}");
            var incendioActualizar = await _unitOfWork.Repository<Incendio>().GetByIdAsync(IdIncendio);
          
            bool actualizarIncendio = false;

            if (incendioActualizar !=null )
            {
                var estadoIncendiobsoleto = await _unitOfWork.Repository<EstadoIncendio>().GetByIdAsync((int)EstadoIncendioEnumeration.Extinguido);
                
                if (estadoIncendiobsoleto != null && !estadoIncendiobsoleto.Obsoleto &&
                (EstadoIncendioEnumeration)estadoIncendio == EstadoIncendioEnumeration.Extinguido &&
                (EstadoSucesoEnumeration)incendioActualizar.IdEstadoSuceso != EstadoSucesoEnumeration.Cerrado)
                {
                    incendioActualizar.IdEstadoSuceso = (int)EstadoSucesoEnumeration.Cerrado;
                    actualizarIncendio = true;
                }         
            }

            if (actualizarIncendio && incendioActualizar !=null)
            {
                _unitOfWork.Repository<Incendio>().UpdateEntity(incendioActualizar);
                await _unitOfWork.Complete();
                _logger.LogInformation($"Se actualizo correctamente el estado del suceso del incendio: {IdIncendio} a {(EstadoSucesoEnumeration)incendioActualizar.IdEstadoSuceso}");
            }
            else
            {
               _logger.LogInformation($"No se actualizo el estado del suceso del incendio {IdIncendio}");
            }

        }

        public async Task<bool> ComprobacionEvolucionProcedenciaDestinos(ICollection<int>? evolucionProcedenciasDestinos)
        {

            if (evolucionProcedenciasDestinos != null)
            {           
                foreach (var procedenciaDestino in evolucionProcedenciasDestinos)
                {
                    var procedencia = await _unitOfWork.Repository<ProcedenciaDestino>().GetByIdAsync(procedenciaDestino);
                    if (procedencia == null)
                    {
                        _logger.LogWarning($"evolucionProcedenciaDestino {procedenciaDestino}, no encontrado");
                        throw new NotFoundException(nameof(ProcedenciaDestino), procedenciaDestino);
                    }
                }
            }

            return true;
        }



        public async Task EliminarEvolucion(DeleteEvolucionesCommand request)
        {
            var evolucionToDelete = await _unitOfWork.Repository<Evolucion>().GetByIdAsync(request.Id);
            if (evolucionToDelete is null)
            {
                _logger.LogWarning($"La evolución con id:{request.Id}, no existe en la base de datos");
                throw new NotFoundException(nameof(Evolucion), request.Id);
            }

            if (evolucionToDelete.Borrado != null && !(bool)evolucionToDelete.Borrado)
            {
                evolucionToDelete.Borrado = true;
                evolucionToDelete.FechaEliminacion = DateTime.Now;
                _unitOfWork.Repository<Evolucion>().UpdateEntity(evolucionToDelete);
                await _unitOfWork.Complete();
                _logger.LogInformation($"La evolución con id: {request.Id}, se actualizo estado de borrado con éxito");
            }              
        }
    }
}
