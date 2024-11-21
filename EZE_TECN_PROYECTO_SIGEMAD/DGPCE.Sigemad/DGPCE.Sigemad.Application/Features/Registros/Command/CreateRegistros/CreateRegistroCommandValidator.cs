
using DGPCE.Sigemad.Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace DGPCE.Sigemad.Application.Features.Registros.Command.CreateRegistros;
public class CreateRegistroCommandValidator : AbstractValidator<CreateRegistroCommand>
{

    public CreateRegistroCommandValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(p => p.IdIncendio)
                 .GreaterThan(0).WithMessage(localizer["IncendioObligatorio"]);

    }
}
