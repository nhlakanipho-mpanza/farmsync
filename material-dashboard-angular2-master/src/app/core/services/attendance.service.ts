import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'environments/environment';
import {
  ClockEvent,
  CreateClockEventDTO,
  AttendanceSummary
} from '../models/hr.model';

@Injectable({
  providedIn: 'root'
})
export class AttendanceService {
  private apiUrl = `${environment.apiUrl}/attendance`;

  constructor(private http: HttpClient) {}

  getAllEvents(): Observable<ClockEvent[]> {
    return this.http.get<ClockEvent[]>(`${this.apiUrl}/events`);
  }

  getEmployeeEvents(employeeId: string, fromDate?: string, toDate?: string): Observable<ClockEvent[]> {
    let params = new HttpParams();
    if (fromDate) params = params.set('fromDate', fromDate);
    if (toDate) params = params.set('toDate', toDate);
    
    return this.http.get<ClockEvent[]>(`${this.apiUrl}/employee/${employeeId}/events`, { params });
  }

  clockEvent(event: CreateClockEventDTO): Observable<ClockEvent> {
    return this.http.post<ClockEvent>(`${this.apiUrl}/clock`, event);
  }

  getDailySummary(employeeId: string, date: string): Observable<AttendanceSummary> {
    return this.http.get<AttendanceSummary>(`${this.apiUrl}/employee/${employeeId}/daily/${date}`);
  }

  getTeamDailySummary(teamId: string, date: string): Observable<AttendanceSummary[]> {
    return this.http.get<AttendanceSummary[]>(`${this.apiUrl}/team/${teamId}/daily/${date}`);
  }
}
