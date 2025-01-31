import { OriginDestination } from './origin-destination.type';

export interface EvolucionIncendio {
  idSuceso: number;
  idEvolucion: any;
  registro: Registro;
  datoPrincipal: DatoPrincipal;
  parametro: Parametro;
}

export interface Registro {
  fechaHoraEvolucion: string;
  idEntradaSalida: number;
  idMedio: number;
  registroProcedenciasDestinos: OriginDestination[];
}

export interface DatoPrincipal {
  fechaHora: string;
  observaciones: string;
  prevision: string;
}

export interface Parametro {
  idEstadoIncendio: number;
  fechaFinal: string;
  superficieAfectadaHectarea: number;
  idPlanEmergencia: number;
  idFaseEmergencia: number;
  idSituacionEquivalente: number;
  idPlanSituacion: number;
}
