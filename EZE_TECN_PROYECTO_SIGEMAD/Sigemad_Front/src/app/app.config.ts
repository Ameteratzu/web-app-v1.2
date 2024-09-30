import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { DatePipe } from '@angular/common';

import { routes } from './app.routes';
import { apiUrlInterceptor } from './interceptors/api-url.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    DatePipe,
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(
      withInterceptors([
        apiUrlInterceptor,
      ]),
    )
  ]
};
