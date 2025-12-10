import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface ReportFilter {
  startDate?: string;
  endDate?: string;
  supplierId?: string;
  categoryId?: string;
  employeeId?: string;
  vehicleId?: string;
  departmentId?: string;
  status?: string;
  groupBy?: string;
}

export interface PurchaseOrderReport {
  totalOrders: number;
  totalAmount: number;
  pendingOrders: number;
  approvedOrders: number;
  completedOrders: number;
  orders: any[];
}

export interface InventoryValuation {
  totalInventoryValue: number;
  totalItems: number;
  lowStockItems: number;
  outOfStockItems: number;
  categoryBreakdown: any[];
  items: any[];
}

export interface SupplierTransactionSummary {
  supplierName: string;
  totalPurchases: number;
  totalPayments: number;
  currentBalance: number;
  transactionCount: number;
  transactions: any[];
}

export interface ExpenseSummary {
  totalExpenses: number;
  startDate: string;
  endDate: string;
  categoryBreakdown: any[];
  departmentBreakdown: any[];
  expenses: any[];
}

export interface GoodsReceivedSummary {
  totalGrns: number;
  totalValue: number;
  pendingInspections: number;
  approved: number;
  goodsReceived: any[];
}

@Injectable({
  providedIn: 'root'
})
export class ReportsService {
  private apiUrl = `${environment.apiUrl}/reports`;

  constructor(private http: HttpClient) { }

  getPurchaseOrderReport(filter: ReportFilter): Observable<PurchaseOrderReport> {
    return this.http.post<PurchaseOrderReport>(`${this.apiUrl}/purchase-orders`, filter);
  }

  exportPurchaseOrderReport(filter: ReportFilter, format: 'Pdf' | 'Excel'): Observable<Blob> {
    const params = new HttpParams().set('format', format);
    return this.http.post(`${this.apiUrl}/purchase-orders/export`, filter, {
      params,
      responseType: 'blob'
    });
  }

  getGoodsReceivedReport(filter: ReportFilter): Observable<GoodsReceivedSummary> {
    return this.http.post<GoodsReceivedSummary>(`${this.apiUrl}/goods-received`, filter);
  }

  exportGoodsReceivedReport(filter: ReportFilter, format: 'Pdf' | 'Excel'): Observable<Blob> {
    const params = new HttpParams().set('format', format);
    return this.http.post(`${this.apiUrl}/goods-received/export`, filter, {
      params,
      responseType: 'blob'
    });
  }

  getInventoryValuationReport(filter: ReportFilter): Observable<InventoryValuation> {
    return this.http.post<InventoryValuation>(`${this.apiUrl}/inventory-valuation`, filter);
  }

  exportInventoryValuationReport(filter: ReportFilter, format: 'Pdf' | 'Excel'): Observable<Blob> {
    const params = new HttpParams().set('format', format);
    return this.http.post(`${this.apiUrl}/inventory-valuation/export`, filter, {
      params,
      responseType: 'blob'
    });
  }

  getSupplierTransactionReport(filter: ReportFilter): Observable<SupplierTransactionSummary> {
    return this.http.post<SupplierTransactionSummary>(`${this.apiUrl}/supplier-transactions`, filter);
  }

  exportSupplierTransactionReport(filter: ReportFilter, format: 'Pdf' | 'Excel'): Observable<Blob> {
    const params = new HttpParams().set('format', format);
    return this.http.post(`${this.apiUrl}/supplier-transactions/export`, filter, {
      params,
      responseType: 'blob'
    });
  }

  getExpenseReport(filter: ReportFilter): Observable<ExpenseSummary> {
    return this.http.post<ExpenseSummary>(`${this.apiUrl}/expenses`, filter);
  }

  exportExpenseReport(filter: ReportFilter, format: 'Pdf' | 'Excel'): Observable<Blob> {
    const params = new HttpParams().set('format', format);
    return this.http.post(`${this.apiUrl}/expenses/export`, filter, {
      params,
      responseType: 'blob'
    });
  }

  private downloadFile(blob: Blob, filename: string) {
    const url = window.URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.download = filename;
    link.click();
    window.URL.revokeObjectURL(url);
  }
}
