import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'environments/environment';
import { FuelLog, CreateFuelLogDTO, UpdateFuelLogDTO } from '../models/fleet.model';

@Injectable({
  providedIn: 'root'
})
export class FuelService {
  private apiUrl = `${environment.apiUrl}/fuel`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<FuelLog[]> {
    return this.http.get<FuelLog[]>(this.apiUrl);
  }

  getById(id: string): Observable<FuelLog> {
    return this.http.get<FuelLog>(`${this.apiUrl}/${id}`);
  }

  getByVehicle(vehicleId: string): Observable<FuelLog[]> {
    return this.http.get<FuelLog[]>(`${this.apiUrl}/vehicle/${vehicleId}`);
  }

  getRecent(count: number = 10): Observable<FuelLog[]> {
    return this.http.get<FuelLog[]>(`${this.apiUrl}/recent?count=${count}`);
  }

  getByDateRange(startDate: string, endDate: string): Observable<FuelLog[]> {
    return this.http.get<FuelLog[]>(
      `${this.apiUrl}/date-range?startDate=${startDate}&endDate=${endDate}`
    );
  }

  getAverageFuelEfficiency(vehicleId: string): Observable<{ vehicleId: string, averageFuelEfficiency: number }> {
    return this.http.get<{ vehicleId: string, averageFuelEfficiency: number }>(
      `${this.apiUrl}/vehicle/${vehicleId}/efficiency`
    );
  }

  create(fuelLog: CreateFuelLogDTO): Observable<FuelLog> {
    return this.http.post<FuelLog>(this.apiUrl, fuelLog);
  }

  update(id: string, fuelLog: UpdateFuelLogDTO): Observable<FuelLog> {
    return this.http.put<FuelLog>(`${this.apiUrl}/${id}`, fuelLog);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
