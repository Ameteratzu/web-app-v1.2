using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.EstadosEvolucion.Enumerations;
using DGPCE.Sigemad.Application.Features.EstadosIncendio.Enumerations;
using DGPCE.Sigemad.Application.Features.Incendios.Commands.UpdateIncendios;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Evoluciones.Helpers
{
    public class EvolucionUtils
    {
  
       public async static void cambiarEstadoIncendioPorEstadoEvolucion(int estadoEvolucion, int IdIncendio, ILogger _logger, IUnitOfWork _unitOfWork)
        {

            _logger.LogInformation($"Comprobando estado evolución del incendio: {IdIncendio}");
            var incendioToUpdate = await _unitOfWork.Repository<Incendio>().GetByIdAsync(IdIncendio);
            bool actualizarIncendio = false;

            if ((EstadoEvolucionEnumeration)estadoEvolucion == EstadoEvolucionEnumeration.Extinguido)
            {
                incendioToUpdate.IdEstado = (int)EstadoIncendioEnumeration.Cerrado;
                actualizarIncendio = true;
                _logger.LogInformation($"Se actualizo correctamente el estado del incendio: {IdIncendio} a cerrado");

            }

            if (actualizarIncendio)
            {
                _unitOfWork.Repository<Incendio>().UpdateEntity(incendioToUpdate);
                await _unitOfWork.Complete();
            }

        }

    }
}
