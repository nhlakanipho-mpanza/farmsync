import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { IssuingService } from 'app/core/services/issuing.service';
import { InventoryIssue, EquipmentIssue } from 'app/core/models/hr.model';

@Component({
  selector: 'app-issuing-dashboard',
  templateUrl: './issuing-dashboard.component.html',
  styleUrls: ['./issuing-dashboard.component.css']
})
export class IssuingDashboardComponent implements OnInit {
  inventoryIssues: InventoryIssue[] = [];
  equipmentIssues: EquipmentIssue[] = [];
  pendingInventory: InventoryIssue[] = [];
  overdueEquipment: EquipmentIssue[] = [];
  loading = false;
  viewMode: 'inventory' | 'equipment' | 'pending' | 'overdue' = 'inventory';

  constructor(
    private issuingService: IssuingService,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit() {
    this.loadAllData();
  }

  loadAllData() {
    this.loadInventoryIssues();
    this.loadEquipmentIssues();
    this.loadPendingApprovals();
    this.loadOverdueEquipment();
  }

  loadInventoryIssues() {
    this.loading = true;
    this.issuingService.getInventoryIssues().subscribe({
      next: (data) => {
        this.inventoryIssues = data;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading inventory issues:', error);
        this.loading = false;
      }
    });
  }

  loadEquipmentIssues() {
    this.loading = true;
    this.issuingService.getEquipmentIssues().subscribe({
      next: (data) => {
        this.equipmentIssues = data;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading equipment issues:', error);
        this.loading = false;
      }
    });
  }

  loadPendingApprovals() {
    this.issuingService.getPendingInventoryApprovals().subscribe({
      next: (data) => this.pendingInventory = data,
      error: (error) => console.error('Error loading pending approvals:', error)
    });
  }

  loadOverdueEquipment() {
    this.issuingService.getOverdueEquipment().subscribe({
      next: (data) => this.overdueEquipment = data,
      error: (error) => console.error('Error loading overdue equipment:', error)
    });
  }

  approveInventoryIssue(id: string) {
    this.issuingService.approveInventory(id, { approve: true }).subscribe({
      next: () => {
        this.snackBar.open('Inventory issue approved', 'Close', { duration: 3000 });
        this.loadAllData();
      },
      error: (error) => {
        console.error('Error approving issue:', error);
        this.snackBar.open('Error approving issue', 'Close', { duration: 3000 });
      }
    });
  }

  rejectInventoryIssue(id: string) {
    this.issuingService.approveInventory(id, { approve: false, notes: 'Rejected' }).subscribe({
      next: () => {
        this.snackBar.open('Inventory issue rejected', 'Close', { duration: 3000 });
        this.loadAllData();
      },
      error: (error) => {
        console.error('Error rejecting issue:', error);
        this.snackBar.open('Error rejecting issue', 'Close', { duration: 3000 });
      }
    });
  }

  approveEquipmentIssue(id: string) {
    this.issuingService.approveEquipment(id, { approve: true }).subscribe({
      next: () => {
        this.snackBar.open('Equipment issue approved', 'Close', { duration: 3000 });
        this.loadAllData();
      },
      error: (error) => {
        console.error('Error approving equipment:', error);
        this.snackBar.open('Error approving equipment', 'Close', { duration: 3000 });
      }
    });
  }

  getStatusClass(statusName: string): string {
    switch(statusName?.toLowerCase()) {
      case 'pending': return 'badge-warning';
      case 'approved': return 'badge-info';
      case 'issued': return 'badge-success';
      case 'returned': return 'badge-secondary';
      case 'cancelled': return 'badge-danger';
      default: return 'badge-secondary';
    }
  }

  calculateOverdueDays(expectedReturnDate: string): number {
    const expected = new Date(expectedReturnDate);
    const today = new Date();
    const diffTime = Math.abs(today.getTime() - expected.getTime());
    const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));
    return diffDays;
  }
}
