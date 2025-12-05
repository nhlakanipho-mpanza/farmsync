import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'environments/environment';
import { GPSLocation } from '../models/fleet.model';

@Injectable({
  providedIn: 'root'
})
export class GPSTrackingService {
  private apiUrl = `${environment.apiUrl}/gps`;

  constructor(private http: HttpClient) {}

  getActiveVehicleLocations(): Observable<GPSLocation[]> {
    return this.http.get<GPSLocation[]>(`${this.apiUrl}/active-vehicles`);
  }

  getLatestLocation(vehicleId: string): Observable<GPSLocation> {
    return this.http.get<GPSLocation>(`${this.apiUrl}/vehicle/${vehicleId}/latest`);
  }

  getLocationHistory(vehicleId: string, startDate: string, endDate: string): Observable<GPSLocation[]> {
    return this.http.get<GPSLocation[]>(
      `${this.apiUrl}/vehicle/${vehicleId}/history?startDate=${startDate}&endDate=${endDate}`
    );
  }

  recordLocation(location: any): Observable<GPSLocation> {
    return this.http.post<GPSLocation>(this.apiUrl, location);
  }
}
