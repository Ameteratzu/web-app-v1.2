import { OpePeriodoTipo } from './ope-periodo-tipo.type';

export type OpePeriodo = {
  id: number;
  nombre: string;
  idOpePeriodoTipo: number;
  opePeriodoTipo: OpePeriodoTipo;
  fechaInicioFaseSalida: string;
  fechaFinFaseSalida: string;
  fechaInicioFaseRetorno: string;
  fechaFinFaseRetorno: string;
};
