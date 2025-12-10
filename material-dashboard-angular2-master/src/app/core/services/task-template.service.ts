import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TaskTemplateService {
  private apiUrl = `${environment.apiUrl}/tasktemplates`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }

  getActive(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/active`);
  }

  getByCategory(category: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/category/${category}`);
  }

  getRecurring(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/recurring`);
  }

  getById(id: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${id}`);
  }

  create(template: any): Observable<any> {
    return this.http.post<any>(this.apiUrl, template);
  }

  update(id: string, template: any): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/${id}`, template);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
