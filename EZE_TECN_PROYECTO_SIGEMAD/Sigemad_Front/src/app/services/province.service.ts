import { Injectable, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { HttpClient } from '@angular/common/http';

import { Province } from '../types/province.type';

@Injectable({ providedIn: 'root' })
export class ProvinceService {
  private http = inject(HttpClient);

  get(ac_id:number = 0) {
    const endpoint = '/Provincias';

    if (ac_id) {
      const endpoint = `/Provincias/${ac_id}`;
    } 

    return firstValueFrom(
      this.http.get<Province[]>(endpoint).pipe((response) => response),
    );
  }
}