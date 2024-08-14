import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';


import { map, delay } from 'rxjs/operators'
import { EstadosAlertasResponse } from './EstadosAlertasResponse';


@Injectable({
  providedIn: 'root'
})
export class EstadosAlertasService {

  private url: string = 'http://localhost:5246/api/v1/EstadoAlerta/pagination?PageSize=10'


  constructor( private _http: HttpClient ) { }


  getEstadosAlertas() {
    return this._http.get<EstadosAlertasResponse>(`${ this.url}`)
        .pipe(
          map( (reponse) => reponse.data ),
        );
  }
}