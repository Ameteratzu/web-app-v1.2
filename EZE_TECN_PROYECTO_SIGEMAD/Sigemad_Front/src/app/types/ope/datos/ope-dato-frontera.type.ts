import { OpeFrontera } from '../administracion/ope-frontera.type';

export type OpeDatoFrontera = {
  id: number;
  idOpeFrontera: number;
  opeFrontera: OpeFrontera;
  fechaHoraInicioIntervalo: string;
  fechaHoraFinIntervalo: string;
  numeroVehiculos: number;
  afluencia: string;
};
