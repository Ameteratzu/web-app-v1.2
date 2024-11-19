import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { MinorEntity } from '../types/minor-entity.type';

@Injectable({ providedIn: 'root' })
export class MinorEntityService {
  private http = inject(HttpClient);

  get() {
    const endpoint = '/entidad-menor';

    return firstValueFrom(
      this.http.get<MinorEntity[]>(endpoint).pipe((response) => response)
    );
  }
}
