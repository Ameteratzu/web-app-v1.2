import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';

import { Municipality } from '../types/municipality.type';

@Injectable({ providedIn: 'root' })
export class MunicipalityService {
  private http = inject(HttpClient);

  get(province_id: number) {
    const endpoint = `/Municipios/${province_id}`;

    return firstValueFrom(
      this.http.get<Municipality[]>(endpoint).pipe((response) => response)
    );
  }
}
