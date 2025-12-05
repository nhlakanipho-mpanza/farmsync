import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { DriverAssignment, CreateDriverAssignmentDTO, UpdateDriverAssignmentDTO } from '../models/fleet.model';

@Injectable({
  providedIn: 'root'
})
export class DriverAssignmentService {
  private apiUrl = `${environment.apiUrl}/driverassignment`;

  constructor(private http: HttpClient) { }

  getCurrentAssignments(): Observable<DriverAssignment[]> {
    return this.http.get<DriverAssignment[]>(`${this.apiUrl}/current`);
  }

  getById(id: string): Observable<DriverAssignment> {
    return this.http.get<DriverAssignment>(`${this.apiUrl}/${id}`);
  }

  getByVehicle(vehicleId: string): Observable<DriverAssignment[]> {
    return this.http.get<DriverAssignment[]>(`${this.apiUrl}/vehicle/${vehicleId}`);
  }

  getCurrentByVehicle(vehicleId: string): Observable<DriverAssignment> {
    return this.http.get<DriverAssignment>(`${this.apiUrl}/vehicle/${vehicleId}/current`);
  }

  getByDriver(driverId: string): Observable<DriverAssignment[]> {
    return this.http.get<DriverAssignment[]>(`${this.apiUrl}/driver/${driverId}`);
  }

  assignDriver(dto: CreateDriverAssignmentDTO): Observable<DriverAssignment> {
    return this.http.post<DriverAssignment>(this.apiUrl, dto);
  }

  updateAssignment(id: string, dto: UpdateDriverAssignmentDTO): Observable<DriverAssignment> {
    return this.http.put<DriverAssignment>(`${this.apiUrl}/${id}`, dto);
  }

  endAssignment(id: string, endDate: string): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/${id}/end`, JSON.stringify(endDate), {
      headers: { 'Content-Type': 'application/json' }
    });
  }

  deleteAssignment(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
