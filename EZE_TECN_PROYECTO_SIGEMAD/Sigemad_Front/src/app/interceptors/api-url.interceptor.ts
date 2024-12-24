import { HttpInterceptorFn } from '@angular/common/http';
import { environment } from '../../environments/environment';

export const apiUrlInterceptor: HttpInterceptorFn = (req, next) => {
  const apiUrl = environment.urlBase;
  const token = sessionStorage.getItem('jwtToken'); // Obtén el token desde sessionStorage

  if (req.url.startsWith('/assets/')) {
    return next(req);
  }

  const headers = token
    ? req.headers.set('Authorization', `Bearer ${token}`) // Inserta el token en el encabezado Authorization
    : req.headers;

  const withUrlAndTokenReq = req.clone({
    url: `${apiUrl}${req.url}`,
    headers,
  });

  return next(withUrlAndTokenReq);
};
