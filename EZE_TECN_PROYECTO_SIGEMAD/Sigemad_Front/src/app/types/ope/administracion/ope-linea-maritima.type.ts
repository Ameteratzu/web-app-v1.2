import { OpeFase } from './ope-fase.type';
import { OpePuerto } from './ope-puerto.type';

export type OpeLineaMaritima = {
  id: number;
  nombre: string;
  idOpePuertoOrigen: number;
  idOpePuertoDestino: number;
  opePuertoOrigen: OpePuerto;
  opePuertoDestino: OpePuerto;
  idOpeFase: number;
  opeFase: OpeFase;
  fechaValidezDesde: string;
  fechaValidezHasta: string;
  numeroRotaciones: number;
  numeroPasajeros: number;
  numeroTurismos: number;
  numeroAutocares: number;
  numeroCamiones: number;
  numeroTotalVehiculos: number;
};
