import { DatePipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { catchError, firstValueFrom, map, throwError } from 'rxjs';
import { ApiResponse } from '../../types/api-response.type';
import { OpePeriodo } from '../../types/ope/ope-periodo.type';

@Injectable({ providedIn: 'root' })
export class OpePeriodosService {
  public http = inject(HttpClient);
  public datepipe = inject(DatePipe);
  public endpoint = '/ope-periodos';

  generateUrlWitchParams({ url, params }: any) {
    return Object.keys(params).reduce((prev: any, key: any, index: any) => {
      if (!params[key]) {
        return `${prev}`;
      }
      return `${prev}&${key}=${params[key]}`;
    }, `${url}`);
  }

  get(query: any = '') {
    const URLBASE = '/ope-periodos?Sort=desc&PageSize=15';

    const endpoint = this.generateUrlWitchParams({
      url: URLBASE,
      params: query,
    });
    return firstValueFrom(this.http.get<ApiResponse<OpePeriodo[]>>(endpoint).pipe((response) => response));
  }

  post(data: any) {
    const body = {
      nombre: data.nombre,
      fechaInicioFaseSalida: this.datepipe.transform(data.fechaInicioFaseSalida, 'yyyy-MM-dd HH:mm:ss'),
      fechaFinFaseSalida: this.datepipe.transform(data.fechaFinFaseSalida, 'yyyy-MM-dd HH:mm:ss'),
      fechaInicioFaseRetorno: this.datepipe.transform(data.fechaInicioFaseRetorno, 'yyyy-MM-dd HH:mm:ss'),
      fechaFinFaseRetorno: this.datepipe.transform(data.fechaFinFaseRetorno, 'yyyy-MM-dd HH:mm:ss'),
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
      fechaInicioFaseSalida: this.datepipe.transform(data.fechaInicioFaseSalida, 'yyyy-MM-dd HH:mm:ss'),
      fechaFinFaseSalida: this.datepipe.transform(data.fechaFinFaseSalida, 'yyyy-MM-dd HH:mm:ss'),
      fechaInicioFaseRetorno: this.datepipe.transform(data.fechaInicioFaseRetorno, 'yyyy-MM-dd HH:mm:ss'),
      fechaFinFaseRetorno: this.datepipe.transform(data.fechaFinFaseRetorno, 'yyyy-MM-dd HH:mm:ss'),
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
    const endpoint = `/ope-periodos/${id}`;

    return firstValueFrom(this.http.delete(endpoint).pipe((response) => response));
  }
}
