import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTableModule } from '@angular/material/table';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FormsModule } from '@angular/forms';
import { GoodsReceivedService } from '../services/goods-received.service';
import { GoodsReceived } from '../models/procurement.model';

@Component({
  selector: 'app-goods-received-view',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatTableModule,
    MatSnackBarModule,
    MatFormFieldModule,
    MatInputModule
  ],
  templateUrl: './goods-received-view.component.html',
  styleUrls: ['./goods-received-view.component.scss']
})
export class GoodsReceivedViewComponent implements OnInit {
  goodsReceived: GoodsReceived | null = null;
  loading = false;
  approving = false;
  rejecting = false;
  displayedColumns = ['itemName', 'ordered', 'received', 'damaged', 'shortfall', 'condition', 'notes'];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private goodsReceivedService: GoodsReceivedService,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.params['id'];
    if (id) {
      this.loadGoodsReceived(id);
    }
  }

  loadGoodsReceived(id: string): void {
    this.loading = true;
    this.goodsReceivedService.getById(id).subscribe({
      next: (data) => {
        this.goodsReceived = data;
        this.loading = false;
      },
      error: (error) => {
        this.snackBar.open('Error loading goods received record', 'Close', { duration: 3000 });
        this.loading = false;
        this.router.navigate(['/procurement/goods-received']);
      }
    });
  }

  approveReceipt(): void {
    if (!this.goodsReceived || !confirm('Are you sure you want to approve this goods receipt?')) {
      return;
    }

    this.approving = true;
    this.goodsReceivedService.approve(this.goodsReceived.id).subscribe({
      next: () => {
        this.snackBar.open('Goods receipt approved successfully', 'Close', { duration: 3000 });
        this.loadGoodsReceived(this.goodsReceived!.id);
        this.approving = false;
      },
      error: (error) => {
        this.snackBar.open(error.error?.message || 'Error approving goods receipt', 'Close', { duration: 3000 });
        this.approving = false;
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/procurement/goods-received']);
  }

  getStatusColor(status: string): string {
    const statusColors: { [key: string]: string } = {
      'Pending': 'warning',
      'Approved': 'success',
      'Rejected': 'danger'
    };
    return statusColors[status] || 'secondary';
  }
}
