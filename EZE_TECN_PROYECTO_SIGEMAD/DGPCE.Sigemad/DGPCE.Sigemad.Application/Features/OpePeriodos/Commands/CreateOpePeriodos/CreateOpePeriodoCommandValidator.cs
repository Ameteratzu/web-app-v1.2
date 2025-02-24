using DGPCE.Sigemad.Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace DGPCE.Sigemad.Application.Features.OpePeriodos.Commands.CreateOpePeriodos;

public class CreateOpePeriodoCommandValidator : AbstractValidator<CreateOpePeriodoCommand>
{
    public CreateOpePeriodoCommandValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(p => p.Denominacion)
            .NotEmpty().WithMessage(localizer["DenominacionNoVacio"])
            .NotNull().WithMessage(localizer["DenominacionObligatorio"])
            .MaximumLength(255).WithMessage(localizer["DenominacionMaxLength"]);

        RuleFor(p => p.FechaInicio)
            .NotEmpty().WithMessage(localizer["FechaInicioObligatorio"]);

        RuleFor(p => p.FechaFin)
            .NotEmpty().WithMessage(localizer["FechaFinObligatorio"]);

    }
}
