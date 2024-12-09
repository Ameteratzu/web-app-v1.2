using DGPCE.Sigemad.Application.Dtos.Impactos;
using DGPCE.Sigemad.Application.Features.ImpactosEvoluciones.Commands.CreateListaImpactoEvolucion;
using FluentValidation;

namespace DGPCE.Sigemad.Application.Features.ImpactosEvoluciones.Commands.ManageImpactos;

public class ManageImpactosCommandValidator : AbstractValidator<ManageImpactosCommand>
{
    public ManageImpactosCommandValidator()
    {
        RuleFor(x => x.IdEvolucion).GreaterThan(0).WithMessage("El ID de Evolución es obligatorio.");
        
        RuleForEach(x => x.Impactos)
            .SetValidator(new ImpactoDtoValidator())
            .When(d => d.Impactos.Count > 0);
    }

    public class ImpactoDtoValidator : AbstractValidator<ManageImpactoDto>
    {
        public ImpactoDtoValidator()
        {
            RuleFor(x => x.IdImpactoClasificado).GreaterThan(0).WithMessage("El ID de Impacto Clasificado es obligatorio.");
            RuleFor(x => x.AlteracionInterrupcion)
                .Must(x => x == 'A' || x == 'I' || x == null)
                .WithMessage("El valor de Alteración/Interrupción debe ser 'A', 'I' o nulo.");
        }
    }
}
