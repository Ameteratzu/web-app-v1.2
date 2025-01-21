﻿using DGPCE.Sigemad.Application.Dtos.Documentaciones;
using DGPCE.Sigemad.Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;


namespace DGPCE.Sigemad.Application.Features.Documentaciones.Commands.ManageDocumentaciones;


public class ManageDocumentacionesCommandListValidator : AbstractValidator<ManageDocumentacionesCommand>
{
    public ManageDocumentacionesCommandListValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(x => x.IdSuceso)
         .GreaterThan(0).WithMessage(localizer["IdSucesoObligatorio"]);

        RuleForEach(x => x.DetallesDocumentaciones)
            .SetValidator(new ManageDocumentacionesCommandValidator(localizer))
            .When(d => d.DetallesDocumentaciones.Count > 0);
    }
}

public class ManageDocumentacionesCommandValidator : AbstractValidator<DetalleDocumentacionDto>
{
    public ManageDocumentacionesCommandValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(x => x)
            .Must(HaveAtLeastOneFileProperty)
            .WithMessage(localizer["IdArchivoOrArchivo"]);

        RuleFor(x => x.FechaHora)
            .NotEmpty().WithMessage(localizer["FechaHoraObligatorio"]);

        RuleFor(x => x.FechaHoraSolicitud)
            .NotEmpty().WithMessage(localizer["FechaHoraSolicitud"]);

        RuleFor(x => x.IdTipoDocumento)
            .GreaterThan(0).WithMessage(localizer["IdTipoDocumentoObligatorio"]);

        RuleFor(x => x.Descripcion)
            .NotEmpty().WithMessage(localizer["DescripcionObligatorio"])
            .MaximumLength(255).WithMessage(localizer["DescripcionMaxLength"]);

    }

    private bool HaveAtLeastOneFileProperty(DetalleDocumentacionDto dto)
    {
        return dto.IdArchivo.HasValue || dto.Archivo != null;
    }

}
