using DGPCE.Sigemad.Application.Helpers;
using DGPCE.Sigemad.Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace DGPCE.Sigemad.Application.Features.AreasAfectadas.Commands.UpdateAreasAfectadas;
public class UpdateAreaAfectadaCommandValidator : AbstractValidator<UpdateAreaAfectadaCommand>
{
    public UpdateAreaAfectadaCommandValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        
        RuleFor(p => p.Id)
            .GreaterThan(0).WithMessage(localizer["IdObligatorio"]);

        RuleFor(p => p.IdEvolucion)
        .GreaterThan(0).WithMessage(localizer["EvolucionObligatorio"]);

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
