import { DatePipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { catchError, firstValueFrom, map, throwError } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class OpeDatosFronterasService {
  public http = inject(HttpClient);
  public datepipe = inject(DatePipe);

  getById(id: Number) {
    let endpoint = `/ope-datos-fronteras/?idOpeFrontera=${id}`;
    return firstValueFrom(this.http.get<any[]>(endpoint).pipe((response) => response));
  }

  post(data: any) {
    const endpoint = '/ope-datos-fronteras/lista';

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

  delete(id: number) {
    const endpoint = `/ope-datos-fronteras/${id}`;

    return firstValueFrom(this.http.delete(endpoint).pipe((response) => response));
  }
}
