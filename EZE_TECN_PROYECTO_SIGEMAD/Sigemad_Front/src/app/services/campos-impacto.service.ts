import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { Campo } from '../types/Campo.type';

@Injectable({ providedIn: 'root' })
export class CamposImpactoService {
  private http = inject(HttpClient);

  getFieldsById(id: string) {
    let endpoint = `/campos-impactos/${id}`;

    return firstValueFrom(
      this.http.get<Campo[]>(endpoint).pipe((response) => response)
    );
  }
}
