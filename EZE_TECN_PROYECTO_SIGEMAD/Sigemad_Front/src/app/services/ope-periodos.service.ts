import { DatePipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { catchError, firstValueFrom, map, throwError } from 'rxjs';

import { ApiResponse } from '../types/api-response.type';
import { FireDetail, FireDetailResponse } from '../types/fire-detail.type';
import { Fire } from '../types/fire.type';
import { OpePeriodo } from '../types/ope-periodo.type';

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
      denominacion: data.denomination,
      fechaInicio: this.datepipe.transform(data.startDateTime, 'yyyy-MM-dd  h:mm:ss'),
      fechaFin: this.datepipe.transform(data.endDateTime, 'yyyy-MM-dd  h:mm:ss'),
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
      denominacion: data.denomination,
      fechaInicio: this.datepipe.transform(data.startDateTime, 'yyyy-MM-dd h:mm:ss'),
      fechaFin: this.datepipe.transform(data.endDateTime, 'yyyy-MM-dd h:mm:ss'),
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
