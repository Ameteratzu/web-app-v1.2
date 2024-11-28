import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { Phases } from '../types/phases.type';

@Injectable({ providedIn: 'root' })
export class PhasesService {
  private http = inject(HttpClient);

  get() {
    const endpoint = '/fases';

    return firstValueFrom(
      this.http.get<Phases[]>(endpoint).pipe((response) => response)
    );
  }
}
