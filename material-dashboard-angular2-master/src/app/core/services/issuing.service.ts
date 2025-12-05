import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'environments/environment';
import {
  InventoryIssue,
  CreateInventoryIssueDTO,
  ApproveInventoryIssueDTO,
  ReturnInventoryIssueDTO,
  EquipmentIssue,
  CreateEquipmentIssueDTO,
  ReturnEquipmentIssueDTO
} from '../models/hr.model';

@Injectable({
  providedIn: 'root'
})
export class IssuingService {
  private apiUrl = `${environment.apiUrl}/issuing`;

  constructor(private http: HttpClient) {}

  // Inventory Issuing
  requestInventory(request: CreateInventoryIssueDTO): Observable<InventoryIssue> {
    return this.http.post<InventoryIssue>(`${this.apiUrl}/inventory/request`, request);
  }

  // Alias for form components
  requestInventoryIssue(request: CreateInventoryIssueDTO): Observable<InventoryIssue> {
    return this.requestInventory(request);
  }

  approveInventory(id: string, approval: ApproveInventoryIssueDTO): Observable<InventoryIssue> {
    return this.http.post<InventoryIssue>(`${this.apiUrl}/inventory/${id}/approve`, approval);
  }

  returnInventory(id: string, returnData: ReturnInventoryIssueDTO): Observable<InventoryIssue> {
    return this.http.post<InventoryIssue>(`${this.apiUrl}/inventory/${id}/return`, returnData);
  }

  getInventoryIssues(): Observable<InventoryIssue[]> {
    return this.http.get<InventoryIssue[]>(`${this.apiUrl}/inventory`);
  }

  getInventoryIssueById(id: string): Observable<InventoryIssue> {
    return this.http.get<InventoryIssue>(`${this.apiUrl}/inventory/${id}`);
  }

  getPendingInventoryApprovals(): Observable<InventoryIssue[]> {
    return this.http.get<InventoryIssue[]>(`${this.apiUrl}/inventory/pending`);
  }

  // Equipment Issuing
  requestEquipment(request: CreateEquipmentIssueDTO): Observable<EquipmentIssue> {
    return this.http.post<EquipmentIssue>(`${this.apiUrl}/equipment/request`, request);
  }

  // Alias for form components
  requestEquipmentIssue(request: CreateEquipmentIssueDTO): Observable<EquipmentIssue> {
    return this.requestEquipment(request);
  }

  approveEquipment(id: string, approval: ApproveInventoryIssueDTO): Observable<EquipmentIssue> {
    return this.http.post<EquipmentIssue>(`${this.apiUrl}/equipment/${id}/approve`, approval);
  }

  returnEquipment(id: string, returnData: ReturnEquipmentIssueDTO): Observable<EquipmentIssue> {
    return this.http.post<EquipmentIssue>(`${this.apiUrl}/equipment/${id}/return`, returnData);
  }

  getEquipmentIssues(): Observable<EquipmentIssue[]> {
    return this.http.get<EquipmentIssue[]>(`${this.apiUrl}/equipment`);
  }

  getEquipmentIssueById(id: string): Observable<EquipmentIssue> {
    return this.http.get<EquipmentIssue>(`${this.apiUrl}/equipment/${id}`);
  }

  getOverdueEquipment(): Observable<EquipmentIssue[]> {
    return this.http.get<EquipmentIssue[]>(`${this.apiUrl}/equipment/overdue`);
  }
}
