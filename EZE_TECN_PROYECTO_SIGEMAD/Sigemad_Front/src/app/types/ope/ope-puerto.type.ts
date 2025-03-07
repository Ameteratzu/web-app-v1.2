import { GeoPosition } from '@type/geo-position.type';

export type OpePuerto = {
  id: number;
  nombre: string;
  fase: string;
  idPais: number;
  idCcaa: number;
  idProvincia: number;
  idMunicipio: number;
  coordenadaUTM_X: string;
  coordenadaUTM_Y: string;
  fechaValidezDesde: string;
  fechaValidezHasta: string;
  capacidad: number;
};
