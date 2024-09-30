import { Injectable, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { HttpClient } from '@angular/common/http';

import { Event } from '../types/event.type';

@Injectable({ providedIn: 'root' })
export class EventService {
  private http = inject(HttpClient);

  get() {
    const endpoint = '/TipoSucesos';

    return firstValueFrom(
      this.http.get<Event[]>(endpoint).pipe((response) => response),
    );
  }
}