using DGPCE.Sigemad.Application.Features.DatosPrincipales.Commands;
using DGPCE.Sigemad.Application.Features.Parametros.Commands;
using DGPCE.Sigemad.Application.Features.Registros.Command.CreateRegistros;
using DGPCE.Sigemad.Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace DGPCE.Sigemad.Application.Features.Evoluciones.Commands.ManageEvoluciones;

public class ManageEvolucionCommandValidator : AbstractValidator<ManageEvolucionCommand>
{
    public ManageEvolucionCommandValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(x => x.IdSuceso)
            .GreaterThan(0).WithMessage(localizer["IdSucesoObligatorio"]);

        RuleFor(x => x.Registro)
            .NotNull().WithMessage(localizer["RegistroObligatorio"])
            .SetValidator(new CreateRegistroCommandValidator(localizer));

        RuleFor(x => x.DatoPrincipal)
            .NotNull().WithMessage(localizer["DatoPrincipalObligatorio"])
            .SetValidator(new CreateDatoPrincipalCommandValidator(localizer));

        RuleFor(x => x.Parametro)
            .NotNull().WithMessage(localizer["ParametroObligatorio"])
            .SetValidator(new CreateParametroCommandValidator(localizer));
    }
}

public class CreateRegistroCommandValidator : AbstractValidator<CreateRegistroCommand>
{
    public CreateRegistroCommandValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(x => x.FechaHoraEvolucion)
            .NotNull().WithMessage(localizer["FechaHoraObligatorio"]);

        RuleFor(x => x.IdEntradaSalida)
            .GreaterThan(0).WithMessage(localizer["IdEntradaSalidaObligatorio"]);

        RuleFor(x => x.IdMedio)
            .GreaterThan(0).WithMessage(localizer["IdMedioObligatorio"]);

        RuleFor(x => x.RegistroProcedenciasDestinos)
            .NotEmpty().WithMessage(localizer["RegistroProcedenciasDestinosObligatorio"]);
    }
}

public class CreateDatoPrincipalCommandValidator : AbstractValidator<CreateDatoPrincipalCommand>
{
    public CreateDatoPrincipalCommandValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(x => x.FechaHora)
            .NotNull().WithMessage(localizer["FechaHoraObligatorio"]);
    }
}

public class CreateParametroCommandValidator : AbstractValidator<CreateParametroCommand>
{
    public CreateParametroCommandValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(x => x.IdEstadoIncendio)
            .GreaterThan(0).WithMessage(localizer["IdEstadoIncendioObligatorio"]);
    }
}
