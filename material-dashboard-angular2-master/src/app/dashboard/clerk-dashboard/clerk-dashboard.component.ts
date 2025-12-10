import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-clerk-dashboard',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatIconModule, MatButtonModule, RouterModule],
  template: `
    <div class="main-content">
      <div class="container-fluid">
        <div class="row">
          <div class="col-lg-12">
            <h2 class="title">Operations Clerk Dashboard</h2>
            <p class="category">Stock & Receiving Operations</p>
          </div>
        </div>

        <div class="row">
          <div class="col-lg-3 col-md-6">
            <div class="card card-stats">
              <div class="card-header card-header-info card-header-icon">
                <div class="card-icon"><mat-icon>local_shipping</mat-icon></div>
                <p class="card-category">Today's Deliveries</p>
                <h3 class="card-title">{{ stats.deliveriesToday }}</h3>
              </div>
              <div class="card-footer">
                <button mat-button color="primary" [routerLink]="['/procurement/receive-goods']">Receive</button>
              </div>
            </div>
          </div>
          <div class="col-lg-3 col-md-6">
            <div class="card card-stats">
              <div class="card-header card-header-warning card-header-icon">
                <div class="card-icon"><mat-icon>pending</mat-icon></div>
                <p class="card-category">Pending GRNs</p>
                <h3 class="card-title">{{ stats.pendingGRNs }}</h3>
              </div>
            </div>
          </div>
          <div class="col-lg-3 col-md-6">
            <div class="card card-stats">
              <div class="card-header card-header-success card-header-icon">
                <div class="card-icon"><mat-icon>inventory</mat-icon></div>
                <p class="card-category">Items in Stock</p>
                <h3 class="card-title">{{ stats.itemsInStock }}</h3>
              </div>
            </div>
          </div>
          <div class="col-lg-3 col-md-6">
            <div class="card card-stats">
              <div class="card-header card-header-danger card-header-icon">
                <div class="card-icon"><mat-icon>warning</mat-icon></div>
                <p class="card-category">Issue Requests</p>
                <h3 class="card-title">{{ stats.issueRequests }}</h3>
              </div>
            </div>
          </div>
        </div>

        <div class="row">
          <div class="col-md-8">
            <div class="card">
              <div class="card-header card-header-primary">
                <h4 class="card-title">Today's Receiving Schedule</h4>
              </div>
              <div class="card-body">
                <div class="table-responsive">
                  <table class="table">
                    <thead><tr><th>PO Number</th><th>Supplier</th><th>Items</th><th>Action</th></tr></thead>
                    <tbody>
                      <tr *ngFor="let delivery of deliveries">
                        <td>{{ delivery.poNumber }}</td>
                        <td>{{ delivery.supplier }}</td>
                        <td>{{ delivery.itemCount }}</td>
                        <td><button mat-icon-button [routerLink]="['/procurement/receive-goods']"><mat-icon>check_circle</mat-icon></button></td>
                      </tr>
                    </tbody>
                  </table>
                </div>
              </div>
            </div>
          </div>
          <div class="col-md-4">
            <div class="card">
              <div class="card-header card-header-success">
                <h4 class="card-title">Quick Actions</h4>
              </div>
              <div class="card-body">
                <button mat-raised-button color="primary" class="mb-2 w-100" [routerLink]="['/procurement/receive-goods']">
                  <mat-icon>add</mat-icon> Receive Goods
                </button>
                <button mat-raised-button class="mb-2 w-100" [routerLink]="['/hr/issuing']">
                  <mat-icon>output</mat-icon> Issue Equipment
                </button>
                <button mat-raised-button class="mb-2 w-100" [routerLink]="['/inventory']">
                  <mat-icon>inventory_2</mat-icon> View Stock
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .title { margin-top: 10px; margin-bottom: 5px; }
    .category { color: #999; margin-bottom: 20px; }
    .mb-2 { margin-bottom: 0.5rem; }
    .w-100 { width: 100%; }
  `]
})
export class ClerkDashboardComponent implements OnInit {
  stats = { deliveriesToday: 5, pendingGRNs: 2, itemsInStock: 234, issueRequests: 3 };
  deliveries = [
    { poNumber: 'PO-2024-001', supplier: 'Fertilizer Co', itemCount: 3 },
    { poNumber: 'PO-2024-002', supplier: 'Seed Ltd', itemCount: 5 }
  ];
  ngOnInit() {}
}
