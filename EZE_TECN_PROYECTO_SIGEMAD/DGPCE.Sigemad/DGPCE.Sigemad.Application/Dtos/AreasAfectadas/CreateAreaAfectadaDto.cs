﻿using NetTopologySuite.Geometries;

namespace DGPCE.Sigemad.Application.Dtos.AreasAfectadas;
public class CreateAreaAfectadaDto
{
    public DateTime FechaHora { get; set; }
    public int IdProvincia { get; set; }
    public int IdMunicipio { get; set; }
    public int? IdEntidadMenor { get; set; }
    public string? Observaciones { get; set; }
    public Geometry? GeoPosicion { get; set; }
}
