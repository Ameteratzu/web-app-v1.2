using DGPCE.Sigemad.Domain.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Specifications.Documentos;
internal class DocumentacionSpecificationByIdIncendio : BaseSpecification<Documentacion>
{
    public DocumentacionSpecificationByIdIncendio(int idIncendio)
    : base(d => d.IdIncendio == idIncendio && d.Borrado == false)
    {
        AddInclude(d => d.DetallesDocumentacion);
    }
}
