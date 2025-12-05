import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'environments/environment';
import {
  WorkTask,
  CreateWorkTaskDTO,
  UpdateWorkTaskDTO
} from '../models/hr.model';

@Injectable({
  providedIn: 'root'
})
export class WorkTaskService {
  private apiUrl = `${environment.apiUrl}/worktasks`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<WorkTask[]> {
    return this.http.get<WorkTask[]>(this.apiUrl);
  }

  getById(id: string): Observable<WorkTask> {
    return this.http.get<WorkTask>(`${this.apiUrl}/${id}`);
  }

  getByStatus(statusId: string): Observable<WorkTask[]> {
    return this.http.get<WorkTask[]>(`${this.apiUrl}/status/${statusId}`);
  }

  getByTeam(teamId: string): Observable<WorkTask[]> {
    return this.http.get<WorkTask[]>(`${this.apiUrl}/team/${teamId}`);
  }

  getByEmployee(employeeId: string): Observable<WorkTask[]> {
    return this.http.get<WorkTask[]>(`${this.apiUrl}/employee/${employeeId}`);
  }

  getScheduledTasks(date: string): Observable<WorkTask[]> {
    return this.http.get<WorkTask[]>(`${this.apiUrl}/scheduled/${date}`);
  }

  create(task: CreateWorkTaskDTO): Observable<WorkTask> {
    return this.http.post<WorkTask>(this.apiUrl, task);
  }

  update(id: string, task: UpdateWorkTaskDTO): Observable<WorkTask> {
    return this.http.put<WorkTask>(`${this.apiUrl}/${id}`, task);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
