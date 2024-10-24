import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { OriginDestination } from '../types/origin-destination.type';

@Injectable({ providedIn: 'root' })
export class OriginDestinationService {
  private http = inject(HttpClient);

  get() {
    const endpoint = '/procedencias-destinos';

    return firstValueFrom(
      this.http.get<OriginDestination[]>(endpoint).pipe((response) => response)
    );
  }
}
