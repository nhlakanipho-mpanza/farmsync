import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTabsModule } from '@angular/material/tabs';
import { MatCardModule } from '@angular/material/card';
import { forkJoin } from 'rxjs';
import { PurchaseOrderService } from '../services/purchase-order.service';
import { GoodsReceivedService } from '../services/goods-received.service';
import { PurchaseOrder, GoodsReceived } from '../models/procurement.model';

@Component({
  selector: 'app-approval-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatSnackBarModule,
    MatTabsModule,
    MatCardModule
  ],
  templateUrl: './approval-dashboard.component.html',
  styleUrls: ['./approval-dashboard.component.scss']
})
export class ApprovalDashboardComponent implements OnInit {
  pendingPurchaseOrders: PurchaseOrder[] = [];
  pendingGoodsReceipts: GoodsReceived[] = [];
  loading = false;
  poDisplayedColumns: string[] = ['poNumber', 'supplier', 'orderDate', 'totalAmount', 'actions'];
  grDisplayedColumns: string[] = ['poNumber', 'supplier', 'receivedDate', 'hasDiscrepancies', 'actions'];

  constructor(
    private poService: PurchaseOrderService,
    private grService: GoodsReceivedService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.loadPendingApprovals();
  }

  loadPendingApprovals(): void {
    this.loading = true;
    forkJoin({
      purchaseOrders: this.poService.getPendingApprovals(),
      goodsReceipts: this.grService.getPendingApprovals()
    }).subscribe({
      next: (data) => {
        this.pendingPurchaseOrders = data.purchaseOrders;
        this.pendingGoodsReceipts = data.goodsReceipts;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading pending approvals:', error);
        this.snackBar.open('Failed to load pending approvals', 'Close', { duration: 3000 });
        this.loading = false;
      }
    });
  }

  approvePurchaseOrder(id: string): void {
    if (confirm('Are you sure you want to approve this purchase order?')) {
      this.poService.approve(id).subscribe({
        next: () => {
          this.snackBar.open('Purchase order approved successfully', 'Close', { duration: 3000 });
          this.loadPendingApprovals();
        },
        error: (error) => {
          console.error('Error approving purchase order:', error);
          this.snackBar.open('Failed to approve purchase order', 'Close', { duration: 3000 });
        }
      });
    }
  }

  viewPurchaseOrder(id: string): void {
    this.router.navigate(['/procurement/purchase-orders/view', id]);
  }

  viewGoodsReceipt(id: string): void {
    this.router.navigate(['/procurement/goods-received/view', id]);
  }

  approveGoodsReceipt(id: string): void {
    if (confirm('Are you sure you want to approve this goods receipt? This will update inventory.')) {
      this.grService.approve(id).subscribe({
        next: () => {
          this.snackBar.open('Goods receipt approved and inventory updated', 'Close', { duration: 3000 });
          this.loadPendingApprovals();
        },
        error: (error) => {
          console.error('Error approving goods receipt:', error);
          this.snackBar.open('Failed to approve goods receipt', 'Close', { duration: 3000 });
        }
      });
    }
  }

  rejectGoodsReceipt(id: string): void {
    const reason = prompt('Please enter the reason for rejection:');
    if (reason) {
      this.grService.reject(id, reason).subscribe({
        next: () => {
          this.snackBar.open('Goods receipt rejected', 'Close', { duration: 3000 });
          this.loadPendingApprovals();
        },
        error: (error) => {
          console.error('Error rejecting goods receipt:', error);
          this.snackBar.open('Failed to reject goods receipt', 'Close', { duration: 3000 });
        }
      });
    }
  }
}
