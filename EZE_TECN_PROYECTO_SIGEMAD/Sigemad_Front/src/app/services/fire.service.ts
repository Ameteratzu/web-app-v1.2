import { DatePipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { catchError, firstValueFrom, map, throwError } from 'rxjs';

import moment from 'moment';
import { ApiResponse } from '../types/api-response.type';
import { FireDetail } from '../types/fire-detail.type';
import { Fire } from '../types/fire.type';

@Injectable({ providedIn: 'root' })
export class FireService {
  public http = inject(HttpClient);
  public datepipe = inject(DatePipe);
  public endpoint = '/Incendios';

  get(query: any = '') {
    let endpoint = '/Incendios?Sort=desc&PageSize=15';

    if (query != '') {
      console.log(moment(query.start).format('MM-DD-YYYY'));

      const territory = query.territory;
      const autonomousCommunity = query.autonomousCommunity;
      const province = query.province;
      const municipality = query.municipality;
      const fireStatus = query.fireStatus;
      const episode = query.episode;
      const severityLevel = query.severityLevel;
      const affectedArea = query.affectedArea;
      const start = moment(query.start).format('YYYY-MM-DD HH:MM:SS.ssss');
      const end = moment(query.end).format('YYYY-MM-DD HH:MM:SS.ssss');
      const between = query.between;
      const move = query.move;

      endpoint = `/Incendios?PageSize=15&Sort=desc&IdTerritorio=${
        territory ? territory : ''
      }&IdCcaa=${autonomousCommunity ? autonomousCommunity : ''}&IdProvincia=${
        province ? province : ''
      }&IdMunicipio=${municipality ? municipality : ''}&IdEstado=${
        fireStatus ? fireStatus : ''
      }&IdEpisodio=${episode ? episode : ''}&IdNivelGravedad=${
        severityLevel ? severityLevel : ''
      }&IdSuperficieAfectada=${affectedArea ? affectedArea : ''}&FechaInicio=${
        start ? start : ''
      }&IdComparativoFecha=${between ? between : ''}&IdMovimiento=${
        move ? move : ''
      }&FechaFin=${end ? end : ''}&Page=1`;
    }

    return firstValueFrom(
      this.http.get<ApiResponse<Fire[]>>(endpoint).pipe((response) => response)
    );
  }

  details(fire_id: number) {
    const endpoint = `/Incendios/${fire_id}/detalles`;

    return firstValueFrom(
      this.http.get<FireDetail[]>(endpoint).pipe((response) => response)
    );
  }

  post(data: any) {
    const body = {
      IdTerritorio: data.territory,
      IdProvincia: data.province,
      IdMunicipio: data.municipality,
      denominacion: data.denomination,
      fechaInicio: this.datepipe.transform(
        data.startDate,
        'yyyy-MM-dd h:mm:ss'
      ),
      IdSuceso: data.event,
      IdTipoSuceso: data.event,
      IdClaseSuceso: data.event,
      IdEstado: 1,
      IdPeligroInicial: 1,
      notaGeneral: data.generalNote,
      GeoPosicion: data.geoposition,
      idPais: 60,
      IdEstadoSuceso: 1,
    };

    return firstValueFrom(
      this.http.post(this.endpoint, body).pipe(
        map((response) => {
          return response;
        }),
        catchError((error) => {
          return throwError(error.error);
        })
      )
    );
  }

  update(data: any) {
    const body = {
      id: data.id,
      IdTerritorio: data.territory,
      IdProvincia: data.province,
      IdMunicipio: data.municipality,
      denominacion: data.denomination,
      fechaInicio: this.datepipe.transform(
        data.startDate,
        'yyyy-MM-dd h:mm:ss'
      ),
      IdSuceso: data.event,
      IdTipoSuceso: data.event,
      IdEstadoSuceso: data.event,
      IdClaseSuceso: data.event,
      IdEstado: 1,
      IdPeligroInicial: 1,
      notaGeneral: data.generalNote,
      GeoPosicion: {
        type: 'Polygon',
        coordinates: data.coordinates,
      },
    };

    return firstValueFrom(
      this.http.put(this.endpoint, body).pipe(
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
    const endpoint = `/Incendios/${id}`;

    return firstValueFrom(
      this.http.delete(endpoint).pipe((response) => response)
    );
  }
}
