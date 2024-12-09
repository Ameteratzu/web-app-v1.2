import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { catchError, firstValueFrom, map, throwError } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class FireDocumentationService {
  public http = inject(HttpClient);

  getById(id: Number) {
    let endpoint = `/Documentaciones/${id}`;

    return firstValueFrom(
      this.http.get<any[]>(endpoint).pipe((response) => response)
    );
  }
  post(data: any) {
    const endpoint = '/Documentaciones';

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

  //delete
  delete(id: number) {
    const endpoint = `/Documentaciones/${id}`;

    return firstValueFrom(
      this.http.delete(endpoint).pipe((response) => response)
    );
  }
}
