using DGPCE.Sigemad.Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;


namespace DGPCE.Sigemad.Application.Features.Documentaciones.ManageDocumentaciones;


public class ManageDocumentacionesCommandListValidator : AbstractValidator<ManageDocumentacionesCommandList>
{
    public ManageDocumentacionesCommandListValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(x => x.IdIncendio)
         .GreaterThan(0).WithMessage(localizer["IncendioIdObligatorio"]);

        RuleForEach(x => x.Documentaciones).SetValidator(new ManageDocumentacionesCommandValidator(localizer));
    }
}

public class ManageDocumentacionesCommandValidator : AbstractValidator<ManageDocumentacionesCommand>
{
    public ManageDocumentacionesCommandValidator(IStringLocalizer<ValidationMessages> localizer)
    {

        RuleFor(x => x.FechaHora)
            .NotEmpty().WithMessage(localizer["FechaHoraObligatorio"]);

        RuleFor(x => x.FechaHoraSolicitud)
            .NotEmpty().WithMessage(localizer["FechaHoraSolicitud"]);

        RuleFor(x => x.IdTipoDocumento)
            .GreaterThan(0).WithMessage(localizer["IdTipoDocumentoObligatorio"]);

        RuleFor(x => x.Descripcion)
            .NotEmpty().WithMessage(localizer["DescripcionObligatorio"])
            .MaximumLength(255).WithMessage(localizer["DescripcionMaxLength"]);

        RuleFor(x => x.IdArchivo)
            .NotEmpty().WithMessage(localizer["IdArchivoObligatorio"]);
    }

}
