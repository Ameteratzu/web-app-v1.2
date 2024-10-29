using DGPCE.Sigemad.Application.Helpers;
using DGPCE.Sigemad.Domain.Enums;
using FluentValidation;

namespace DGPCE.Sigemad.Application.Features.Incendios.Commands.CreateIncendios;

public class CreateIncendioCommandValidator : AbstractValidator<CreateIncendioCommand>
{
    public CreateIncendioCommandValidator()
    {
        RuleFor(p => p.IdTerritorio)
            .IsInEnum().WithMessage("El territorio es una opción inválido")
            .NotEqual(TipoTerritorio.None).WithMessage("El territorio es obligatorio");

        RuleFor(p => p.Denominacion)
            .NotEmpty().WithMessage("Denominacion no puede estar en blanco")
            .NotNull().WithMessage("Denominacion es obligatorio")
            .MaximumLength(255).WithMessage("Denominacion no puede exceder los 255 caracteres");

        RuleFor(p => p.FechaInicio)
            .NotEmpty().WithMessage("Fecha de Inicio es obligatoria");

        RuleFor(p => p.IdClaseSuceso)
            .GreaterThan(0).WithMessage("Clase de Suceso es obligatorio");

        RuleFor(p => p.IdEstadoSuceso)
            .GreaterThan(0).WithMessage("Estado de Suceso es obligatorio");

        RuleFor(p => p.GeoPosicion)
            .NotNull().WithMessage("GeoPosicion es obligatorio")
            .Must(GeoJsonValidatorUtil.IsGeometryInWgs84).WithMessage("La geometría no es válida, sistema de referencia no es Wgs84");


        When(p => p.IdTerritorio == TipoTerritorio.Nacional, () =>
        {
            RuleFor(p => p.IdProvincia)
            .NotNull().WithMessage("Provincia es obligatoria")
            .GreaterThan(0).WithMessage("Provincia es inválido");

            RuleFor(p => p.IdMunicipio)
            .NotNull().WithMessage("Municipio es obligatoria")
                .GreaterThan(0).WithMessage("Municipio es inválido");
        });

        When(p => p.IdTerritorio == TipoTerritorio.Extranjero, () =>
        {
            RuleFor(p => p.IdPais)
            .NotNull().WithMessage("Pais es obligatoria")
            .GreaterThan(0).WithMessage("Pais es inválido");

            RuleFor(p => p.Ubicacion)
                .NotEmpty().WithMessage("Ubicación es obligatoria")
                .NotNull().WithMessage("Ubicación no puede ser nula");
        });

    }
}
