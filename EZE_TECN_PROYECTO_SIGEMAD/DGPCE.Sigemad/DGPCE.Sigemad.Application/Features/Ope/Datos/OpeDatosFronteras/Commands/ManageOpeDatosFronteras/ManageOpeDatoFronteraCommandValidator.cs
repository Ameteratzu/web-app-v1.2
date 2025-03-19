using DGPCE.Sigemad.Application.Dtos.Ope.Datos.OpeDatosFronteras;
using DGPCE.Sigemad.Application.Dtos.OtraInformaciones;
using DGPCE.Sigemad.Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosFronteras.Commands.CreateOpeDatosFronteras;

public class ManageOpeDatoFronteraCommandValidator : AbstractValidator<ManageOpeDatoFronteraCommand>
{
    public ManageOpeDatoFronteraCommandValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(x => x.IdOpeFrontera)
            .GreaterThan(0).WithMessage(localizer["IdOpeFronteraObligatorio"]);

        RuleForEach(x => x.Lista)
            .SetValidator(new OpeDatoFronteraDtoValidator(localizer));
    }
}

public class OpeDatoFronteraDtoValidator : AbstractValidator<CreateOpeDatoFronteraDto>
{
    public OpeDatoFronteraDtoValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(p => p.FechaHoraInicioIntervalo)
            .NotEmpty().WithMessage(localizer["FechaHoraInicioIntervaloObligatorio"]);

        RuleFor(p => p.FechaHoraFinIntervalo)
           .NotEmpty().WithMessage(localizer["FechaHoraFinIntervaloObligatorio"]);

    }
}
