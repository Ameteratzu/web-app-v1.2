import { Injectable, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class MenuService {
  private http = inject(HttpClient);

  get() {
    const endpoint = '/Menus';

    return firstValueFrom(
      this.http.get(endpoint).pipe((response) => response),
    );
  }
}