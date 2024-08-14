using FluentValidation;

namespace DGPCE.Sigemad.Application.Features.Alertas.Commands.CreateAlertas
{
    public class CreateAlertaCommandValidator : AbstractValidator<CreateAlertaCommand>
    {
        public CreateAlertaCommandValidator()
        {
            RuleFor(p => p.Descripcion)
                .NotEmpty().WithMessage("{Descripcion} no puede estar en blanco")
                .NotNull()
                .MaximumLength(50).WithMessage("{Descripcion} no puede exceder los 50 caracteres");

            RuleFor(p => p.FechaAlerta)
                .NotEmpty().WithMessage("La {FechaAlerta} no puede estar en blanco")
                .NotNull();

            RuleFor(p => p.EstadoAlertaId)
                .NotEmpty().WithMessage("La {EstadoAlertaId} no puede estar en blanco")
                .NotNull();

        }
    }
}
