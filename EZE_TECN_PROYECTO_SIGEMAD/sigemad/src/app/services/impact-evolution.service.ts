import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { catchError, firstValueFrom, map, throwError } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class ImpactEvolutionService {
  private http = inject(HttpClient);

  post(body: any) {
    const endpoint = '/impactos-evoluciones';

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
