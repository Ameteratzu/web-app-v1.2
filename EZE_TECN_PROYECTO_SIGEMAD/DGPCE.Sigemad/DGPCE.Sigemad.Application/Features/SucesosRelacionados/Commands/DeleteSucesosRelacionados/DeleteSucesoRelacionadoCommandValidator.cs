using DGPCE.Sigemad.Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace DGPCE.Sigemad.Application.Features.SucesosRelacionados.Commands.DeleteSucesosRelacionados;
public class DeleteSucesoRelacionadoCommandValidator : AbstractValidator<DeleteSucesoRelacionadoCommand>
{
    public DeleteSucesoRelacionadoCommandValidator(IStringLocalizer<ValidationMessages> localizer)
    {        
        RuleFor(p => p.IdSucesoAsociado)
            .NotEmpty().WithMessage(localizer["IdSucesoAsociadoNoVacio"])
            .NotEqual(0).WithMessage(localizer["IdSucesoAsociadoObligatorio"]);
        RuleFor(p => p.IdSucesoPrincipal)
            .NotEmpty().WithMessage(localizer["IdSucesoPrincipalNoVacio"])
            .NotEqual(0).WithMessage(localizer["IdSucesoPrincipalObligatorio"]);
    }
}