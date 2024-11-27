import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { catchError, firstValueFrom, map, throwError } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class CoordinationAddressService {
  private http = inject(HttpClient);

  // getByEvolution(id: Number) {
  //   let endpoint = `/areas-afectadas/evolucion/${id}`;

  //   return firstValueFrom(
  //     this.http.get<any[]>(endpoint).pipe((response) => response)
  //   );
  // }

  postAddress(body: any) {
    const endpoint = `/direcciones-coordinaciones-emergencias/direcciones`;

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

  postCecopi(body: any) {
    const endpoint = `/direcciones-coordinaciones-emergencias/coordinaciones-cecopi`;

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

  postPma(body: any) {
    const endpoint = `/direcciones-coordinaciones-emergencias/coordinaciones-pma`;

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
  
}
