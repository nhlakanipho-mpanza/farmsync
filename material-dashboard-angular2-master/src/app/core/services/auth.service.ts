import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, BehaviorSubject, throwError } from 'rxjs';
import { map, catchError, tap } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { User, LoginRequest, LoginResponse, RegisterRequest } from '../models/auth.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private currentUserSubject: BehaviorSubject<User | null>;
  public currentUser: Observable<User | null>;
  private readonly TOKEN_KEY = 'farmsync_token';
  private readonly USER_KEY = 'farmsync_user';

  constructor(
    private http: HttpClient,
    private router: Router
  ) {
    const storedUser = localStorage.getItem(this.USER_KEY);
    this.currentUserSubject = new BehaviorSubject<User | null>(
      storedUser ? JSON.parse(storedUser) : null
    );
    this.currentUser = this.currentUserSubject.asObservable();
  }

  public get currentUserValue(): User | null {
    return this.currentUserSubject.value;
  }

  login(credentials: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${environment.apiUrl}/auth/login`, credentials)
      .pipe(
        tap(response => {
          console.log('Login response:', response);
          if (response && response.token) {
            // Store token and user info
            localStorage.setItem(this.TOKEN_KEY, response.token);
            
            const user: User = {
              id: '', // Will be extracted from token
              username: response.username,
              email: response.email,
              fullName: response.fullName,
              roles: response.roles
            };
            
            localStorage.setItem(this.USER_KEY, JSON.stringify(user));
            this.currentUserSubject.next(user);
          }
        }),
        catchError(error => {
          console.error('Login error:', error);
          return throwError(() => error);
        })
      );
  }

  register(data: RegisterRequest): Observable<any> {
    return this.http.post(`${environment.apiUrl}/auth/register`, data)
      .pipe(
        catchError(error => {
          console.error('Registration error:', error);
          return throwError(() => error);
        })
      );
  }

  logout(): void {
    localStorage.removeItem(this.TOKEN_KEY);
    localStorage.removeItem(this.USER_KEY);
    this.currentUserSubject.next(null);
    this.router.navigate(['/login']);
  }

  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY);
  }

  isAuthenticated(): boolean {
    const token = this.getToken();
    
    if (!token) {
      return false;
    }
    
    const isExpired = this.isTokenExpired(token);
    
    return !isExpired;
  }

  hasRole(role: string): boolean {
    const user = this.currentUserValue;
    return user ? user.roles.includes(role) : false;
  }

  hasAnyRole(roles: string[]): boolean {
    const user = this.currentUserValue;
    if (!user) return false;
    return roles.some(role => user.roles.includes(role));
  }

  private isTokenExpired(token: string): boolean {
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      
      // Check for standard exp claim or Microsoft exp claim
      const expiry = payload.exp || payload['exp'];
      
      // If no expiry claim, assume token is valid (some tokens don't have exp)
      if (!expiry) {
        console.warn('Token has no expiration claim, assuming valid');
        return false;
      }
      
      const currentTime = Math.floor((new Date()).getTime() / 1000);
      return currentTime >= expiry;
    } catch (error) {
      console.error('Error checking token expiration:', error);
      return true;
    }
  }

  getUserFromToken(): User | null {
    const token = this.getToken();
    if (!token) return null;

    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      
      // Handle Microsoft JWT claim format
      const roleKey = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';
      const nameKey = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name';
      const emailKey = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress';
      
      const roles = payload[roleKey];
      const rolesArray = Array.isArray(roles) ? roles : [roles];
      
      return {
        id: payload.sub || '',
        username: payload[nameKey] || payload.unique_name || '',
        email: payload[emailKey] || payload.email || '',
        fullName: payload.fullName || payload[nameKey] || '',
        roles: rolesArray
      };
    } catch (error) {
      console.error('Error parsing token:', error);
      return null;
    }
  }
}
