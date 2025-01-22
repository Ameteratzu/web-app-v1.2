﻿using DGPCE.Sigemad.Application.Dtos.ActivacionSistema;
using DGPCE.Sigemad.Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace DGPCE.Sigemad.Application.Features.ActivacionesSistemas.Commands.ManageActivacionSistema;
public class ManageActivacionSistemaCommandValidator : AbstractValidator<ManageActivacionSistemaCommand>
{
    public ManageActivacionSistemaCommandValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(x => x.IdSuceso)
            .GreaterThan(0).WithMessage(localizer["IdSucesoObligatorio"]);

        RuleForEach(x => x.ActivacionSistemas).SetValidator(new ActivacionSistemaDtoValidator(localizer));
    }
}

public class ActivacionSistemaDtoValidator : AbstractValidator<ManageActivacionSistemaDto>
{
    public ActivacionSistemaDtoValidator(IStringLocalizer<ValidationMessages> localizer)
    {

        RuleFor(x => x.IdTipoSistemaEmergencia)
         .GreaterThan(0).WithMessage(localizer["IdTipoSistemaEmergencia"]);

        RuleFor(x => x.Autoridad)
            .NotEmpty().WithMessage(localizer["AutoridadObligatorio"])
            .MaximumLength(510).WithMessage(localizer["AutoridadMaxLength"]);

    }
}