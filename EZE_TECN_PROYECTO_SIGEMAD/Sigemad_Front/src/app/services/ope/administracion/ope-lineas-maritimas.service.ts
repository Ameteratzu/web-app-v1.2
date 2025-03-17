import { DatePipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { catchError, firstValueFrom, map, throwError } from 'rxjs';
import { ApiResponse } from '../../../types/api-response.type';
import { OpeLineaMaritima } from '../../../types/ope/administracion/ope-linea-maritima.type';

@Injectable({ providedIn: 'root' })
export class OpeLineasMaritimasService {
  public http = inject(HttpClient);
  public datepipe = inject(DatePipe);
  public endpoint = '/ope-lineas-maritimas';

  generateUrlWitchParams({ url, params }: any) {
    return Object.keys(params).reduce((prev: any, key: any, index: any) => {
      if (!params[key]) {
        return `${prev}`;
      }
      return `${prev}&${key}=${params[key]}`;
    }, `${url}`);
  }

  get(query: any = '') {
    const URLBASE = '/ope-lineas-maritimas?Sort=desc&PageSize=15';

    const endpoint = this.generateUrlWitchParams({
      url: URLBASE,
      params: query,
    });
    return firstValueFrom(this.http.get<ApiResponse<OpeLineaMaritima[]>>(endpoint).pipe((response) => response));
  }

  post(data: any) {
    const body = {
      nombre: data.nombre,
      idOpePuertoOrigen: data.opePuertoOrigen,
      idOpePuertoDestino: data.opePuertoDestino,
      idOpeFase: data.opeFase,
      fechaValidezDesde: this.datepipe.transform(data.fechaValidezDesde, 'yyyy-MM-dd HH:mm:ss'),
      fechaValidezHasta: this.datepipe.transform(data.fechaValidezHasta, 'yyyy-MM-dd HH:mm:ss'),
      numeroRotaciones: data.numeroRotaciones,
      numeroPasajeros: data.numeroPasajeros,
      numeroTurismos: data.numeroTurismos,
      numeroAutocares: data.numeroAutocares,
      numeroCamiones: data.numeroCamiones,
      numeroTotalVehiculos: data.numeroTotalVehiculos,
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
      idOpePuertoOrigen: data.opePuertoOrigen,
      idOpePuertoDestino: data.opePuertoDestino,
      idOpeFase: data.opeFase,
      fechaValidezDesde: this.datepipe.transform(data.fechaValidezDesde, 'yyyy-MM-dd HH:mm:ss'),
      fechaValidezHasta: this.datepipe.transform(data.fechaValidezHasta, 'yyyy-MM-dd HH:mm:ss'),
      numeroRotaciones: data.numeroRotaciones,
      numeroPasajeros: data.numeroPasajeros,
      numeroTurismos: data.numeroTurismos,
      numeroAutocares: data.numeroAutocares,
      numeroCamiones: data.numeroCamiones,
      numeroTotalVehiculos: data.numeroTotalVehiculos,
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
    const endpoint = `/ope-lineas-maritimas/${id}`;

    return firstValueFrom(this.http.delete(endpoint).pipe((response) => response));
  }
}
