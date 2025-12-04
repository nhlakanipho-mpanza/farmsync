import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { FormsModule } from '@angular/forms';
import { PurchaseOrderService } from '../services/purchase-order.service';
import { PurchaseOrder, POStatus } from '../models/procurement.model';

@Component({
  selector: 'app-purchase-order-list',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatSelectModule,
    MatFormFieldModule,
    MatProgressSpinnerModule,
    MatSnackBarModule
  ],
  templateUrl: './purchase-order-list.component.html',
  styleUrls: ['./purchase-order-list.component.scss']
})
export class PurchaseOrderListComponent implements OnInit {
  purchaseOrders: PurchaseOrder[] = [];
  filteredPurchaseOrders: PurchaseOrder[] = [];
  loading = false;
  searchTerm = '';
  selectedStatus = 'All';
  statuses = ['All', 'Created', 'Approved', 'PartiallyReceived', 'FullyReceived', 'Closed'];

  constructor(
    private purchaseOrderService: PurchaseOrderService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.loadPurchaseOrders();
  }

  loadPurchaseOrders(): void {
    this.loading = true;
    this.purchaseOrderService.getAll().subscribe({
      next: (data) => {
        this.purchaseOrders = data;
        this.applyFilter();
        this.loading = false;
      },
      error: (error) => {
        this.snackBar.open('Error loading purchase orders', 'Close', { duration: 3000 });
        this.loading = false;
      }
    });
  }

  applyFilter(): void {
    let filtered = this.purchaseOrders;
    
    // Filter by status
    if (this.selectedStatus !== 'All') {
      filtered = filtered.filter(po => po.status === this.selectedStatus);
    }
    
    // Filter by search term
    if (this.searchTerm.trim()) {
      const term = this.searchTerm.toLowerCase();
      filtered = filtered.filter(po =>
        po.poNumber.toLowerCase().includes(term) ||
        po.supplierName.toLowerCase().includes(term) ||
        po.status.toLowerCase().includes(term)
      );
    }
    
    this.filteredPurchaseOrders = filtered;
  }

  onStatusChange(): void {
    this.applyFilter();
  }

  onSearch(): void {
    this.applyFilter();
  }

  createNew(): void {
    this.router.navigate(['/procurement/purchase-orders/new']);
  }

  editPurchaseOrder(id: string): void {
    this.router.navigate(['/procurement/purchase-orders/edit', id]);
  }

  viewDetails(id: string): void {
    this.router.navigate(['/procurement/purchase-orders/view', id]);
  }

  approvePurchaseOrder(id: string): void {
    if (confirm('Are you sure you want to approve this purchase order?')) {
      this.purchaseOrderService.approve(id).subscribe({
        next: () => {
          this.snackBar.open('Purchase order approved successfully', 'Close', { duration: 3000 });
          this.loadPurchaseOrders();
        },
        error: (error) => {
          this.snackBar.open(error.error?.message || 'Error approving purchase order', 'Close', { duration: 3000 });
        }
      });
    }
  }

  receiveGoods(id: string): void {
    this.router.navigate(['/procurement/receive-goods'], { queryParams: { poId: id } });
  }

  deletePurchaseOrder(id: string): void {
    if (confirm('Are you sure you want to delete this purchase order?')) {
      this.purchaseOrderService.delete(id).subscribe({
        next: () => {
          this.snackBar.open('Purchase order deleted successfully', 'Close', { duration: 3000 });
          this.loadPurchaseOrders();
        },
        error: (error) => {
          this.snackBar.open(error.error?.message || 'Error deleting purchase order', 'Close', { duration: 3000 });
        }
      });
    }
  }

  getStatusColor(status: string): string {
    switch (status) {
      case 'Created': return 'primary';
      case 'Approved': return 'accent';
      case 'PartiallyReceived': return 'warn';
      case 'FullyReceived': return 'success';
      case 'Closed': return 'default';
      default: return 'default';
    }
  }
}
