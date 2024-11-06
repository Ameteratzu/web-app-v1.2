import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';

import { AutonomousCommunity } from '../types/autonomous-community.type';

@Injectable({ providedIn: 'root' })
export class AutonomousCommunityService {
  private http = inject(HttpClient);

  get() {
    const endpoint = '/ComunidadesAutonomas';

    return firstValueFrom(
      this.http
        .get<AutonomousCommunity[]>(endpoint)
        .pipe((response) => response)
    );
  }

  getByCountry(idCountry: string) {
    const endpoint = `/paises/${idCountry}/comunidades`;

    return firstValueFrom(
      this.http
        .get<AutonomousCommunity[]>(endpoint)
        .pipe((response) => response)
    );
  }
}
