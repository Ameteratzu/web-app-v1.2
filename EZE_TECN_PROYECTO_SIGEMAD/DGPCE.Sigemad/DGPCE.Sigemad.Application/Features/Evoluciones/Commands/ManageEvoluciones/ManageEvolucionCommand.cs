using DGPCE.Sigemad.Application.Features.DatosPrincipales.Commands;
using DGPCE.Sigemad.Application.Features.Parametros.Commands;
using DGPCE.Sigemad.Application.Features.Registros.Command.CreateRegistros;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.Evoluciones.Commands.ManageEvoluciones
{
    public class ManageEvolucionCommand : IRequest<ManageEvolucionResponse>
    {
        public int IdIncendio { get; set; }
        public int? IdEvolucion { get; set; }
        public CreateRegistroCommand? Registro { get; set; }
        public CreateDatoPrincipalCommand? DatoPrincipal { get; set; }
        public CreateParametroCommand? Parametro { get; set; }

        public ICollection<int>? RegistroProcedenciasDestinos { get; set; }
    }
}
