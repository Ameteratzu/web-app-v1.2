using DGPCE.Sigemad.Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace DGPCE.Sigemad.Application.Features.SucesosRelacionados.Commands.UpdateSucesosRelacionados;
public class UpdateSucesoRelacionadoCommandValidator : AbstractValidator<UpdateSucesoRelacionadoCommand>
{
    public UpdateSucesoRelacionadoCommandValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(p => p.Id)
            .NotEmpty().WithMessage(localizer["IdNoVacio"])
            .NotEqual(0).WithMessage(localizer["IdNoVacio"]);
        RuleFor(p => p.IdSucesoAsociado)
            .NotEmpty().WithMessage(localizer["IdSucesoAsociadoNoVacio"])
            .NotEqual(0).WithMessage(localizer["IdSucesoAsociadoObligatorio"]);
        RuleFor(p => p.IdSucesoPrincipal)
            .NotEmpty().WithMessage(localizer["IdSucesoPrincipalNoVacio"])
            .NotEqual(0).WithMessage(localizer["IdSucesoPrincipalObligatorio"]);
    }
}
