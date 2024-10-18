using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.EstadosIncendio.Enumerations;
using DGPCE.Sigemad.Application.Features.EstadosSucesos.Enumerations;
using DGPCE.Sigemad.Application.Features.Evoluciones.Commands.CreateEvoluciones;
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


        public async Task<Evolucion>  CrearNuevaEvolucion(CreateEvolucionCommand request)
        {
            var evolucion = new Evolucion
            {
                IdIncendio = request.IdIncendio,
                IdEstadoIncendio = request.IdEstadoIncendio,
                FechaHoraEvolucion = request.FechaHoraEvolucion,
                IdEntradaSalida = request.IdEntradaSalida,
                IdMedio = request.IdMedio,
                IdTecnico = request.IdTecnico,
                IdEntidadMenor = request.IdEntidadMenor,
                Resumen = request.Resumen,
                Observaciones = request.Observaciones,
                Prevision = request.Prevision,
                SuperficieAfectadaHectarea = request.SuperficieAfectadaHectarea,
                FechaFinal = request.FechaFinal,
                IdProvinciaAfectada = request.IdProvinciaAfectada,
                IdMunicipioAfectado = request.IdMunicipioAfectado,
                GeoPosicionAreaAfectada = request.GeoPosicionAreaAfectada,
                IdTipoRegistro = request.IdTipoRegistro
            };

            _unitOfWork.Repository<Evolucion>().AddEntity(evolucion);

            var result = await _unitOfWork.Complete();
            if (result <= 0)
            {
                throw new Exception("No se pudo insertar nueva evolución");
            }

            _logger.LogInformation($"La evolución {evolucion.Id} fue creado correctamente");

            return evolucion;

        }

        public async Task CrearEvolucioneProcedenciaDestinos(int idEvolucion, ICollection<int> listadoProcedencias)
        {
            _logger.LogInformation($"Creando evolucionProcedenciasDestinos para la evolución {idEvolucion}");
            foreach (var procedenciaDestino in listadoProcedencias)
            {
                var evolucionProcedenciaDestino = new EvolucionProcedenciaDestino
                {
                    IdEvolucion = idEvolucion,
                    IdProcedenciaDestino = procedenciaDestino
                };

                await _unitOfWork.Repository<EvolucionProcedenciaDestino>().AddAsync(evolucionProcedenciaDestino);

            }

          await _unitOfWork.Complete();
          
         _logger.LogInformation($"evolucionProcedenciasDestinos creadas correctamente para la evolución {idEvolucion}");
        }




    }
}
