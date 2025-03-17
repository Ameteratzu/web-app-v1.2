import { AutonomousCommunity } from '@type/autonomous-community.type';
import { Municipality } from '@type/municipality.type';
import { Province } from '@type/province.type';

export type OpeFrontera = {
  id: number;
  nombre: string;
  idCcaa: number;
  CCAA: AutonomousCommunity;
  idProvincia: number;
  provincia: Province;
  idMunicipio: number;
  municipio: Municipality;
  carreteraPK: string;
  coordenadaUTM_X: string;
  coordenadaUTM_Y: string;
  transitoMedioVehiculos: number;
  transitoAltoVehiculos: number;
};
