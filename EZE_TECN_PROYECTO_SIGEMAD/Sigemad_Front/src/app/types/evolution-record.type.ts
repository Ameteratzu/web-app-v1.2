export interface EvolucionIncendio {
	idIncendio: number;
	idEvolucion: number;
	registro: Registro;
	datoPrincipal: DatoPrincipal;
	parametro: Parametro;
	registroProcedenciasDestinos: number[];
  }
  
  export interface Registro {
	fechaHoraEvolucion: string; 
	idEntradaSalida: number;
	idMedio: number;
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