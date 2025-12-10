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

export interface ReferenceDto {
  id: string;
  name: string;
  description?: string;
  isActive: boolean;
  rate?: number; // For positions
  isDriverPosition?: boolean; // For positions
  createdAt?: Date;
  updatedAt?: Date;
}

export interface CreateReferenceDto {
  name: string;
  description?: string;
  isActive?: boolean;
}

export interface UpdateReferenceDto {
  name: string;
  description?: string;
  isActive: boolean;
}

export interface ReplaceAndDeleteDto {
  sourceId: string;
  targetId: string;
}

export interface UsageInfo {
  id: string;
  name: string;
  usageCount: number;
  usedInTables: string[];
  canDelete: boolean;
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
  private apiBase = `${environment.apiUrl}/referencedata`;

  constructor(private http: HttpClient) {}

  // ========== Inventory/General Reference Data ==========

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

  // ========== Legacy GET Methods (Kept for backward compatibility) ==========
  // These are still used by other components. Can be removed later if all components use getTableData()

  getPositions(): Observable<Position[]> {
    return this.http.get<Position[]>(`${this.apiBase}/positions`);
  }

  getEmploymentTypes(): Observable<EmploymentType[]> {
    return this.http.get<EmploymentType[]>(`${this.apiBase}/employment-types`);
  }

  getRoles(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiBase}/roles`);
  }

  getTeamTypes(): Observable<TeamType[]> {
    return this.http.get<TeamType[]>(`${this.apiBase}/team-types`);
  }

  getBankNames(): Observable<BankName[]> {
    return this.http.get<BankName[]>(`${this.apiBase}/bank-names`);
  }

  getAccountTypes(): Observable<AccountType[]> {
    return this.http.get<AccountType[]>(`${this.apiBase}/account-types`);
  }

  getTaskStatuses(): Observable<TaskStatus[]> {
    return this.http.get<TaskStatus[]>(`${this.apiBase}/task-statuses`);
  }

  getIssueStatuses(): Observable<IssueStatus[]> {
    return this.http.get<IssueStatus[]>(`${this.apiBase}/issue-statuses`);
  }

  getWorkAreas(): Observable<WorkArea[]> {
    return this.http.get<WorkArea[]>(`${this.apiBase}/work-areas`);
  }

  getVehicleTypes(): Observable<ReferenceItem[]> {
    return this.http.get<ReferenceItem[]>(`${this.apiBase}/vehicle-types`);
  }

  getDocumentTypes(): Observable<ReferenceDto[]> {
    return this.http.get<ReferenceDto[]>(`${this.apiBase}/document-types`);
  }

  getLeaveTypes(): Observable<ReferenceDto[]> {
    return this.http.get<ReferenceDto[]>(`${this.apiBase}/leave-types`);
  }

  getFieldPhases(): Observable<ReferenceDto[]> {
    return this.http.get<ReferenceDto[]>(`${this.apiBase}/field-phases`);
  }

  getVehicleStatuses(): Observable<ReferenceItem[]> {
    return this.http.get<ReferenceItem[]>(`${this.apiBase}/vehicle-statuses`);
  }

  getFuelTypes(): Observable<ReferenceItem[]> {
    return this.http.get<ReferenceItem[]>(`${this.apiBase}/fuel-types`);
  }

  getFleetMaintenanceTypes(): Observable<ReferenceItem[]> {
    return this.http.get<ReferenceItem[]>(`${this.apiBase}/fleet-maintenance-types`);
  }

  getFleetTaskStatuses(): Observable<ReferenceItem[]> {
    return this.http.get<ReferenceItem[]>(`${this.apiBase}/fleet-task-statuses`);
  }

  // ========== Generic CRUD Methods (New Configuration-Based Approach) ==========

  /**
   * GET all records for a table
   */
  getTableData(tableKey: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiBase}/${tableKey}`);
  }

  /**
   * CREATE a new record
   */
  create(tableKey: string, payload: any): Observable<any> {
    return this.http.post<any>(`${this.apiBase}/${tableKey}`, payload);
  }

  /**
   * UPDATE an existing record
   */
  update(tableKey: string, id: string, payload: any): Observable<any> {
    return this.http.put<any>(`${this.apiBase}/${tableKey}/${id}`, payload);
  }

  /**
   * DELETE a record
   */
  delete(tableKey: string, id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiBase}/${tableKey}/${id}`);
  }

  /**
   * GET usage information for a record
   */
  getUsage(tableKey: string, id: string): Observable<UsageInfo> {
    return this.http.get<UsageInfo>(`${this.apiBase}/${tableKey}/${id}/usage`);
  }

  /**
   * REPLACE and DELETE - migrate references then delete
   */
  replaceAndDelete(tableKey: string, sourceId: string, targetId: string): Observable<{ success: boolean; message: string }> {
    return this.http.post<{ success: boolean; message: string }>(
      `${this.apiBase}/${tableKey}/replace-and-delete`,
      { sourceId, targetId }
    );
  }

  // ========== Utility Methods ==========

  /**
   * Generic method to check if an item can be safely deleted
   */
  canDelete(usageCount: number): boolean {
    return usageCount === 0;
  }

  /**
   * Get a confirmation message for deletion
   */
  getDeleteConfirmMessage(name: string, usageCount: number): string {
    if (usageCount === 0) {
      return `Are you sure you want to delete "${name}"?`;
    }
    return `"${name}" is currently used by ${usageCount} record(s). Please select a replacement before deleting.`;
  }
}
