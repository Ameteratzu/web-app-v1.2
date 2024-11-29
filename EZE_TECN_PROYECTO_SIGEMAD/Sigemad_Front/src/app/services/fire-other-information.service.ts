import { DatePipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { catchError, firstValueFrom, map, throwError } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class FireOtherInformationService {
  public http = inject(HttpClient);
  public datepipe = inject(DatePipe);
  //public endpoint = '/Incendios';
  /*
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
*/
  post(data: any) {
    const endpoint = '/otras-informaciones/lista';

    return firstValueFrom(
      this.http.post(endpoint, data).pipe(
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
