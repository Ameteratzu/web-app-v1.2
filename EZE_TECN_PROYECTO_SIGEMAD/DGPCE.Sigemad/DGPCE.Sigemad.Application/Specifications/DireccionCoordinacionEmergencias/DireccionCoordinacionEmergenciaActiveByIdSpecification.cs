﻿using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.DireccionCoordinacionEmergencias;

public class DireccionCoordinacionEmergenciaActiveByIdSpecification : BaseSpecification<DireccionCoordinacionEmergencia>
{
    public DireccionCoordinacionEmergenciaActiveByIdSpecification(DireccionCoordinacionEmergenciaSpecificationParams request)
       : base(DireccionCoordinacionEmergencia =>
         (!request.Id.HasValue || DireccionCoordinacionEmergencia.Id == request.Id) &&
        (!request.IdSuceso.HasValue || DireccionCoordinacionEmergencia.IdSuceso == request.IdSuceso) &&
        (DireccionCoordinacionEmergencia.Borrado == false))
    {
        AddInclude(d => d.Direcciones.Where(dir => !dir.Borrado));
        AddInclude("Direcciones.TipoDireccionEmergencia");

        AddInclude(d => d.CoordinacionesCecopi.Where(dir => !dir.Borrado));
        AddInclude("CoordinacionesCecopi.Provincia");
        AddInclude("CoordinacionesCecopi.Municipio");

        AddInclude(d => d.CoordinacionesPMA.Where(dir => !dir.Borrado));
        AddInclude("CoordinacionesPMA.Provincia");
        AddInclude("CoordinacionesPMA.Municipio");
    }
}
