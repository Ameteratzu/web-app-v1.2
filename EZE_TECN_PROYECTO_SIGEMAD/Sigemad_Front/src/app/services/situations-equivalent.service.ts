import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';

import { SituationsEquivalent } from '../types/situations-equivalent.type';

@Injectable({ providedIn: 'root' })
export class SituationsEquivalentService {
  private http = inject(HttpClient);

  get() {
    //const endpoint = '/NivelGravedad'; ANTIGUO
    const endpoint = '/situaciones-equivalentes';

    return firstValueFrom(this.http.get<SituationsEquivalent[]>(endpoint).pipe((response) => response));
  }
}
