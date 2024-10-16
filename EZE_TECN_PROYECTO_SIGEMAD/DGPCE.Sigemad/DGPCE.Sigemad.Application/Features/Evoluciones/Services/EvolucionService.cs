using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.EstadosIncendio.Enumerations;
using DGPCE.Sigemad.Application.Features.EstadosSucesos.Enumerations;
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

        public async Task CambiarEstadoIncendioDesdeEstadoEvolucion(int estadoIncendio, int IdIncendio)
        {

            _logger.LogInformation($"Comprobando estado del incendio: {IdIncendio}");
            var incendioActualizar = await _unitOfWork.Repository<Incendio>().GetByIdAsync(IdIncendio);
            bool actualizarIncendio = false;

            if (incendioActualizar !=null &&
                (EstadoIncendioEnumeration)estadoIncendio == EstadoIncendioEnumeration.Extinguido &&
                (EstadoSucesoEnumeration)incendioActualizar.IdEstadoSuceso !=  EstadoSucesoEnumeration.Cerrado)
            {
                incendioActualizar.IdEstadoSuceso = (int)EstadoSucesoEnumeration.Cerrado;
                actualizarIncendio = true;
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

  
    }
}
