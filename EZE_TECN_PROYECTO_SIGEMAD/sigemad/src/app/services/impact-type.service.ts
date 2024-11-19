import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class ImpactTypeService {
  private http = inject(HttpClient);

  get() {
    const endpoint = '/tipos-impactos';
    return firstValueFrom(this.http.get(endpoint).pipe((response) => response));
  }
}
