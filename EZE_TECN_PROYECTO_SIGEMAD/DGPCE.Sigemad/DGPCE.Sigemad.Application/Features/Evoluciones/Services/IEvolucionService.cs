
using DGPCE.Sigemad.Application.Features.Evoluciones.Commands.DeleteEvoluciones;

namespace DGPCE.Sigemad.Application.Features.Evoluciones.Services
{
    public interface IEvolucionService
    {
        Task CambiarEstadoSucesoIncendioEvolucion(int estadoEvolucion, int idIncendio);
        Task<bool> ComprobacionEvolucionProcedenciaDestinos(ICollection<int>? evolucionProcedenciaDestinos);
        Task EliminarEvolucion(DeleteEvolucionesCommand request);
    }
}
