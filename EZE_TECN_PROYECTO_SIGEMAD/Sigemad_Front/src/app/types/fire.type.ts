import { FireStatus } from './fire-status.type';

export type Fire = {
  id: number;
  denominacion: string;
  fechaInicio: string;
  estadoSuceso: FireStatus;
  comentarios: string;
  idSuceso: number;
  idEstado: number;
  idProvincia: number;
  idTerritorio: number;
  idMunicipio: number;
  idClaseSuceso: number;
};
