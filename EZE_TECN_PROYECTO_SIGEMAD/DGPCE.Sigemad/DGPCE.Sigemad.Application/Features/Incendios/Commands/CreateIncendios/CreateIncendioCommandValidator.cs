using DGPCE.Sigemad.Application.Helpers;
using FluentValidation;

namespace DGPCE.Sigemad.Application.Features.Incendios.Commands.CreateIncendios;

public class CreateIncendioCommandValidator: AbstractValidator<CreateIncendioCommand>
{
    public CreateIncendioCommandValidator()
    {
        RuleFor(p => p.IdTerritorio)
            .GreaterThan(0).WithMessage("Es obligatorio y debe ser mayor a 0");

        RuleFor(p => p.IdPais)
            .GreaterThan(0).WithMessage("Es obligatorio y debe ser mayor a 0");

        RuleFor(p => p.IdProvincia)
            .GreaterThan(0).WithMessage("Es obligatorio y debe ser mayor a 0");

        RuleFor(p => p.IdMunicipio)
            .GreaterThan(0).WithMessage("Es obligatorio y debe ser mayor a 0");

        RuleFor(p => p.Denominacion)
            .NotEmpty().WithMessage("Denominacion no puede estar en blanco")
            .NotNull().WithMessage("Denominacion es obligatorio")
            .MaximumLength(255).WithMessage("Denominacion no puede exceder los 255 caracteres");

        RuleFor(p => p.FechaInicio)
            .NotEmpty().WithMessage("FechaInicio es obligatoria");

        RuleFor(p => p.IdTipoSuceso)
            .GreaterThan(0).WithMessage("Es obligatorio y debe ser mayor a 0");

        RuleFor(p => p.IdClaseSuceso)
            .GreaterThan(0).WithMessage("Es obligatorio y debe ser mayor a 0");

        RuleFor(p => p.IdEstadoSuceso)
            .GreaterThan(0).WithMessage("Es obligatorio y debe ser mayor a 0");

        RuleFor(p => p.Comentarios)
            .NotEmpty().WithMessage("Comentarios no puede estar en blanco");

        RuleFor(p => p.GeoPosicion)
            .NotNull().WithMessage("GeoPosicion es obligatorio")
            .Must(GeoJsonValidatorUtil.IsGeometryInWgs84).WithMessage("La geometría no es válida, sistema de referencia no es Wgs84");
    }
}
