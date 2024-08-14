using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Features.Alertas.Commands.UpdateAlertas
{
    public class UpdateAlertaCommandValidator : AbstractValidator<UpdateAlertaCommand>
    {
        public UpdateAlertaCommandValidator()
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
