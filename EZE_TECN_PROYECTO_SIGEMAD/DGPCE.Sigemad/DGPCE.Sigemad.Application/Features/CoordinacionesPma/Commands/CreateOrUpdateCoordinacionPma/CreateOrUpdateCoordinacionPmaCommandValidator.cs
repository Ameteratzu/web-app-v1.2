﻿using DGPCE.Sigemad.Application.Dtos.CoordinacionesPMA;
using DGPCE.Sigemad.Application.Resources;
using DGPCE.Sigemad.Domain.Constracts;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace DGPCE.Sigemad.Application.Features.CoordinacionesPma.Commands.CreateOrUpdateCoordinacionPma;
internal class CreateOrUpdateCoordinacionPmaCommandValidator : AbstractValidator<CreateOrUpdateCoordinacionPmaCommand>
{
    public CreateOrUpdateCoordinacionPmaCommandValidator(IStringLocalizer<ValidationMessages> localizer, IGeometryValidator geometryValidator)
    {
        RuleFor(x => x.IdSuceso)
            .GreaterThan(0).WithMessage(localizer["IdSucesoObligatorio"]);

        RuleForEach(x => x.Coordinaciones)
            .SetValidator(new CoordinacionPmaDtoValidator(localizer, geometryValidator))
            .When(d => d.Coordinaciones.Count > 0);
    }
}

public class CoordinacionPmaDtoValidator : AbstractValidator<CreateOrUpdateCoordinacionPmaDto>
{
    public CoordinacionPmaDtoValidator(IStringLocalizer<ValidationMessages> localizer, IGeometryValidator geometryValidator)
    {

        RuleFor(x => x.Lugar)
            .NotEmpty().WithMessage(localizer["LugarObligatorio"]);

        RuleFor(d => d.FechaInicio)
            .NotEmpty().WithMessage(localizer["FechaInicioObligatorio"])
            .LessThanOrEqualTo(d => d.FechaFin).When(d => d.FechaFin.HasValue)
            .WithMessage(localizer["FechaInicioDebeSerMenorQueFechaFin"]);

        RuleFor(p => p.IdProvincia)
           .NotNull().WithMessage(localizer["ProvinciaObligatorio"])
           .GreaterThan(0).WithMessage(localizer["ProvinciaInvalido"]);

        RuleFor(p => p.IdMunicipio)
            .NotNull().WithMessage(localizer["MunicipioObligatorio"])
            .GreaterThan(0).WithMessage(localizer["MunicipioInvalido"]);

        RuleFor(p => p.GeoPosicion)
            .Must(geometry => geometryValidator.IsGeometryValidAndInEPSG4326(geometry))
            .When(p => p.GeoPosicion != null)
            .WithMessage(localizer["GeoPosicionInvalida"]);
    }
}
