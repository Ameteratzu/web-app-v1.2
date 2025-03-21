using DGPCE.Sigemad.Application.Helpers;
using DGPCE.Sigemad.Application.Resources;
using DGPCE.Sigemad.Domain.Enums;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosFronteras.Commands.UpdateOpeDatosFronteras;

public class UpdateOpeDatoFronteraCommandValidator : AbstractValidator<UpdateOpeDatoFronteraCommand>
{
    public UpdateOpeDatoFronteraCommandValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(p => p.Id)
            .NotEmpty().WithMessage(localizer["IdNoVacio"])
            .NotNull().WithMessage(localizer["IdObligatorio"]);

        RuleFor(p => p.FechaHoraInicioIntervalo)
            .NotEmpty().WithMessage(localizer["FFechaHoraInicioIntervaloObligatorio"]);

        RuleFor(p => p.FechaHoraFinIntervalo)
            .NotEmpty().WithMessage(localizer["FechaHoraFinIntervaloObligatorio"]);

        RuleFor(p => p.NumeroVehiculos)
           .NotEmpty().WithMessage(localizer["NumeroVehiculosObligatorio"]);

        RuleFor(p => p.Afluencia)
           .NotEmpty().WithMessage(localizer["AfluenciaObligatorio"]);
    }
}
