import { HttpClient } from '@angular/common/http';
import { Injectable, inject, signal } from '@angular/core';
import { catchError, firstValueFrom, map, throwError } from 'rxjs';
import { EmergenciaNacional, Zagep, Cecod, Notificaciones, Planes, ActivacionSistemas } from '../types/actions-relevant.type';
import { Movilizacion } from '../types/mobilization.type';

@Injectable({ providedIn: 'root' })
export class ActionsRelevantService {
  private http = inject(HttpClient);
  public dataEmergencia = signal<EmergenciaNacional[]>([]);
  public dataZagep = signal<Zagep[]>([]);
  public dataCecod = signal<Cecod[]>([]);
  public dataNotificaciones = signal<Notificaciones[]>([]);
  public dataPlanes = signal<Planes[]>([]);
  public dataSistemas = signal<ActivacionSistemas[]>([]);
  public dataMovilizacion = signal<Movilizacion[]>([]);

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

  postPlanes(data: any) {
    const endpoint = '/actuaciones-relevantes/activaciones-planes/lista';
  
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

  postSistemas(data: any) {
    const endpoint = '/actuaciones-relevantes/activaciones-sistemas/lista';
  
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

  clearData(): void {
    this.dataEmergencia.set([]);
    this.dataZagep.set([]);
    this.dataCecod.set([]);
    this.dataNotificaciones.set([]);
    this.dataPlanes.set([]);
    this.dataSistemas.set([]);
    this.dataMovilizacion.set([]);
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

  getAllPlanes() {
    let endpoint = `/tipos-planes`;
    return firstValueFrom(this.http.get<any[]>(endpoint).pipe((response) => response));
  }

  getModosActivacion() {
    let endpoint = `/modos-activacion`;
    return firstValueFrom(this.http.get<any[]>(endpoint).pipe((response) => response));
  }

  getTipoActivacion() {
    let endpoint = `/tipos-sistemas-emergencia`;
    return firstValueFrom(this.http.get<any[]>(endpoint).pipe((response) => response));
  }

  getTipoGestion(id?: number) {
    let endpoint = id ? `/movilizaciones-medios/tipos-gestion?IdPasoActual=${id}` : '/movilizaciones-medios/tipos-gestion';
    return firstValueFrom(this.http.get<any[]>(endpoint).pipe((response) => response));
  }
  
}
