import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTableModule } from '@angular/material/table';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { PurchaseOrderService } from '../services/purchase-order.service';
import { PurchaseOrder } from '../models/procurement.model';

@Component({
  selector: 'app-purchase-order-view',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatTableModule,
    MatSnackBarModule
  ],
  templateUrl: './purchase-order-view.component.html',
  styleUrls: ['./purchase-order-view.component.scss']
})
export class PurchaseOrderViewComponent implements OnInit {
  purchaseOrder: PurchaseOrder | null = null;
  loading = false;
  displayedColumns = ['itemName', 'description', 'quantity', 'unitPrice', 'total'];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private purchaseOrderService: PurchaseOrderService,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.params['id'];
    if (id) {
      this.loadPurchaseOrder(id);
    }
  }

  loadPurchaseOrder(id: string): void {
    this.loading = true;
    this.purchaseOrderService.getById(id).subscribe({
      next: (data) => {
        this.purchaseOrder = data;
        this.loading = false;
      },
      error: (error) => {
        this.snackBar.open('Error loading purchase order', 'Close', { duration: 3000 });
        this.loading = false;
        this.router.navigate(['/procurement/purchase-orders']);
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/procurement/purchase-orders']);
  }

  editPurchaseOrder(): void {
    if (this.purchaseOrder) {
      this.router.navigate(['/procurement/purchase-orders/edit', this.purchaseOrder.id]);
    }
  }

  receiveGoods(): void {
    if (this.purchaseOrder) {
      this.router.navigate(['/procurement/receive-goods'], { 
        queryParams: { poId: this.purchaseOrder.id } 
      });
    }
  }

  getStatusColor(status: string): string {
    const statusColors: { [key: string]: string } = {
      'Created': 'info',
      'Approved': 'success',
      'PartiallyReceived': 'warning',
      'FullyReceived': 'primary',
      'Closed': 'secondary',
      'ClosedWithIssues': 'danger',
      'Cancelled': 'dark'
    };
    return statusColors[status] || 'secondary';
  }
}
