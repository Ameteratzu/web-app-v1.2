import { DatePipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { catchError, firstValueFrom, map, throwError } from 'rxjs';

import { ApiResponse } from '../types/api-response.type';
import { FireDetail } from '../types/fire-detail.type';
import { Fire } from '../types/fire.type';

@Injectable({ providedIn: 'root' })
export class FireService {
  public http = inject(HttpClient);
  public datepipe = inject(DatePipe);
  public endpoint = '/Incendios';

  generateUrlWitchParams({ url, params }: any) {
    return Object.keys(params).reduce((prev: any, key: any, index: any) => {
      if (!params[key]) {
        return `${prev}`;
      }
      return `${prev}&${key}=${params[key]}`;
    }, `${url}`);
  }

  get(query: any = '') {
    const URLBASE = '/Incendios?Sort=desc&PageSize=15';

    const endpoint = this.generateUrlWitchParams({
      url: URLBASE,
      params: query,
    });

    return firstValueFrom(this.http.get<ApiResponse<Fire[]>>(endpoint).pipe((response) => response));
  }

  getById(id: number) {
    let endpoint = `/Incendios/${id}`;

    return firstValueFrom(this.http.get<Fire>(endpoint).pipe((response) => response));
  }

  details(fire_id: number) {
    const endpoint = `/Incendios/${fire_id}/registros`;

    return firstValueFrom(this.http.get<FireDetail[]>(endpoint).pipe((response) => response));
  }

  post(data: any) {
    const body = {
      IdTerritorio: data.territory ? data.territory : 1,
      idClaseSuceso: data.classEvent,
      idEstadoSuceso: data.eventStatus,
      fechaInicio: this.datepipe.transform(data.startDate, 'yyyy-MM-dd h:mm:ss'),
      denominacion: data.denomination,
      notaGeneral: data.generalNote,
      IdProvincia: data.province,
      IdMunicipio: data.municipality,
      idPais: data.country,
      ubicacion: data.ubication,
      esLimitrofe: data.isLimitrofe,
      GeoPosicion: data.geoposition,
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
      idClaseSuceso: data.classEvent,
      idEstadoSuceso: data.eventStatus,
      fechaInicio: this.datepipe.transform(data.startDate, 'yyyy-MM-dd h:mm:ss'),
      denominacion: data.denomination,
      notaGeneral: data.generalNote,
      IdProvincia: data.province,
      IdMunicipio: data.municipality,
      idPais: data.country,
      ubicacion: data.ubication,
      esLimitrofe: data.isLimitrofe,
      GeoPosicion: data.geoposition,
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

    return firstValueFrom(this.http.delete(endpoint).pipe((response) => response));
  }
}
