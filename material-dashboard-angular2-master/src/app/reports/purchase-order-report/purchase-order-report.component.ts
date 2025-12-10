import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ReportsService, PurchaseOrderReport, ReportFilter } from '../reports.service';

@Component({
  selector: 'app-purchase-order-report',
  templateUrl: './purchase-order-report.component.html',
  styleUrls: ['./purchase-order-report.component.css']
})
export class PurchaseOrderReportComponent implements OnInit {
  filterForm: FormGroup;
  report: PurchaseOrderReport | null = null;
  loading = false;
  error: string | null = null;

  displayedColumns: string[] = ['poNumber', 'orderDate', 'supplierName', 'status', 'totalAmount'];

  constructor(
    private fb: FormBuilder,
    private reportsService: ReportsService
  ) {
    this.filterForm = this.fb.group({
      startDate: [''],
      endDate: [''],
      supplierId: [''],
      status: ['']
    });
  }

  ngOnInit(): void {
    this.loadReport();
  }

  loadReport(): void {
    this.loading = true;
    this.error = null;

    const filter: ReportFilter = {
      ...this.filterForm.value,
      startDate: this.filterForm.value.startDate ? 
        new Date(this.filterForm.value.startDate).toISOString() : undefined,
      endDate: this.filterForm.value.endDate ? 
        new Date(this.filterForm.value.endDate).toISOString() : undefined
    };

    // Remove undefined values
    Object.keys(filter).forEach(key => {
      if (filter[key] === undefined || filter[key] === '') {
        delete filter[key];
      }
    });

    this.reportsService.getPurchaseOrderReport(filter).subscribe({
      next: (report) => {
        this.report = report;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load report';
        this.loading = false;
        console.error(err);
      }
    });
  }

  exportPdf(): void {
    const filter: ReportFilter = {
      ...this.filterForm.value,
      startDate: this.filterForm.value.startDate ? 
        new Date(this.filterForm.value.startDate).toISOString() : undefined,
      endDate: this.filterForm.value.endDate ? 
        new Date(this.filterForm.value.endDate).toISOString() : undefined
    };

    // Remove undefined values
    Object.keys(filter).forEach(key => {
      if (filter[key] === undefined || filter[key] === '') {
        delete filter[key];
      }
    });

    this.reportsService.exportPurchaseOrderReport(filter, 'Pdf').subscribe({
      next: (blob) => {
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.download = `PurchaseOrders_${new Date().getTime()}.pdf`;
        link.click();
        window.URL.revokeObjectURL(url);
      },
      error: (err) => {
        console.error('Export failed', err);
      }
    });
  }

  exportExcel(): void {
    const filter: ReportFilter = {
      ...this.filterForm.value,
      startDate: this.filterForm.value.startDate ? 
        new Date(this.filterForm.value.startDate).toISOString() : undefined,
      endDate: this.filterForm.value.endDate ? 
        new Date(this.filterForm.value.endDate).toISOString() : undefined
    };

    // Remove undefined values
    Object.keys(filter).forEach(key => {
      if (filter[key] === undefined || filter[key] === '') {
        delete filter[key];
      }
    });

    this.reportsService.exportPurchaseOrderReport(filter, 'Excel').subscribe({
      next: (blob) => {
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.download = `PurchaseOrders_${new Date().getTime()}.xlsx`;
        link.click();
        window.URL.revokeObjectURL(url);
      },
      error: (err) => {
        console.error('Export failed', err);
      }
    });
  }

  resetFilters(): void {
    this.filterForm.reset();
    this.loadReport();
  }
}
