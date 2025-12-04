import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AuthService } from '../services/auth.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private authService: AuthService) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // Get token from auth service
    const token = this.authService.getToken();
    
    // Clone request and add authorization header if token exists
    if (token) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`
        }
      });
    } else {
      console.log('No token available for request:', request.url);
    }

    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        console.error('HTTP Error:', error.status, error.url);
        
        if (error.status === 401) {
          // Only logout if this is not a login attempt
          if (!request.url.includes('/auth/login')) {
            console.warn('401 Unauthorized - logging out');
            // Give a small delay to see the error
            setTimeout(() => {
              this.authService.logout();
            }, 1000);
          }
        }
        return throwError(() => error);
      })
    );
  }
}
