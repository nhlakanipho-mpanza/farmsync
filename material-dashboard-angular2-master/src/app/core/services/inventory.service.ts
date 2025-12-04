import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  InventoryItem,
  CreateInventoryItemDto,
  UpdateInventoryItemDto,
  InventoryStats
} from '../models/inventory.model';

@Injectable({
  providedIn: 'root'
})
export class InventoryService {
  private apiUrl = `${environment.apiUrl}/InventoryItems`;

  constructor(private http: HttpClient) {}

  getAll(params?: { category?: string; search?: string }): Observable<InventoryItem[]> {
    let httpParams = new HttpParams();
    if (params?.category) {
      httpParams = httpParams.set('category', params.category);
    }
    if (params?.search) {
      httpParams = httpParams.set('search', params.search);
    }
    return this.http.get<InventoryItem[]>(this.apiUrl, { params: httpParams });
  }

  getById(id: string): Observable<InventoryItem> {
    return this.http.get<InventoryItem>(`${this.apiUrl}/${id}`);
  }

  create(item: CreateInventoryItemDto): Observable<InventoryItem> {
    return this.http.post<InventoryItem>(this.apiUrl, item);
  }

  update(id: string, item: UpdateInventoryItemDto): Observable<InventoryItem> {
    return this.http.put<InventoryItem>(`${this.apiUrl}/${id}`, item);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  getStats(): Observable<InventoryStats> {
    return this.http.get<InventoryStats>(`${this.apiUrl}/stats`);
  }

  getLowStockItems(): Observable<InventoryItem[]> {
    return this.http.get<InventoryItem[]>(`${this.apiUrl}/low-stock`);
  }
}
