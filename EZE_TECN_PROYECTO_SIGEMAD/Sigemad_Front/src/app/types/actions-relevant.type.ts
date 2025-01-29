export interface EmergenciaNacional {
  idSuceso: number;
  idActuacionRelevante: any;
  emergenciaNacional: Declaracion;
}

export interface Declaracion {
  autoridad: string;
  descripcionSolicitud: string;
  fechaHoraSolicitud: string;
  fechaHoraDeclaracion: string;
  descripcionDeclaracion: string;
  fechaHoraDireccion: string;
  observaciones: string;
}

export interface Zagep {
  id: number;
  fechaSolicitud: string;
  denominacion: string;
  observaciones: string;
}

export interface Cecod {
  id: number;
  fechaInicio: string;
  fechaFin: string;
  lugar: string;
  convocados: string;
  participantes: string;
  observaciones: string;
}

