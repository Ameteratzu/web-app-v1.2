import { HttpInterceptorFn } from '@angular/common/http';
import { environment } from '../../environments/environment';

export const apiUrlInterceptor: HttpInterceptorFn = (req, next) => {
  const apiUrl = environment.urlBase;

  // Excluir rutas locales como 'assets/'
  if (req.url.startsWith('/assets/')) {
    return next(req); // No modifica la solicitud
  }

  // Modificar solicitudes al backend
  const withUrlReq = req.clone({
    url: `${apiUrl}${req.url}`,
  });

  return next(withUrlReq);
};