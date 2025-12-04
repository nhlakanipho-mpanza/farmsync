import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

export interface ReferenceItem {
  id: string;
  name: string;
  description?: string;
}

export interface UnitOfMeasure extends ReferenceItem {
  abbreviation: string;
}

export interface Location extends ReferenceItem {
  address?: string;
}

export interface AllReferenceData {
  categories: ReferenceItem[];
  types: ReferenceItem[];
  unitsOfMeasure: UnitOfMeasure[];
  transactionStatuses: ReferenceItem[];
  equipmentConditions: ReferenceItem[];
  maintenanceTypes: ReferenceItem[];
  locations: Location[];
}

@Injectable({
  providedIn: 'root'
})
export class ReferenceDataService {
  private apiUrl = `${environment.apiUrl}/ReferenceData`;

  constructor(private http: HttpClient) {}

  getCategories(): Observable<ReferenceItem[]> {
    return this.http.get<ReferenceItem[]>(`${this.apiUrl}/categories`);
  }

  getTypes(): Observable<ReferenceItem[]> {
    return this.http.get<ReferenceItem[]>(`${this.apiUrl}/types`);
  }

  getUnitsOfMeasure(): Observable<UnitOfMeasure[]> {
    return this.http.get<UnitOfMeasure[]>(`${this.apiUrl}/units`);
  }

  getLocations(): Observable<Location[]> {
    return this.http.get<Location[]>(`${this.apiUrl}/locations`);
  }

  getAllReferenceData(): Observable<AllReferenceData> {
    return this.http.get<AllReferenceData>(`${this.apiUrl}/all`);
  }
}
