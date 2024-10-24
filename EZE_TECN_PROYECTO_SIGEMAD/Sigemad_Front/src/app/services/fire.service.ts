import { DatePipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { catchError, firstValueFrom, map, throwError } from 'rxjs';

import { ApiResponse } from '../types/api-response.type';
import { Fire } from '../types/fire.type';
import { FireDetail } from '../types/fire-detail.type';

@Injectable({ providedIn: 'root' })
export class FireService {
  public http = inject(HttpClient);
  public datepipe = inject(DatePipe);
  public endpoint = '/Incendios';

  get(query: any = '') {
    const endpoint = '/Incendios?Sort=desc&PageSize=15';

    if (query != '') {
      const territory = query.territory;
      const autonomousCommunity = query.autonomousCommunity;
      const province = query.province;
      const municipality = query.municipality;
      const fireStatus = query.fireStatus;
      const episode = query.episode;
      const severityLevel = query.severityLevel;
      const affectedArea = query.affectedArea;
      const start = query.start;
      const end = query.end;

      const endpoint = `/Incendios?IdTerritorio=${territory}&IdCcaa=${autonomousCommunity}&IdProvincia=${province}&IdMunicipio=${municipality}&IdEstado=${fireStatus}&IdEpisodio=${episode}&IdNivelGravedad=${severityLevel}&IdSuperficieAfectada=${affectedArea}&FechaInicio=${start}&FechaFin=${end}&Sort=desc&Page=1&PageSize=15`;
    }

    return firstValueFrom(
      this.http.get<ApiResponse<Fire[]>>(endpoint).pipe((response) => response)
    );
  }

  details(fire_id:number) {
    const endpoint = `/Incendios/${fire_id}/detalles`;

    return firstValueFrom(
      this.http.get<FireDetail[]>(endpoint).pipe((response) => response)
    );
  }

  post(data: any) {
    const body = {
      IdTerritorio: data.territory,
      IdProvincia: data.province,
      IdMunicipio: data.municipality,
      denominacion: data.name,
      fechaInicio: this.datepipe.transform(data.start, 'yyyy-MM-dd h:mm:ss'),
      IdSuceso: data.event,
      IdTipoSuceso: data.event,
      IdClaseSuceso: 1,
      IdEstado: 1,
      IdPeligroInicial: 1,
      comentarios: data.generalNote,
      GeoPosicion: data.geoposition,
      idPais: 60,
      IdEstadoSuceso: 1,
    };

    return firstValueFrom(
      this.http.post(this.endpoint, body).pipe(
        map((response) => {
          return response;
        }),
        catchError((error) => {
          return throwError(error.error);
        })
      )
    );
  }

  update(data: any) {
    const body = {
      id: data.id,
      IdTerritorio: data.territory,
      IdProvincia: data.province,
      IdMunicipio: data.municipality,
      denominacion: data.name,
      fechaInicio: this.datepipe.transform(data.start, 'yyyy-MM-dd h:mm:ss'),
      IdSuceso: data.event,
      IdTipoSuceso: data.event,
      IdClaseSuceso: 1,
      IdEstado: 1,
      IdPeligroInicial: 1,
      comentarios: data.note,
      GeoPosicion: {
        type: 'Polygon',
        coordinates: data.coordinates,
      },
    };

    return firstValueFrom(
      this.http.put(this.endpoint, body).pipe(
        map((response) => {
          return response;
        }),
        catchError((error) => {
          return throwError(error.error);
        })
      )
    );
  }

  delete(id: number) {
    const endpoint = `/Incendios/${id}`;

    return firstValueFrom(
      this.http.delete(endpoint).pipe((response) => response)
    );
  }
}
