import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';

import { Province } from '../types/province.type';

@Injectable({ providedIn: 'root' })
export class ProvinceService {
  private http = inject(HttpClient);

  get(ac_id: number = 0) {
    let endpoint = '/Provincias';

    if (ac_id != 0) {
      endpoint = `/Provincias/${ac_id}`;
    }

    return firstValueFrom(
      this.http.get<Province[]>(endpoint).pipe((response) => response)
    );
  }
}
