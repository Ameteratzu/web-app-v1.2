﻿using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;

namespace DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpeAreasDescanso;
public class OpeAreaDescansoActiveByIdSpecification : BaseSpecification<OpeAreaDescanso>
{
    public OpeAreaDescansoActiveByIdSpecification(int id)
        : base(i => i.Id == id && i.Borrado == false)
    {

    }
}
