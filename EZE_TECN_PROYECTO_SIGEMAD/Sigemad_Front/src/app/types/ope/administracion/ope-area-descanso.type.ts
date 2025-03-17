import { AutonomousCommunity } from '@type/autonomous-community.type';
import { Municipality } from '@type/municipality.type';
import { Province } from '@type/province.type';
import { OpeAreaDescansoTipo } from './ope-area-descanso-tipo';
import { OpeEstadoOcupacion } from './ope-estado-ocupacion.type';

export type OpeAreaDescanso = {
  id: number;
  nombre: string;
  idOpeAreaDescansoTipo: number;
  opeAreaDescansoTipo: OpeAreaDescansoTipo;
  idCcaa: number;
  CCAA: AutonomousCommunity;
  idProvincia: number;
  provincia: Province;
  idMunicipio: number;
  municipio: Municipality;
  carreteraPK: string;
  coordenadaUTM_X: string;
  coordenadaUTM_Y: string;
  capacidad: number;
  idOpeEstadoOcupacion: number;
  opeEstadoOcupacion: OpeEstadoOcupacion;
};
