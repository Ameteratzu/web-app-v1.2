import { HttpClient } from '@angular/common/http';
import { Injectable, inject, signal } from '@angular/core';
import { catchError, firstValueFrom, map, throwError } from 'rxjs';

interface FormTypeCecopi {
  id?: string,
  idIncendio?: number
  fechaInicio: Date,
  fechaFin: Date,
  idProvincia: { id: number, descripcion: string };
  idMunicipio: { id: number, descripcion: string };
  lugar?: string,
  observaciones?: string,
}

interface FormTypeAddress {
  id?: string,
  autoridadQueDirige: string,
  idIncendio?: number
  fechaInicio: Date,
  fechaFin: Date,
  idTipoDireccionEmergencia: { id: number, descripcion: string };
}

interface FormTypePma{
  id?: string,
  autoridadQueDirige: string,
  idIncendio?: number
  fechaInicio: Date,
  fechaFin: Date,
  idTipoDireccionEmergencia: number,
  idProvincia: { id: number, descripcion: string };
  idMunicipio: { id: number, descripcion: string };
  lugar?: string,
}

@Injectable({ providedIn: 'root' })
export class CoordinationAddressService {
  private http = inject(HttpClient);
  public dataCecopi = signal<FormTypeCecopi[]>([]); 
  public dataCoordinationAddress = signal<FormTypeAddress[]>([]); 
  public dataPma = signal<FormTypePma[]>([]); 

  clearData(): void {
    this.dataCecopi.set([]); 
    this.dataCoordinationAddress.set([]);
    this.dataPma.set([]);
  }

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
