﻿using MediatR;
using NetTopologySuite.Geometries;

namespace DGPCE.Sigemad.Application.Features.ImpactosEvoluciones.Commands.UpdateImpactoEvoluciones;
public class UpdateImpactoEvolucionCommand: IRequest
{
    public int Id { get; set; }
    public int IdEvolucion { get; set; }
    public int IdImpactoClasificado { get; set; }
    public bool? Nuclear { get; set; }
    public int? ValorAD { get; set; }
    public int? Numero { get; set; }
    public string? Observaciones { get; set; }
    public DateTime? Fecha { get; set; }
    public DateTime? FechaHora { get; set; }
    public DateTime? FechaHoraInicio { get; set; }
    public DateTime? FechaHoraFin { get; set; }
    public char? AlteracionInterrupcion { get; set; }
    public string? Causa { get; set; }
    public int? NumeroGraves { get; set; }
    public string? TipoDanio { get; set; }
    public Geometry? ZonaPlanificacion { get; set; }
    public int? NumeroUsuarios { get; set; }
    public int? NumeroIntervinientes { get; set; }
    public int? NumeroServicios { get; set; }
    public int? NumeroLocalidades { get; set; }
}
