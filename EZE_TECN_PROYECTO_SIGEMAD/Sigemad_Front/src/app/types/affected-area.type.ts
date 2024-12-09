export interface AffectedArea {
  id: number;
  fechaHora: string;
  idProvincia: { id: number; descripcion: string };
  idMunicipio: { id: number; descripcion: string };
  idEntidadMenor: { id: number; descripcion: string };
  observaciones: string;
  geoPosicion?: any;
}
