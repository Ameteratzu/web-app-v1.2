export interface EvolucionIncendio {
  idIncendio: number;
  idEvolucion: any;
  registro: Registro;
  datoPrincipal: DatoPrincipal;
  parametro: Parametro;
}

export interface Registro {
  fechaHoraEvolucion: string;
  idEntradaSalida: number;
  idMedio: number;
  registroProcedenciasDestinos: any[];
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
  planEmergenciaActivado: string;
  idFase: number;
  idSituacionOperativa: number;
  idSituacionEquivalente: number;
}
