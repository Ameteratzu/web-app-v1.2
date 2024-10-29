using DGPCE.Sigemad.Application.Helpers;
using FluentValidation;

namespace DGPCE.Sigemad.Application.Features.AreasAfectadas.Commands.CreateAreasAfectadas;
public class CreateAreaAfectadaCommandValidator : AbstractValidator<CreateAreaAfectadaCommand>
{
    public CreateAreaAfectadaCommandValidator()
    {
        RuleFor(p => p.IdEvolucion)
            .GreaterThan(0).WithMessage("Es obligatorio y debe ser mayor a 0");

        RuleFor(p => p.FechaHora)
            .NotEmpty().WithMessage("FechaHora es obligatoria");

        RuleFor(p => p.IdProvincia)
            .GreaterThan(0).WithMessage("Es obligatorio y debe ser mayor a 0");

        RuleFor(p => p.IdMunicipio)
            .GreaterThan(0).WithMessage("Es obligatorio y debe ser mayor a 0");

        RuleFor(p => p.IdEntidadMenor)
            .GreaterThan(0).WithMessage("Es obligatorio y debe ser mayor a 0");

        RuleFor(p => p.GeoPosicion)
          .NotNull().WithMessage("GeoPosicion es obligatorio")
         .Must(GeoJsonValidatorUtil.IsGeometryInWgs84).WithMessage("La geometría no es válida, sistema de referencia no es Wgs84");
    }

}
