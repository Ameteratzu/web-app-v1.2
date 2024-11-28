using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Features.Documentaciones.Commands.DeleteDocumentaciones;
public class DeleteDocumentacionesCommand : IRequest
{
    public int Id { get; set; }

}