export interface AffectedArea {
	idEvolucion: number;
	idIncendio: number;
	areasAfectadas: Lista[];
  }
  
  export interface Lista {
	id: number;
	fechaHora: string; 
	idProvincia: number;
	idMunicipio: number;
	idEntidadMenor: number;
	observaciones: string;
  }