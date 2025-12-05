import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'environments/environment';
import { Vehicle, CreateVehicleDTO, UpdateVehicleDTO } from '../models/fleet.model';

@Injectable({
  providedIn: 'root'
})
export class VehicleService {
  private apiUrl = `${environment.apiUrl}/vehicles`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Vehicle[]> {
    return this.http.get<Vehicle[]>(this.apiUrl);
  }

  getActive(): Observable<Vehicle[]> {
    return this.http.get<Vehicle[]>(`${this.apiUrl}/active`);
  }

  getById(id: string): Observable<Vehicle> {
    return this.http.get<Vehicle>(`${this.apiUrl}/${id}`);
  }

  getByRegistration(registrationNumber: string): Observable<Vehicle> {
    return this.http.get<Vehicle>(`${this.apiUrl}/registration/${registrationNumber}`);
  }

  getByType(typeId: string): Observable<Vehicle[]> {
    return this.http.get<Vehicle[]>(`${this.apiUrl}/type/${typeId}`);
  }

  getByStatus(statusId: string): Observable<Vehicle[]> {
    return this.http.get<Vehicle[]>(`${this.apiUrl}/status/${statusId}`);
  }

  getByDriver(driverId: string): Observable<Vehicle[]> {
    return this.http.get<Vehicle[]>(`${this.apiUrl}/driver/${driverId}`);
  }

  getDueForMaintenance(): Observable<Vehicle[]> {
    return this.http.get<Vehicle[]>(`${this.apiUrl}/maintenance-due`);
  }

  create(vehicle: CreateVehicleDTO): Observable<Vehicle> {
    return this.http.post<Vehicle>(this.apiUrl, vehicle);
  }

  update(id: string, vehicle: UpdateVehicleDTO): Observable<Vehicle> {
    return this.http.put<Vehicle>(`${this.apiUrl}/${id}`, vehicle);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
