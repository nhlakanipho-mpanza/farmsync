import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GoodsReceived, CreateGoodsReceivedDto } from '../models/procurement.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class GoodsReceivedService {
  private apiUrl = `${environment.apiUrl}/GoodsReceived`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<GoodsReceived[]> {
    return this.http.get<GoodsReceived[]>(this.apiUrl);
  }

  getById(id: string): Observable<GoodsReceived> {
    return this.http.get<GoodsReceived>(`${this.apiUrl}/${id}`);
  }

  getByPurchaseOrder(purchaseOrderId: string): Observable<GoodsReceived[]> {
    return this.http.get<GoodsReceived[]>(`${this.apiUrl}/purchase-order/${purchaseOrderId}`);
  }

  getPendingApprovals(): Observable<GoodsReceived[]> {
    return this.http.get<GoodsReceived[]>(`${this.apiUrl}/pending-approvals`);
  }

  create(dto: CreateGoodsReceivedDto): Observable<GoodsReceived> {
    return this.http.post<GoodsReceived>(this.apiUrl, dto);
  }

  approve(id: string): Observable<GoodsReceived> {
    return this.http.post<GoodsReceived>(`${this.apiUrl}/${id}/approve`, {});
  }

  reject(id: string, reason: string): Observable<GoodsReceived> {
    return this.http.post<GoodsReceived>(`${this.apiUrl}/${id}/reject`, JSON.stringify(reason), {
      headers: { 'Content-Type': 'application/json' }
    });
  }
}
