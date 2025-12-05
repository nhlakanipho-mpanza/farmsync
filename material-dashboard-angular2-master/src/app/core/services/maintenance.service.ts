import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'environments/environment';
import { MaintenanceRecord, CreateMaintenanceRecordDTO, UpdateMaintenanceRecordDTO } from '../models/fleet.model';

@Injectable({
  providedIn: 'root'
})
export class MaintenanceService {
  private apiUrl = `${environment.apiUrl}/maintenance`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<MaintenanceRecord[]> {
    return this.http.get<MaintenanceRecord[]>(this.apiUrl);
  }

  getById(id: string): Observable<MaintenanceRecord> {
    return this.http.get<MaintenanceRecord>(`${this.apiUrl}/${id}`);
  }

  getByVehicle(vehicleId: string): Observable<MaintenanceRecord[]> {
    return this.http.get<MaintenanceRecord[]>(`${this.apiUrl}/vehicle/${vehicleId}`);
  }

  getOverdue(): Observable<MaintenanceRecord[]> {
    return this.http.get<MaintenanceRecord[]>(`${this.apiUrl}/overdue`);
  }

  getByType(typeId: string): Observable<MaintenanceRecord[]> {
    return this.http.get<MaintenanceRecord[]>(`${this.apiUrl}/type/${typeId}`);
  }

  getByDateRange(startDate: string, endDate: string): Observable<MaintenanceRecord[]> {
    return this.http.get<MaintenanceRecord[]>(
      `${this.apiUrl}/date-range?startDate=${startDate}&endDate=${endDate}`
    );
  }

  create(maintenance: CreateMaintenanceRecordDTO): Observable<MaintenanceRecord> {
    return this.http.post<MaintenanceRecord>(this.apiUrl, maintenance);
  }

  update(id: string, maintenance: UpdateMaintenanceRecordDTO): Observable<MaintenanceRecord> {
    return this.http.put<MaintenanceRecord>(`${this.apiUrl}/${id}`, maintenance);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
