import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { InputOutput } from '../types/input-output.type';

@Injectable({ providedIn: 'root' })
export class InputOutputService {
  private http = inject(HttpClient);

  get() {
    const endpoint = '/entradas-salidas';

    return firstValueFrom(
      this.http.get<InputOutput[]>(endpoint).pipe((response) => response)
    );
  }
}
