using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Features.Documentaciones.ManageDocumentaciones;
public class ManageDocumentacionesCommandList : IRequest<Unit>
{
    public int IdIncendio { get; set; }
    public List<ManageDocumentacionesCommand> Documentaciones { get; set; }
}
