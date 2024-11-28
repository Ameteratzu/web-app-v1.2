using DGPCE.Sigemad.Application.Dtos.OtraInformaciones;
using DGPCE.Sigemad.Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace DGPCE.Sigemad.Application.Features.OtrasInformaciones.Commands.ManageOtraInformaciones;


public class ManageOtraInformacionCommandValidator : AbstractValidator<ManageOtraInformacionCommand>
{
    public ManageOtraInformacionCommandValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(x => x.IdIncendio)
            .GreaterThan(0).WithMessage(localizer["IdIncendioObligatorio"]);

        RuleFor(command => command.Lista)
        .NotNull().WithMessage(localizer["ListaOtraInformacion"])
            .NotEmpty().WithMessage(localizer["ListaOtraInformacion"]);

        RuleForEach(x => x.Lista).SetValidator(new DetalleOtraInformacionDtoValidator(localizer));
    }
}

public class DetalleOtraInformacionDtoValidator : AbstractValidator<CreateDetalleOtraInformacionDto>
{
    public DetalleOtraInformacionDtoValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(p => p.FechaHora)
            .NotEmpty().WithMessage(localizer["FechaHoraObligatorio"]);

        RuleFor(p => p.IdMedio)
           .NotNull().WithMessage(localizer["MedioIdNoVacio"])
           .GreaterThan(0).WithMessage(localizer["MedioIdObligatorio"]);

        RuleFor(d => d.Asunto)
            .NotEmpty().WithMessage(localizer["AsuntoNoNulo"]);

        RuleFor(d => d.Observaciones)
            .NotEmpty().WithMessage(localizer["ObservacionObligatorio"]);
    }
}