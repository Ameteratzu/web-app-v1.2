import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { ComparativeDate } from '../types/comparative-date.type';

@Injectable({ providedIn: 'root' })
export class ComparativeDateService {
  private http = inject(HttpClient);

  get() {
    let endpoint = '/comparativa-fechas';

    return firstValueFrom(
      this.http.get<ComparativeDate[]>(endpoint).pipe((response) => response)
    );
  }
}
