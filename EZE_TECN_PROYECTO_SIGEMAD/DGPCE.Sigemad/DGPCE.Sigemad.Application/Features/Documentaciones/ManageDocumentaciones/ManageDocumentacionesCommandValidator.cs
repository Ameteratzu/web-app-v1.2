using DGPCE.Sigemad.Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;


namespace DGPCE.Sigemad.Application.Features.Documentaciones.ManageDocumentaciones;


public class ManageDocumentacionesCommandListValidator : AbstractValidator<ManageDocumentacionesCommandList>
{
    public ManageDocumentacionesCommandListValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(x => x.IdIncendio)
       .NotEmpty().WithMessage(localizer["IdIncendio is required."]);

        RuleForEach(x => x.Documentaciones).SetValidator(new ManageDocumentacionesCommandValidator(localizer));
    }
}


public class ManageDocumentacionesCommandValidator : AbstractValidator<ManageDocumentacionesCommand>
{

    public ManageDocumentacionesCommandValidator(IStringLocalizer<ValidationMessages> localizer)
    {

        RuleFor(x => x.FechaHora)
            .NotEmpty().WithMessage(localizer["FechaHora is required."]);

        RuleFor(x => x.FechaHoraSolicitud)
            .NotEmpty().WithMessage(localizer["FechaHoraSolicitud is required."]);

        RuleFor(x => x.IdTipoDocumento)
            .NotEmpty().WithMessage(localizer["IdTipoDocumento is required."]);

        RuleFor(x => x.Descripcion)
            .NotEmpty().WithMessage(localizer["Descripcion is required."])
            .MaximumLength(255).WithMessage(localizer["Descripcion must not exceed 255 characters."]);

        RuleFor(x => x.IdArchivo)
            .NotEmpty().WithMessage(localizer["IdArchivo is required."]);
    }

}
