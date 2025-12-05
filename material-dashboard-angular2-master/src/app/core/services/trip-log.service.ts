import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'environments/environment';
import { TripLog, CreateTripLogDTO, UpdateTripLogDTO } from '../models/fleet.model';

@Injectable({
  providedIn: 'root'
})
export class TripLogService {
  private apiUrl = `${environment.apiUrl}/triplogs`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<TripLog[]> {
    return this.http.get<TripLog[]>(this.apiUrl);
  }

  getById(id: string): Observable<TripLog> {
    return this.http.get<TripLog>(`${this.apiUrl}/${id}`);
  }

  getByVehicle(vehicleId: string): Observable<TripLog[]> {
    return this.http.get<TripLog[]>(`${this.apiUrl}/vehicle/${vehicleId}`);
  }

  getByDriver(driverId: string): Observable<TripLog[]> {
    return this.http.get<TripLog[]>(`${this.apiUrl}/driver/${driverId}`);
  }

  getActive(): Observable<TripLog[]> {
    return this.http.get<TripLog[]>(`${this.apiUrl}/active`);
  }

  getByDateRange(startDate: string, endDate: string): Observable<TripLog[]> {
    return this.http.get<TripLog[]>(
      `${this.apiUrl}/date-range?startDate=${startDate}&endDate=${endDate}`
    );
  }

  create(tripLog: CreateTripLogDTO): Observable<TripLog> {
    return this.http.post<TripLog>(this.apiUrl, tripLog);
  }

  update(id: string, tripLog: UpdateTripLogDTO): Observable<TripLog> {
    return this.http.put<TripLog>(`${this.apiUrl}/${id}`, tripLog);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
