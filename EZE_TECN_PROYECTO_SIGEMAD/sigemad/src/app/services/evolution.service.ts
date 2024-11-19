import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { catchError, firstValueFrom, map, throwError } from 'rxjs';

import { Evolution } from '../types/evolution.type';

@Injectable({ providedIn: 'root' })
export class EvolutionService {
  private http = inject(HttpClient);

  get(fire_id: any) {
    const endpoint = `/Evoluciones/${fire_id}`;

    return firstValueFrom(
      this.http.get<Evolution[]>(endpoint).pipe((response) => response)
    );
  }

  post(data: any) {
    const endpoint = `/Evoluciones`;

    const body = {
      idTecnico: '550E683E-0458-43E8-A6E6-20887DC2BDDD',
      idIncendio: data.fire_id,
      fechaHoraEvolucion: data.startDate,
      idEntradaSalida: data.inputOutput,
      idMedio: data.media,
      idTipoRegistro: 1,
      idEntidadMenor: data.areasAffected?.[0]?.minorEntity,
      resumen: true, //?
      observaciones: data.observations_1,
      idEstadoIncendio: data.status,
      superficieAfectadaHectarea: data.affectedSurface,
      fechaFinal: data.end_date,
      idProvinciaAfectada: data.areasAffected?.[0]?.province_1,
      idMunicipioAfectado: data.areasAffected?.[0]?.municipality_1,
      geoPosicionAreaAfectada: data.areasAffected?.[0]?.geoPosicion || {},
      evolucionProcedenciaDestinos: data.originDestination,
    };

    return firstValueFrom(
      this.http.post(endpoint, body).pipe(
        map((response) => {
          return response;
        }),
        catchError((error) => {
          return throwError(error.error);
        })
      )
    );
  }

  update(body: any) {
    const endpoint = `/Evoluciones`;

    return firstValueFrom(
      this.http.put(endpoint, body).pipe(
        map((response) => {
          return response;
        }),
        catchError((error) => {
          return throwError(error.error);
        })
      )
    );
  }
}
