using FluentValidation;

namespace DGPCE.Sigemad.Application.Features.EstadosAlertas.Commands.CreateAlertas
{
    public class CreateEstadoAlertaCommandValidator : AbstractValidator<CreateEstadoAlertaCommand>
    {
        public CreateEstadoAlertaCommandValidator()
        {
            RuleFor(p => p.Descripcion)
                .NotEmpty().WithMessage("{Descripcion} no puede estar en blanco")
                .NotNull()
                .MaximumLength(50).WithMessage("{Descripcion} no puede exceder los 50 caracteres");


        }
    }
}
