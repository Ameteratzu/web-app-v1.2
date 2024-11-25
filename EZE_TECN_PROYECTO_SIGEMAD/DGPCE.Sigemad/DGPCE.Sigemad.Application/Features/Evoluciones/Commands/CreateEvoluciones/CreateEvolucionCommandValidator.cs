using DGPCE.Sigemad.Application.Features.Evoluciones.Commands.ManageEvoluciones;
using DGPCE.Sigemad.Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace DGPCE.Sigemad.Application.Features.Evoluciones.Commands.CreateEvoluciones;

public class CreateEvolucionCommandValidator : AbstractValidator<ManageEvolucionCommand>
{

    public CreateEvolucionCommandValidator(IStringLocalizer<ValidationMessages> localizer)
    {


    }
}