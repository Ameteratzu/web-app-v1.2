import { Injectable, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { HttpClient } from '@angular/common/http';

import { Territory } from '../types/territory.type';

@Injectable({ providedIn: 'root' })
export class TerritoryService {
  private http = inject(HttpClient);

  get() {
    const endpoint = '/Territorios';

    return firstValueFrom(
      this.http.get<Territory[]>(endpoint).pipe((response) => response),
    );
  }
}