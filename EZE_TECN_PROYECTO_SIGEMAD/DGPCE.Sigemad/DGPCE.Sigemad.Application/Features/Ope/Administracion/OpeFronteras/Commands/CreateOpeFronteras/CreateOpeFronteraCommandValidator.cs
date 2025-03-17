using DGPCE.Sigemad.Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeFronteras.Commands.CreateOpeFronteras;

public class CreateOpeFronteraCommandValidator : AbstractValidator<CreateOpeFronteraCommand>
{
    public CreateOpeFronteraCommandValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(p => p.Nombre)
            .NotEmpty().WithMessage(localizer["NombreNoVacio"])
            .NotNull().WithMessage(localizer["NombreObligatorio"])
            .MaximumLength(255).WithMessage(localizer["NombreMaxLength"]);
    }
}
