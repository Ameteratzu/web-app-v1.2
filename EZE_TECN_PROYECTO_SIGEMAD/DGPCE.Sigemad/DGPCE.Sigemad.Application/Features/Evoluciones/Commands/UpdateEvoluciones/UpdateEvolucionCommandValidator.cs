using DGPCE.Sigemad.Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace DGPCE.Sigemad.Application.Features.Evoluciones.Commands.UpdateEvoluciones
{
    public class UpdateEvolucionCommandValidator : AbstractValidator<UpdateEvolucionCommand>
    {
        public UpdateEvolucionCommandValidator(IStringLocalizer<ValidationMessages> localizer)
        {

            RuleFor(p => p.Id)
               .GreaterThan(0).WithMessage(localizer["IncendioObligatorio"]);

            RuleFor(p => p.IdIncendio)
               .GreaterThan(0).WithMessage(localizer["IdObligatorio"]);
        }


    }
}
