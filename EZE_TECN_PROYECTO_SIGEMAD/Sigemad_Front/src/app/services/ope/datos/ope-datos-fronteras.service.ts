import { DatePipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { catchError, firstValueFrom, map, throwError } from 'rxjs';
import { ApiResponse } from '../../../types/api-response.type';
import { OpeDatoFrontera } from '@type/ope/datos/ope-dato-frontera.type';

@Injectable({ providedIn: 'root' })
export class OpeDatosFronterasService {
  public http = inject(HttpClient);
  public datepipe = inject(DatePipe);
  public endpoint = '/ope-datos-fronteras';

  generateUrlWitchParams({ url, params }: any) {
    return Object.keys(params).reduce((prev: any, key: any, index: any) => {
      if (!params[key]) {
        return `${prev}`;
      }
      return `${prev}&${key}=${params[key]}`;
    }, `${url}`);
  }

  get(query: any = '') {
    const URLBASE = '/ope-datos-fronteras?Sort=desc&PageSize=15';

    const endpoint = this.generateUrlWitchParams({
      url: URLBASE,
      params: query,
    });
    return firstValueFrom(this.http.get<ApiResponse<OpeDatoFrontera[]>>(endpoint).pipe((response) => response));
  }

  post(data: any) {
    const body = {
      //idOpeFrontera: data.opeFrontera,
      idOpeFrontera: data.opeFrontera,
      fechaHoraInicioIntervalo: this.datepipe.transform(data.fechaHoraInicioIntervalo, 'yyyy-MM-dd HH:mm:ss'),
      fechaHoraFinIntervalo: this.datepipe.transform(data.fechaHoraFinIntervalo, 'yyyy-MM-dd HH:mm:ss'),
      numeroVehiculos: data.numeroVehiculos,
      afluencia: data.afluencia,
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
      idOpeFrontera: data.opeFrontera,
      fechaHoraInicioIntervalo: this.datepipe.transform(data.fechaHoraInicioIntervalo, 'yyyy-MM-dd HH:mm:ss'),
      fechaHoraFinIntervalo: this.datepipe.transform(data.fechaHoraFinIntervalo, 'yyyy-MM-dd HH:mm:ss'),
      numeroVehiculos: data.numeroVehiculos,
      afluencia: data.afluencia,
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
    const endpoint = `/ope-datos-fronteras/${id}`;

    return firstValueFrom(this.http.delete(endpoint).pipe((response) => response));
  }
}
