using FluentValidation;

namespace DGPCE.Sigemad.Application.Features.Incendios.Commands.UpdateIncendios;

public class UpdateIncendioCommandValidator : AbstractValidator<UpdateIncendioCommand>
{
    public UpdateIncendioCommandValidator()
    {
        RuleFor(p => p.Id)
            .NotEmpty().WithMessage("Id no puede estar en blanco")
            .NotNull().WithMessage("IdT es obligatorio");

        RuleFor(p => p.IdTerritorio)
            .NotEmpty().WithMessage("IdTerritorio no puede estar en blanco")
            .NotNull().WithMessage("IdTerritorio es obligatorio");

        RuleFor(p => p.IdProvincia)
            .NotEmpty().WithMessage("IdProvincia no puede estar en blanco")
            .NotNull().WithMessage("IdProvincia es obligatorio");

        RuleFor(p => p.IdMunicipio)
            .NotEmpty().WithMessage("IdMunicipio no puede estar en blanco")
            .NotNull().WithMessage("IdMunicipio es obligatorio");

        RuleFor(p => p.Denominacion)
            .NotEmpty().WithMessage("Denominacion no puede estar en blanco")
            .NotNull().WithMessage("Denominacion es obligatorio")
            .MaximumLength(255).WithMessage("Denominacion no puede exceder los 255 caracteres");

        RuleFor(p => p.FechaInicio)
            .NotEmpty().WithMessage("FechaInicio no puede estar en blanco")
            .NotNull().WithMessage("FechaInicio es obligatorio");

        RuleFor(p => p.IdTipoSuceso)
            .NotEmpty().WithMessage("IdTipoSuceso no puede estar en blanco")
            .NotNull().WithMessage("IdTipoSuceso es obligatorio");

        RuleFor(p => p.IdClaseSuceso)
            .NotEmpty().WithMessage("IdClaseSuceso no puede estar en blanco")
            .NotNull().WithMessage("IdClaseSuceso es obligatorio");

        RuleFor(p => p.IdEstado)
            .NotEmpty().WithMessage("IdEstado no puede estar en blanco")
            .NotNull().WithMessage("IdEstado es obligatorio");

        RuleFor(p => p.IdPeligroInicial)
            .NotEmpty().WithMessage("IdPeligroInicial no puede estar en blanco")
            .NotNull().WithMessage("IdPeligroInicial es obligatorio");

        RuleFor(p => p.Comentarios)
            .NotEmpty().WithMessage("Comentarios no puede estar en blanco");

        RuleFor(p => p.GeoPosicion)
            .NotEmpty().WithMessage("GeoPosicion no puede estar en blanco")
            .NotNull().WithMessage("GeoPosicion es obligatorio");
    }
}
