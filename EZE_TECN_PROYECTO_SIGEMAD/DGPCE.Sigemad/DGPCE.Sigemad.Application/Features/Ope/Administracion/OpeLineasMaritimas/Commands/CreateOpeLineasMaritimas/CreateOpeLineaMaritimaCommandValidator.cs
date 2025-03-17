using DGPCE.Sigemad.Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeLineasMaritimas.Commands.CreateOpeLineasMaritimas;

public class CreateOpeLineaMaritimaCommandValidator : AbstractValidator<CreateOpeLineaMaritimaCommand>
{
    public CreateOpeLineaMaritimaCommandValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(p => p.Nombre)
            .NotEmpty().WithMessage(localizer["NombreNoVacio"])
            .NotNull().WithMessage(localizer["NombreObligatorio"])
            .MaximumLength(255).WithMessage(localizer["NombreMaxLength"]);

        RuleFor(p => p.FechaValidezDesde)
            .NotEmpty().WithMessage(localizer["FechaValidezDesdeObligatorio"]);

        RuleFor(p => p.FechaValidezHasta)
            .NotEmpty().WithMessage(localizer["FechaValidezHastaObligatorio"]);

    }
}
