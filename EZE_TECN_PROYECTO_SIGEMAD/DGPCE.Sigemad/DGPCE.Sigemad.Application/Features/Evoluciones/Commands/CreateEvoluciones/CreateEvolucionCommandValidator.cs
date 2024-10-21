using DGPCE.Sigemad.Application.Helpers;
using FluentValidation;

namespace DGPCE.Sigemad.Application.Features.Evoluciones.Commands.CreateEvoluciones;

public class CreateEvolucionCommandValidator : AbstractValidator<CreateEvolucionCommand>
{

    public CreateEvolucionCommandValidator()
    {

        RuleFor(p => p.IdIncendio)
             .GreaterThan(0).WithMessage("Es obligatorio y debe ser mayor a 0");

        RuleFor(p => p.IdEntradaSalida)
             .GreaterThan(0).WithMessage("Es obligatorio y debe ser mayor a 0");

        RuleFor(p => p.IdTipoRegistro)
             .GreaterThan(0).WithMessage("Es obligatorio y debe ser mayor a 0");

        RuleFor(p => p.IdMedio)
             .GreaterThan(0).WithMessage("Es obligatorio y debe ser mayor a 0");

        RuleFor(p => p.IdTecnico)
            .NotEmpty().WithMessage("IdTecnico no puede estar en blanco")
            .NotNull().WithMessage("IdTecnico es obligatorio");

        RuleFor(p => p.IdEntidadMenor)
                .GreaterThan(0).WithMessage("Es obligatorio y debe ser mayor a 0");

        RuleFor(p => p.IdEstadoIncendio)
             .GreaterThan(0).WithMessage("Es obligatorio y debe ser mayor a 0");

        RuleFor(p => p.IdMunicipioAfectado)
             .GreaterThan(0).WithMessage("Es obligatorio y debe ser mayor a 0");

        RuleFor(p => p.IdProvinciaAfectada)
            .GreaterThan(0).WithMessage("Es obligatorio y debe ser mayor a 0");

        RuleFor(p => p.FechaHoraEvolucion)
          .NotEmpty().WithMessage("FechaHoraEvolucion es obligatoria");

        RuleFor(p => p.GeoPosicionAreaAfectada)
            .NotEmpty().WithMessage("GeoPosicionAreaAfectada no puede estar en blanco")
            .NotNull().WithMessage("GeoPosicionAreaAfectada es obligatorio")
            .Must(GeoJsonValidatorUtil.IsGeometryInWgs84).WithMessage("La geometría no es válida, sistema de referencia no es Wgs84");

    }
}