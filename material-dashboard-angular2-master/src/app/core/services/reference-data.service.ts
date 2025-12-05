import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  Position,
  EmploymentType,
  RoleType,
  TeamType,
  BankName,
  AccountType,
  TaskStatus,
  IssueStatus,
  WorkArea
} from '../models/hr.model';

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
  private hrApiUrl = `${environment.apiUrl}/referencedata`;

  constructor(private http: HttpClient) {}

  // Inventory/Procurement Reference Data
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

  // HR Reference Data
  getPositions(): Observable<Position[]> {
    return this.http.get<Position[]>(`${this.hrApiUrl}/positions`);
  }

  getEmploymentTypes(): Observable<EmploymentType[]> {
    return this.http.get<EmploymentType[]>(`${this.hrApiUrl}/employment-types`);
  }

  getRoleTypes(): Observable<RoleType[]> {
    return this.http.get<RoleType[]>(`${this.hrApiUrl}/role-types`);
  }

  getTeamTypes(): Observable<TeamType[]> {
    return this.http.get<TeamType[]>(`${this.hrApiUrl}/team-types`);
  }

  getBankNames(): Observable<BankName[]> {
    return this.http.get<BankName[]>(`${this.hrApiUrl}/bank-names`);
  }

  getAccountTypes(): Observable<AccountType[]> {
    return this.http.get<AccountType[]>(`${this.hrApiUrl}/account-types`);
  }

  getTaskStatuses(): Observable<TaskStatus[]> {
    return this.http.get<TaskStatus[]>(`${this.hrApiUrl}/task-statuses`);
  }

  getIssueStatuses(): Observable<IssueStatus[]> {
    return this.http.get<IssueStatus[]>(`${this.hrApiUrl}/issue-statuses`);
  }

  getWorkAreas(): Observable<WorkArea[]> {
    return this.http.get<WorkArea[]>(`${this.hrApiUrl}/work-areas`);
  }

  // Fleet Reference Data
  getVehicleTypes(): Observable<ReferenceItem[]> {
    return this.http.get<ReferenceItem[]>(`${this.hrApiUrl}/vehicle-types`);
  }

  getVehicleStatuses(): Observable<ReferenceItem[]> {
    return this.http.get<ReferenceItem[]>(`${this.hrApiUrl}/vehicle-statuses`);
  }

  getFuelTypes(): Observable<ReferenceItem[]> {
    return this.http.get<ReferenceItem[]>(`${this.hrApiUrl}/fuel-types`);
  }

  getFleetMaintenanceTypes(): Observable<ReferenceItem[]> {
    return this.http.get<ReferenceItem[]>(`${this.hrApiUrl}/fleet-maintenance-types`);
  }

  getFleetTaskStatuses(): Observable<ReferenceItem[]> {
    return this.http.get<ReferenceItem[]>(`${this.hrApiUrl}/fleet-task-statuses`);
  }
}
