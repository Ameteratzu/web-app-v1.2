import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { catchError, firstValueFrom, map, throwError } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class SucesosRelacionadosService {
  private http = inject(HttpClient);
  generateUrlWitchParams({ url, params }: any) {
    return Object.keys(params).reduce((prev: any, key: any, index: any) => {
      if (!params[key]) {
        return `${prev}`;
      }
      return `${prev}&${key}=${params[key]}`;
    }, `${url}`);
  }

  getListaSuceso(query: any = '') {
    const URLBASE = '/Sucesos?Sort=desc&PageSize=15';

    const endpoint = this.generateUrlWitchParams({
      url: URLBASE,
      params: query,
    });

    return firstValueFrom(this.http.get<any[]>(endpoint).pipe((response) => response));
  }

  get(idSucesoPrincipal: string | number) {
    const endpoint = `/sucesos/${idSucesoPrincipal}/relacionados`;

    return firstValueFrom(this.http.get<any[]>(endpoint).pipe((response) => response));
  }

  post(idSucesoPrincipal: string | number, body: any) {
    const endpoint = `/sucesos/${idSucesoPrincipal}/relacionados`;
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

  delete({ idSucesoPrincipal, idSucesoAsociado }: { idSucesoPrincipal: number; idSucesoAsociado: number }) {
    const endpoint = `/sucesos/${idSucesoPrincipal}/relacionados/${idSucesoAsociado}`;
    return firstValueFrom(this.http.delete(endpoint).pipe((response) => response));
  }
}
