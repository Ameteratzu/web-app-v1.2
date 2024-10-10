
namespace DGPCE.Sigemad.Application.Features.Evoluciones.Services
{
    public interface IEvolucionService
    {
        Task CambiarEstadoIncendioDesdeEstadoEvolucion(int estadoEvolucion, int idIncendio);

   
    }
}
