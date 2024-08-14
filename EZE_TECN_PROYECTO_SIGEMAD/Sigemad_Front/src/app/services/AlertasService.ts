import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Alerta } from '../models/alerta';
import { map, delay } from 'rxjs/operators'
import { AlertasResponse } from './Alertasresponse';

@Injectable({
  providedIn: 'root'
})
export class AlertasService {

  private url: string = 'http://localhost:5246/api/v1/alerta/pagination?PageSize=11'
  private urlById: string = 'http://localhost:5246/api/v1/Alerta/ObtenerAlerta?id='

  constructor( private _http: HttpClient ) { }


  getAlertas() {
    return this._http.get<AlertasResponse>(`${ this.url}`)
        .pipe(
          map( (reponse) => reponse.data ),
        );
  }

  getAlerta(id: string)
  {
    return this._http.get<Alerta>(`${ this.urlById + id}`)
        .pipe(
        map( (reponse) => reponse ),
        
    );
  }
}