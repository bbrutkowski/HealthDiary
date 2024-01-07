import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpInterceptorFn, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';
import { AuthService } from '../services/auth.service/auth.service';
import { Router } from '@angular/router';

@Injectable()
export class TokenInterceptor implements HttpInterceptor  {

  public constructor(private authService: AuthService, private router: Router) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    const token = this.authService.getToken();

    if(token){
      request.clone({
        setHeaders: {Authorization: `Bearer ${token}`}
      });
    }
    return next.handle(request).pipe(
      catchError((err: any) => {
        if(err instanceof HttpErrorResponse) {
          if(err.status === 401){
            console.error('Token is expired. Login again');
            this.router.navigate(['/login']);
          }
        }
        return throwError(() => new Error('Unexpected error occured'))
      })
    );
  }

};
