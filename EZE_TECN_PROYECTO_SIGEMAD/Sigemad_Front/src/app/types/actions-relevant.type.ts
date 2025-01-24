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