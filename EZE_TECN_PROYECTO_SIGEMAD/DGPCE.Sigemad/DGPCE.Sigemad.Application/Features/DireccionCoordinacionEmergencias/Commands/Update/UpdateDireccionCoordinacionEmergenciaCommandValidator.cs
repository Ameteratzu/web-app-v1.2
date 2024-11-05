using DGPCE.Sigemad.Application.Resources;
using DGPCE.Sigemad.Domain.Enums;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Commands.Update;
public class UpdateDireccionCoordinacionEmergenciaCommandValidator : AbstractValidator<UpdateDireccionCoordinacionEmergenciaCommand>
{
    public UpdateDireccionCoordinacionEmergenciaCommandValidator(IStringLocalizer<ValidationMessages> localizer)
    {

        RuleFor(p => p.Id)
                .NotEmpty().WithMessage(localizer["IdNoVacio"])
                .NotNull().WithMessage(localizer["IdObligatorio"]);

        RuleFor(p => p.IdIncendio)
                 .GreaterThan(0).WithMessage(localizer["IncendioObligatorio"]);

        RuleFor(p => p.IdTipoDireccionEmergencia)
                 .IsInEnum().WithMessage(localizer["TipoDireccionEmergenciaInvalido"])
                 .NotEqual(TipoDireccionEmergenciaEnum.None).WithMessage(localizer["TipoDireccionEmergenciaObligatorio"]);

        RuleFor(p => p.IdProvinciaPMA)
               .NotNull().WithMessage(localizer["ProvinciaPMAObligatorio"])
                .GreaterThan(0).WithMessage(localizer["ProvinciaPMAInvalido"]);

        RuleFor(p => p.IdProvinciaCECOPI)
                .NotNull().WithMessage(localizer["ProvinciaCECOPIObligatorio"])
                .GreaterThan(0).WithMessage(localizer["ProvinciaCECOPIInvalido"]);

        RuleFor(p => p.IdMunicipioPMA)
                .NotNull().WithMessage(localizer["MunicipioPMAObligatorio"])
                .GreaterThan(0).WithMessage(localizer["MunicipioPMAiInvalido"]);

        RuleFor(p => p.IdMunicipioCECOPI)
              .NotNull().WithMessage(localizer["MunicipioCECOPIObligatorio"])
              .GreaterThan(0).WithMessage(localizer["MunicipioCECOPIInvalido"]);

        RuleFor(p => p.AutoridadQueDirige)
              .NotEmpty().WithMessage(localizer["AutoridadQueDirigeEnBlanco"])
              .NotNull().WithMessage(localizer["AutoridadQueDirigeObligatorio"]);

        RuleFor(p => p.FechaInicioPMA)
           .NotEmpty().WithMessage(localizer["FechaInicioPMAObligatorio"]);
    }
}
