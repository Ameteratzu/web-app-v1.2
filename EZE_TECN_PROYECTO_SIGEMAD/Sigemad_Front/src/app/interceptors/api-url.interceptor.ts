import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { HttpErrorResponse, HttpRequest, HttpHandlerFn, HttpEvent } from '@angular/common/http';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { catchError, filter, switchMap, take } from 'rxjs/operators';
import { environment } from '../../environments/environment';
import { Router } from '@angular/router';

let isRefreshing = false;
const refreshTokenSubject = new BehaviorSubject<string | null>(null);

export const apiUrlInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const router = inject(Router);
  const token = sessionStorage.getItem('jwtToken');
  let apiUrl = environment.urlBase;

  if (req.url.startsWith('/assets/')) {
    return next(req);
  }

  const updatedApiUrl = req.url.includes('tipos-gestion') ? apiUrl.replace('/v1', '') : apiUrl;
  const updatedUrl = req.url.startsWith('http') ? req.url : `${updatedApiUrl}${req.url}`;

  let modifiedReq = req.clone({ url: updatedUrl });

  if (token) {
    modifiedReq = modifiedReq.clone({
      setHeaders: { Authorization: `Bearer ${token}` },
    });
  }

  return next(modifiedReq).pipe(
    catchError((error: HttpErrorResponse) => {
      // sessionStorage.removeItem('jwtToken')
      // sessionStorage.removeItem('refreshToken');
      // router.navigate(['/login']);
      if (error.status === 401 && !req.url.includes('/refresh-token')) {
        return handle401Error(modifiedReq, next, authService);
      }
      return throwError(() => error);
    })
  );
};

const handle401Error = (req: HttpRequest<any>, next: HttpHandlerFn, authService: AuthService): Observable<HttpEvent<any>> => {
  if (!isRefreshing) {
    isRefreshing = true;
    refreshTokenSubject.next(null);

    const refreshToken = sessionStorage.getItem('refreshToken');
    if (!refreshToken) {
      return throwError(() => new Error('Refresh token no disponible'));
    }

    return authService.refreshToken(refreshToken).pipe(
      switchMap((response: any) => {
        isRefreshing = false;
        refreshTokenSubject.next(response.token);
        sessionStorage.setItem('jwtToken', response.token);

        const modifiedReq = req.clone({
          setHeaders: { Authorization: `Bearer ${response.token}` },
        });
        return next(modifiedReq);
      }),
      catchError((err) => {
        isRefreshing = false;
        return throwError(() => err);
      })
    );
  } else {
    return refreshTokenSubject.pipe(
      filter((token) => token != null),
      take(1),
      switchMap((token) => {
        const modifiedReq = req.clone({
          setHeaders: { Authorization: `Bearer ${token}` },
        });
        return next(modifiedReq);
      })
    );
  }
};
