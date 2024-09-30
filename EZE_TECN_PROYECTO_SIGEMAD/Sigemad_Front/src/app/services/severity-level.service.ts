import { Injectable, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { HttpClient } from '@angular/common/http';

import { SeverityLevel } from '../types/severity-level.type';

@Injectable({ providedIn: 'root' })
export class SeverityLevelService {
  private http = inject(HttpClient);

  get() {
    const endpoint = '/NivelGravedad';

    return firstValueFrom(
      this.http.get<SeverityLevel[]>(endpoint).pipe((response) => response),
    );
  }
}