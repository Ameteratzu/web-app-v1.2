import { DatePipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { catchError, firstValueFrom, map, throwError } from 'rxjs';
import { ApiResponse } from '../../../types/api-response.type';
import { OpePuerto } from '../../../types/ope/administracion/ope-puerto.type';

@Injectable({ providedIn: 'root' })
export class OpePuertosService {
  public http = inject(HttpClient);
  public datepipe = inject(DatePipe);
  public endpoint = '/ope-puertos';

  generateUrlWitchParams({ url, params }: any) {
    return Object.keys(params).reduce((prev: any, key: any, index: any) => {
      if (!params[key]) {
        return `${prev}`;
      }
      return `${prev}&${key}=${params[key]}`;
    }, `${url}`);
  }

  get(query: any = '') {
    const URLBASE = '/ope-puertos?Sort=desc&PageSize=15';

    const endpoint = this.generateUrlWitchParams({
      url: URLBASE,
      params: query,
    });
    return firstValueFrom(this.http.get<ApiResponse<OpePuerto[]>>(endpoint).pipe((response) => response));
  }

  post(data: any) {
    const body = {
      nombre: data.nombre,
      idOpeFase: data.opeFase,
      idPais: data.country,
      idCcaa: data.autonomousCommunity,
      idProvincia: data.provincia,
      idMunicipio: data.municipality,
      coordenadaUTM_X: data.coordenadaUTM_X,
      coordenadaUTM_Y: data.coordenadaUTM_Y,
      fechaValidezDesde: this.datepipe.transform(data.fechaValidezDesde, 'yyyy-MM-dd HH:mm:ss'),
      fechaValidezHasta: this.datepipe.transform(data.fechaValidezHasta, 'yyyy-MM-dd HH:mm:ss'),
      capacidad: data.capacidad,
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
      nombre: data.nombre,
      idOpeFase: data.opeFase,
      idPais: data.country,
      idCcaa: data.autonomousCommunity,
      idProvincia: data.provincia,
      idMunicipio: data.municipality,
      coordenadaUTM_X: data.coordenadaUTM_X,
      coordenadaUTM_Y: data.coordenadaUTM_Y,
      fechaValidezDesde: this.datepipe.transform(data.fechaValidezDesde, 'yyyy-MM-dd HH:mm:ss'),
      fechaValidezHasta: this.datepipe.transform(data.fechaValidezHasta, 'yyyy-MM-dd HH:mm:ss'),
      capacidad: data.capacidad,
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
    const endpoint = `/ope-puertos/${id}`;

    return firstValueFrom(this.http.delete(endpoint).pipe((response) => response));
  }
}
