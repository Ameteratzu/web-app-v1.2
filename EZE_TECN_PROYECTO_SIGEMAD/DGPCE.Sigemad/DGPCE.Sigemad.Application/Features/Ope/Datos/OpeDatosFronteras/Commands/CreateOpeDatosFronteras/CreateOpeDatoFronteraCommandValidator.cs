using DGPCE.Sigemad.Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosFronteras.Commands.CreateOpeDatosFronteras;

public class CreateOpeDatoFronteraCommandValidator : AbstractValidator<CreateOpeDatoFronteraCommand>
{
    public CreateOpeDatoFronteraCommandValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(p => p.FechaHoraInicioIntervalo)
            .NotEmpty().WithMessage(localizer["FechaHoraInicioIntervaloObligatorio"]);

        RuleFor(p => p.FechaHoraFinIntervalo)
            .NotEmpty().WithMessage(localizer["FechaHoraFinIntervaloObligatorio"]);

        RuleFor(p => p.NumeroVehiculos)
           .NotEmpty().WithMessage(localizer["NumeroVehiculosObligatorio"]);

        RuleFor(p => p.Afluencia)
            .NotEmpty().WithMessage(localizer["AfluenciaObligatorio"]);

    }
}
