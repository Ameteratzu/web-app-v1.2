using DGPCE.Sigemad.Application.Helpers;
using DGPCE.Sigemad.Application.Resources;
using DGPCE.Sigemad.Domain.Enums;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace DGPCE.Sigemad.Application.Features.OpePeriodos.Commands.UpdateOpePeriodos;

public class UpdateOpePeriodoCommandValidator : AbstractValidator<UpdateOpePeriodoCommand>
{
    public UpdateOpePeriodoCommandValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(p => p.Id)
            .NotEmpty().WithMessage(localizer["IdNoVacio"])
            .NotNull().WithMessage(localizer["IdObligatorio"]);


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
