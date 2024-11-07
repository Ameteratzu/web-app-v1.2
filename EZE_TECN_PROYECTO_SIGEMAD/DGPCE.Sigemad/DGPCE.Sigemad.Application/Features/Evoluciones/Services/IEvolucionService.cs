
using DGPCE.Sigemad.Application.Features.Evoluciones.Commands.CreateEvoluciones;
using DGPCE.Sigemad.Application.Features.Evoluciones.Commands.DeleteEvoluciones;
using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Features.Evoluciones.Services
{
    public interface IEvolucionService
    {
        Task CambiarEstadoSucesoIncendioEvolucion(int estadoEvolucion, int idIncendio);
        Task<bool> ComprobacionEvolucionProcedenciaDestinos(ICollection<int>? evolucionProcedenciaDestinos);
        Task<Evolucion> CrearNuevaEvolucion(CreateEvolucionCommand request);
        Task EliminarEvolucion(DeleteEvolucionesCommand request);
        ICollection<EvolucionProcedenciaDestino> MapEvolucionProcedenciaDestinos(ICollection<int>? source);
    }
}
