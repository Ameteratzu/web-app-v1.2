using DGPCE.Sigemad.Application.Dtos.CoordinacionCecopis;
using DGPCE.Sigemad.Application.Features.CoordinacionCecopis.Commands.CreateCoordinacionCecopi;
using DGPCE.Sigemad.Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace DGPCE.Sigemad.Application.Features.CoordinacionCecopis.Commands.CreateOrUpdateCoordinacionCecopi;
public class CreateOrUpdateCoordinacionCecopiCommandValidator : AbstractValidator<CreateOrUpdateCoordinacionCecopiCommand>
{
    public CreateOrUpdateCoordinacionCecopiCommandValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(x => x.IdIncendio)
            .GreaterThan(0).WithMessage(localizer["IdIncendioObligatorio"]);

        RuleFor(command => command.Coordinaciones)
            .NotNull().WithMessage(localizer["CoordinacionesObligatorio"])
            .NotEmpty().WithMessage(localizer["CoordinacionesObligatorio"]);

        RuleForEach(x => x.Coordinaciones).SetValidator(new CoordinacionCecopiDtoValidator(localizer));
    }
}

public class CoordinacionCecopiDtoValidator : AbstractValidator<CreateOrUpdateCoordinacionCecopiDto>
{
    public CoordinacionCecopiDtoValidator(IStringLocalizer<ValidationMessages> localizer)
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
    }
}
