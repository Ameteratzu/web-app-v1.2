import { HttpClient } from '@angular/common/http';
import { Injectable, inject, signal } from '@angular/core';
import { catchError, firstValueFrom, map, throwError } from 'rxjs';
import { EmergenciaNacional, Zagep, Cecod, Notificaciones } from '../types/actions-relevant.type';

@Injectable({ providedIn: 'root' })
export class ActionsRelevantService {
  private http = inject(HttpClient);
  public dataEmergencia = signal<EmergenciaNacional[]>([]);
  public dataZagep = signal<Zagep[]>([]);
  public dataCecod = signal<Cecod[]>([]);
  public dataNotificaciones = signal<Notificaciones[]>([]);

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

  postDataZagep(body: any) {
    const endpoint = `/actuaciones-relevantes/declaraciones-zagep/lista`;

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

  postDataCecod(body: any) {
    const endpoint = `/actuaciones-relevantes/convocatoria-cecod/lista`;

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

  postDataNotificaciones(body: any) {
    const endpoint = `/actuaciones-relevantes/notificaciones/lista`;

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

  clearData(): void {
    this.dataEmergencia.set([]);
    this.dataZagep.set([]);
    this.dataCecod.set([]);
    this.dataNotificaciones.set([]);
  }

  getById(id: Number) {
    let endpoint = `/actuaciones-relevantes/${id}`;
    return firstValueFrom(this.http.get<any[]>(endpoint).pipe((response) => response));
  }

  deleteActions(id: number) {
    const endpoint = `/actuaciones-relevantes/${id}`;
    return firstValueFrom(this.http.delete(endpoint).pipe((response) => response));
  }

  //Maestros
  getTipoNotificacion() {
    let endpoint = `/tipo-notificaciones`;
    return firstValueFrom(this.http.get<any[]>(endpoint).pipe((response) => response));
  }

}
