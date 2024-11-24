﻿using DGPCE.Sigemad.Application.Dtos.Common;
using DGPCE.Sigemad.Application.Dtos.CoordinacionCecopis;
using DGPCE.Sigemad.Application.Dtos.Direcciones;
using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Dtos.DireccionCoordinaciones;
public class DireccionCoordinacionEmergenciaDto : BaseDto<int>
{
    public int IdIncendio { get; set; }
    public Incendio Incendio { get; set; }

    public List<DireccionDto> Direcciones { get; set; } = new();
    public List<CoordinacionCecopiDto> CoordinacionesCecopi { get; set; } = new();
    public List<CoordinacionPMA> CoordinacionesPMA { get; set; } = new();
}
