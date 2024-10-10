using DGPCE.Sigemad.Application.Features.Evoluciones.CreateEvolucion;
using DGPCE.Sigemad.Application.Helpers;
using FluentValidation;

namespace DGPCE.Sigemad.Application.Features.Evoluciones.CreateEvoluciones
{
    public class CreateEvolucionCommandValidator : AbstractValidator<CreateEvolucionCommand>
    {

        public CreateEvolucionCommandValidator()
        {

            RuleFor(p => p.IdIncendio)
                .NotEmpty().WithMessage("IdIncendio no puede estar en blanco")
                .NotNull().WithMessage("IdIncendio es obligatorio");

            RuleFor(p => p.IdEntradaSalida)
                .NotEmpty().WithMessage("IdEntradaSalida no puede estar en blanco")
                .NotNull().WithMessage("IdEntradaSalida es obligatorio");

            RuleFor(p => p.IdMedio)
                .NotEmpty().WithMessage("IdMedio no puede estar en blanco")
                .NotNull().WithMessage("IdMedio es obligatorio");

            RuleFor(p => p.IdProcedenciaDestino)
                .NotEmpty().WithMessage("IdProcedenciaDestino no puede estar en blanco")
                .NotNull().WithMessage("IdProcedenciaDestino es obligatorio");

            RuleFor(p => p.IdTecnico)
                .NotEmpty().WithMessage("IdTecnico no puede estar en blanco")
                .NotNull().WithMessage("IdTecnico es obligatorio");
    
            RuleFor(p => p.IdEstadoEvolucion)
                .NotEmpty().WithMessage("IdEstadoEvolucion no puede estar en blanco")
                .NotNull().WithMessage("IdEstadoEvolucion es obligatorio");

            RuleFor(p => p.IdMunicipioAfectado)
                .NotEmpty().WithMessage("IdMunicipioAfectado no puede estar en blanco")
                .NotNull().WithMessage("IdMunicipioAfectado es obligatorio");

            RuleFor(p => p.IdProvinciaAfectada)
                .NotEmpty().WithMessage("IdProvinciaAfectada no puede estar en blanco")
                .NotNull().WithMessage("IdProvinciaAfectada es obligatorio");

            RuleFor(p => p.FechaHoraEvolucion)
                .NotEmpty().WithMessage("FechaHoraEvolucion no puede estar en blanco")
                .NotNull().WithMessage("FechaHoraEvolucion es obligatorio");

            RuleFor(p => p.GeoPosicionAreaAfectada)
                .NotEmpty().WithMessage("GeoPosicionAreaAfectada no puede estar en blanco")
                .NotNull().WithMessage("GeoPosicionAreaAfectada es obligatorio")
                .Must(GeoJsonValidatorUtil.IsGeometryInWgs84).WithMessage("La geometría no es válida, sistema de referencia no es Wgs84");

        }
    }
}