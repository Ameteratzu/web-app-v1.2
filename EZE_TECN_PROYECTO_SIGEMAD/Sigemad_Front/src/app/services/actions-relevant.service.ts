import { HttpClient } from '@angular/common/http';
import { Injectable, inject, signal } from '@angular/core';
import { catchError, firstValueFrom, map, throwError } from 'rxjs';
import { EmergenciaNacional } from '../types/actions-relevant.type';

@Injectable({ providedIn: 'root' })
export class ActionsRelevantService {
  private http = inject(HttpClient);
  public dataEmergencia = signal<EmergenciaNacional[]>([]);

  postData(body: any) {
    const endpoint = `/actuaciones-relevantes/emergencia-nacional`;

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

  update(body: any) {
    const endpoint = `/Evoluciones`;

    return firstValueFrom(
      this.http.put(endpoint, body).pipe(
        map((response) => {
          return response;
        }),
        catchError((error) => {
          return throwError(error.error);
        })
      )
    );
  }

  clearData(): void {
    this.dataEmergencia.set([]);
  }

  getById(id: Number) {
    let endpoint = `/Evoluciones/${id}`;
    return firstValueFrom(this.http.get<any[]>(endpoint).pipe((response) => response));
  }

  delete(id: number) {
    const endpoint = `/Evoluciones/${id}`;

    return firstValueFrom(this.http.delete(endpoint).pipe((response) => response));
  }

  deleteConse(id: number) {
    const endpoint = `/evoluciones/${id}`;

    return firstValueFrom(this.http.delete(endpoint).pipe((response) => response));
  }
}
