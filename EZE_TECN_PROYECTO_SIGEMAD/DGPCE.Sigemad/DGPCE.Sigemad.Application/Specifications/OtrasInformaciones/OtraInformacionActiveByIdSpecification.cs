﻿using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.OtrasInformaciones;
public class OtraInformacionActiveByIdSpecification : BaseSpecification<OtraInformacion>
{
    public OtraInformacionActiveByIdSpecification(int id)
        : base(i => i.Id == id && i.Borrado == false)
    {
        AddInclude(i => i.DetallesOtraInformacion);
    }
}
