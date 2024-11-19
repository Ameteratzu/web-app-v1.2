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

    return firstValueFrom(
      this.http.get<ApiResponse<Fire[]>>(endpoint).pipe((response) => response)
    );
  }

  getById(id: number) {
    let endpoint = `/Incendios/${id}`;

    return firstValueFrom(
      this.http.get<Fire>(endpoint).pipe((response) => response)
    );
  }

  details(fire_id: number) {
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
      denominacion: data.denomination,
      fechaInicio: this.datepipe.transform(
        data.startDate,
        'yyyy-MM-dd h:mm:ss'
      ),
      IdSuceso: data.event,
      IdTipoSuceso: data.event,
      IdClaseSuceso: data.event,
      notaGeneral: data.generalNote,
      GeoPosicion: data.geoposition,
      idPais: data.country,
      ubicacion: data.ubication,
      IdEstadoSuceso: 1,
      IdEstado: 1,
      IdPeligroInicial: 1,
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
      denominacion: data.denomination,
      fechaInicio: this.datepipe.transform(
        data.startDate,
        'yyyy-MM-dd h:mm:ss'
      ),
      IdSuceso: data.event,
      IdTipoSuceso: data.event,
      IdEstadoSuceso: data.event,
      IdClaseSuceso: data.event,
      IdEstado: 1,
      IdPeligroInicial: 1,
      notaGeneral: data.generalNote,
      GeoPosicion: {
        type: 'Point',
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
