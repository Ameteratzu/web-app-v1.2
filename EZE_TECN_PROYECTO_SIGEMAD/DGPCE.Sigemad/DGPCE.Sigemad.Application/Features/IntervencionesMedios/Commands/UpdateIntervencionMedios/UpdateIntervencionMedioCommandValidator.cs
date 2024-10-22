using DGPCE.Sigemad.Application.Helpers;
using FluentValidation;

namespace DGPCE.Sigemad.Application.Features.IntervencionesMedios.Commands.UpdateIntervencionMedios;
public class UpdateIntervencionMedioCommandValidator : AbstractValidator<UpdateIntervencionMedioCommand>
{
    public UpdateIntervencionMedioCommandValidator()
    {
        RuleFor(p => p.IdEvolucion)
             .GreaterThan(0).WithMessage("Es obligatorio y debe ser mayor a 0");

        RuleFor(p => p.IdTipoIntervencionMedio)
             .GreaterThan(0).WithMessage("Es obligatorio y debe ser mayor a 0");

        RuleFor(p => p.IdCaracterMedio)
             .GreaterThan(0).WithMessage("Es obligatorio y debe ser mayor a 0");

        RuleFor(p => p.IdClasificacionMedio)
             .GreaterThan(0).WithMessage("Es obligatorio y debe ser mayor a 0");

        RuleFor(p => p.IdTitularidadMedio)
            .NotEmpty().WithMessage("IdTecnico no puede estar en blanco")
            .NotNull().WithMessage("IdTecnico es obligatorio");

        RuleFor(p => p.IdMunicipio)
                .GreaterThan(0).WithMessage("Es obligatorio y debe ser mayor a 0");

        RuleFor(p => p.Cantidad)
                .GreaterThan(0).WithMessage("Es obligatorio y debe ser mayor a 0");

        RuleFor(p => p.Unidad)
            .NotEmpty().WithMessage("No puede estar en blanco")
            .NotNull().WithMessage("Es obligatorio")
            .MaximumLength(100).WithMessage("No puede exceder los 100 caracteres");

        RuleFor(p => p.Titular)
            .NotEmpty().WithMessage("No puede estar en blanco")
            .NotNull().WithMessage("Es obligatorio")
            .MaximumLength(255).WithMessage("No puede exceder los 255 caracteres");

        RuleFor(p => p.GeoPosicion)
            .NotEmpty().WithMessage("No puede estar en blanco")
            .NotNull().WithMessage("Es obligatorio")
            .Must(GeoJsonValidatorUtil.IsGeometryInWgs84).WithMessage("La geometría no es válida, sistema de referencia no es Wgs84");
    }
}
