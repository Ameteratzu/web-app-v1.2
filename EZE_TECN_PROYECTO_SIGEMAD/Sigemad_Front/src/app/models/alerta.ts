export class Alerta {
    id?: string;
    descripcion?: string;
    fechaAlerta?: Date;
    idEstadoAlerta?: string;
    estadoAlerta?: {
        descripcion?: string;
    };

    constructor() {
    }
}