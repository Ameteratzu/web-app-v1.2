using DGPCE.Sigemad.Application.Dtos.CoordinacionesPMA;
using DGPCE.Sigemad.Application.Resources;
using DGPCE.Sigemad.Domain.Constracts;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace DGPCE.Sigemad.Application.Features.CoordinacionesPma.Commands.CreateOrUpdateCoordinacionPma;
internal class CreateOrUpdateCoordinacionPmaCommandValidator : AbstractValidator<CreateOrUpdateCoordinacionPmaCommand>
{
    public CreateOrUpdateCoordinacionPmaCommandValidator(IStringLocalizer<ValidationMessages> localizer, IGeometryValidator geometryValidator)
    {
        RuleFor(x => x.IdIncendio)
            .GreaterThan(0).WithMessage(localizer["IdIncendioObligatorio"]);

        RuleFor(command => command.Coordinaciones)
            .NotNull().WithMessage(localizer["CoordinacionesObligatorio"])
            .NotEmpty().WithMessage(localizer["CoordinacionesObligatorio"]);

        RuleForEach(x => x.Coordinaciones).SetValidator(new CoordinacionPmaDtoValidator(localizer, geometryValidator));
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
            .LessThanOrEqualTo(d => d.FechaFin).WithMessage(localizer["FechaInicioDebeSerMenorQueFechaFin"]);

        RuleFor(d => d.FechaFin)
            .NotEmpty().WithMessage(localizer["FechaFinObligatorio"]);

        RuleFor(p => p.IdProvincia)
           .NotNull().WithMessage(localizer["ProvinciaObligatorio"])
           .GreaterThan(0).WithMessage(localizer["ProvinciaInvalido"]);

        RuleFor(p => p.IdMunicipio)
            .NotNull().WithMessage(localizer["MunicipioObligatorio"])
            .GreaterThan(0).WithMessage(localizer["MunicipioInvalido"]);

        RuleFor(p => p.GeoPosicion)
            .NotNull().When(p => p.GeoPosicion != null)
            .Must(geometryValidator.IsGeometryValidAndInEPSG4326).WithMessage(localizer["GeoPosicionInvalida"]);
    }
}
