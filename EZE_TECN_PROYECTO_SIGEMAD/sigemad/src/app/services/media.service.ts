import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { Media } from '../types/media.type';

@Injectable({ providedIn: 'root' })
export class MediaService {
  private http = inject(HttpClient);

  get() {
    const endpoint = '/medios';

    return firstValueFrom(
      this.http.get<Media[]>(endpoint).pipe((response) => response)
    );
  }
}
