using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Features.EstadosAlertas.Commands.UpdateAlertas
{
    public class UpdateEstadoAlertaCommandValidator : AbstractValidator<UpdateEstadoAlertaCommand>
    {
        public UpdateEstadoAlertaCommandValidator()
        {
            RuleFor(p => p.Descripcion)
                .NotEmpty().WithMessage("{Descripcion} no puede estar en blanco")
                .NotNull()
                .MaximumLength(50).WithMessage("{Descripcion} no puede exceder los 50 caracteres");

        }
    }
}
