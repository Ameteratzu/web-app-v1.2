import { FireStatus } from './fire-status.type';

export type Fire = {
  id: number;
  denominacion: string;
  fechaInicio: string;
  estadoIncendio: FireStatus;
  comentarios: string;
  idSuceso: number;
  idProvincia: number;
  idTerritorio: number;
  idMunicipio: number;
  idClaseSuceso: number;
};
