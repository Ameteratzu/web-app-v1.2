import { FireNational } from './fire-national.type';
import { FireStatus } from './fire-status.type';
import { GeoPosition } from './geo-position.type';

export type Fire = {
  id: number;
  denominacion: string;
  fechaInicio: string;
  fechaModificacion: string;
  estadoSuceso: FireStatus;
  notaGeneral: string;
  idSuceso: number;
  idEstado: number;
  idProvincia: number;
  idTerritorio: number;
  idEstadoSuceso: number;
  idMunicipio: number;
  idClaseSuceso: number;
  incendioNacional: FireNational;
  geoPosicion: GeoPosition;
  municipio?: {
    descripcion: string;
  };
};
