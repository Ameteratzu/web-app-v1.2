using DGPCE.Sigemad.Application.Dtos.AreasAfectadas;
using DGPCE.Sigemad.Application.Helpers;
using DGPCE.Sigemad.Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace DGPCE.Sigemad.Application.Features.AreasAfectadas.Commands.CreateAreasAfectadas;
public class CreateAreaAfectadaCommandValidator : AbstractValidator<CreateAreaAfectadaCommand>
{
    public CreateAreaAfectadaCommandValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(p => p.IdIncendio)
            .GreaterThan(0).WithMessage(localizer["IncendioIdObligatorio"]);

        RuleFor(command => command.AreasAfectadas)
            .NotNull().WithMessage(localizer["AreaAfectadaNoNulo"])
            .NotEmpty().WithMessage(localizer["AreaAfectadaVacio"]);

        // Validación para cada elemento de la lista AreasAfectadas
        RuleForEach(command => command.AreasAfectadas).SetValidator(new AreaAfectadaDtoValidator(localizer));
    }
}

public class AreaAfectadaDtoValidator : AbstractValidator<CreateAreaAfectadaDto>
{
    public AreaAfectadaDtoValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(p => p.FechaHora)
            .NotEmpty().WithMessage(localizer["FechaHoraObligatorio"]);

        RuleFor(p => p.IdProvincia)
           .NotNull().WithMessage(localizer["ProvinciaObligatorio"])
           .GreaterThan(0).WithMessage(localizer["ProvinciaInvalido"]);

        RuleFor(p => p.IdMunicipio)
            .NotNull().WithMessage(localizer["MunicipioObligatorio"])
            .GreaterThan(0).WithMessage(localizer["MunicipioInvalido"]);

        RuleFor(p => p.IdEntidadMenor)
                .GreaterThan(0).WithMessage(localizer["EntidadMenorObligatorio"]);

        RuleFor(p => p.GeoPosicion)
                .NotNull().WithMessage(localizer["GeoPosicionObligatorio"])
                .Must(GeoJsonValidatorUtil.IsGeometryInWgs84).WithMessage(localizer["GeoPosicionInvalida"]);
    }
}
