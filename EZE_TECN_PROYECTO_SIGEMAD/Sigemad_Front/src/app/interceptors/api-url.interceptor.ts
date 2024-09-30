import { HttpInterceptorFn } from '@angular/common/http';
import { environment } from '../../environments/environment';

export const apiUrlInterceptor: HttpInterceptorFn = (req, next) => {
  const apiUrl = environment.urlBase;

  const withUrlReq = req.clone({
    url: `${apiUrl}${req.url}`,
  });

  return next(withUrlReq);
};
