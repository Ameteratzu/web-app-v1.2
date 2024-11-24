using DGPCE.Sigemad.Application.Dtos.Direcciones;
using DGPCE.Sigemad.Application.Features.Direcciones.Commands.CreateDirecciones;
using DGPCE.Sigemad.Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace DGPCE.Sigemad.Application.Features.Direcciones.Commands.CreateOrUpdateDirecciones;
public class CreateOrUpdateDireccionCommandValidator : AbstractValidator<CreateOrUpdateDireccionCommand>
{
    public CreateOrUpdateDireccionCommandValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(x => x.IdIncendio)
            .GreaterThan(0).WithMessage(localizer["IdIncendioObligatorio"]);

        RuleForEach(x => x.Direcciones).SetValidator(new DireccionDtoValidator(localizer));
    }
}


public class DireccionDtoValidator : AbstractValidator<CreateOrUpdateDireccionDto>
{
    public DireccionDtoValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(d => d.IdTipoDireccionEmergencia)
            .GreaterThan(0).WithMessage(localizer["IdTipoDireccionEmergenciaInvalido"]);

        RuleFor(d => d.AutoridadQueDirige)
            .NotEmpty().WithMessage(localizer["AutoridadQueDirigeObligatorio"]);

        RuleFor(d => d.FechaInicio)
            .NotEmpty().WithMessage(localizer["FechaInicioObligatorio"])
            .LessThanOrEqualTo(d => d.FechaFin).WithMessage(localizer["FechaInicioDebeSerMenorQueFechaFin"]);

        RuleFor(d => d.FechaFin)
            .NotEmpty().WithMessage(localizer["FechaFinObligatorio"]);
    }
}