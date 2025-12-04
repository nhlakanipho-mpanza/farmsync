import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { 
  PurchaseOrder, 
  CreatePurchaseOrderDto, 
  UpdatePurchaseOrderDto 
} from '../models/procurement.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PurchaseOrderService {
  private apiUrl = `${environment.apiUrl}/PurchaseOrders`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<PurchaseOrder[]> {
    return this.http.get<PurchaseOrder[]>(this.apiUrl);
  }

  getById(id: string): Observable<PurchaseOrder> {
    return this.http.get<PurchaseOrder>(`${this.apiUrl}/${id}`);
  }

  getByStatus(status: string): Observable<PurchaseOrder[]> {
    return this.http.get<PurchaseOrder[]>(`${this.apiUrl}/status/${status}`);
  }

  getPendingApprovals(): Observable<PurchaseOrder[]> {
    return this.http.get<PurchaseOrder[]>(`${this.apiUrl}/pending-approvals`);
  }

  getAvailableForReceiving(): Observable<PurchaseOrder[]> {
    return this.http.get<PurchaseOrder[]>(`${this.apiUrl}/available-for-receiving`);
  }

  create(dto: CreatePurchaseOrderDto): Observable<PurchaseOrder> {
    return this.http.post<PurchaseOrder>(this.apiUrl, dto);
  }

  update(id: string, dto: UpdatePurchaseOrderDto): Observable<PurchaseOrder> {
    return this.http.put<PurchaseOrder>(`${this.apiUrl}/${id}`, dto);
  }

  approve(id: string): Observable<PurchaseOrder> {
    return this.http.post<PurchaseOrder>(`${this.apiUrl}/${id}/approve`, {});
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
