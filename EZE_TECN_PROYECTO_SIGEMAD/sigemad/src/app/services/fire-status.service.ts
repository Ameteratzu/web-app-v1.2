import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';

import { FireStatus } from '../types/fire-status.type';

@Injectable({ providedIn: 'root' })
export class FireStatusService {
  private http = inject(HttpClient);

  get() {
    const endpoint = '/estados-incendios';

    return firstValueFrom(
      this.http.get<FireStatus[]>(endpoint).pipe((response) => response)
    );
  }
}
