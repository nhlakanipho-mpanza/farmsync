import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from 'app/core/services/auth.service';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatIconModule, MatButtonModule, RouterModule],
  template: `
    <div class="main-content">
      <div class="container-fluid">
        <div class="row">
          <div class="col-lg-12">
            <h2 class="title">System Administrator Dashboard</h2>
            <p class="category">Full System Overview & Management</p>
          </div>
        </div>

        <!-- Quick Stats -->
        <div class="row">
          <div class="col-lg-3 col-md-6 col-sm-6">
            <div class="card card-stats">
              <div class="card-header card-header-warning card-header-icon">
                <div class="card-icon">
                  <mat-icon>people</mat-icon>
                </div>
                <p class="card-category">Total Users</p>
                <h3 class="card-title">{{ stats.totalUsers }}</h3>
              </div>
              <div class="card-footer">
                <button mat-button color="primary" [routerLink]="['/admin/users']">Manage Users</button>
              </div>
            </div>
          </div>

          <div class="col-lg-3 col-md-6 col-sm-6">
            <div class="card card-stats">
              <div class="card-header card-header-success card-header-icon">
                <div class="card-icon">
                  <mat-icon>inventory_2</mat-icon>
                </div>
                <p class="card-category">Inventory Items</p>
                <h3 class="card-title">{{ stats.inventoryItems }}</h3>
              </div>
              <div class="card-footer">
                <button mat-button color="primary" [routerLink]="['/inventory']">View Inventory</button>
              </div>
            </div>
          </div>

          <div class="col-lg-3 col-md-6 col-sm-6">
            <div class="card card-stats">
              <div class="card-header card-header-danger card-header-icon">
                <div class="card-icon">
                  <mat-icon>warning</mat-icon>
                </div>
                <p class="card-category">System Alerts</p>
                <h3 class="card-title">{{ stats.systemAlerts }}</h3>
              </div>
              <div class="card-footer">
                <div class="stats">
                  <mat-icon>update</mat-icon>
                  Just Updated
                </div>
              </div>
            </div>
          </div>

          <div class="col-lg-3 col-md-6 col-sm-6">
            <div class="card card-stats">
              <div class="card-header card-header-info card-header-icon">
                <div class="card-icon">
                  <mat-icon>assessment</mat-icon>
                </div>
                <p class="card-category">Active Modules</p>
                <h3 class="card-title">7/7</h3>
              </div>
              <div class="card-footer">
                <div class="stats">
                  <mat-icon>check_circle</mat-icon>
                  All Operational
                </div>
              </div>
            </div>
          </div>
        </div>

        <!-- Module Status -->
        <div class="row">
          <div class="col-md-8">
            <div class="card">
              <div class="card-header card-header-primary">
                <h4 class="card-title">Module Status</h4>
                <p class="card-category">System health across all modules</p>
              </div>
              <div class="card-body">
                <div class="table-responsive">
                  <table class="table">
                    <thead class="text-primary">
                      <tr>
                        <th>Module</th>
                        <th>Status</th>
                        <th>Users</th>
                        <th>Actions</th>
                      </tr>
                    </thead>
                    <tbody>
                      <tr *ngFor="let module of moduleStatus">
                        <td>
                          <mat-icon>{{ module.icon }}</mat-icon>
                          {{ module.name }}
                        </td>
                        <td>
                          <span class="badge" [ngClass]="module.status === 'Active' ? 'badge-success' : 'badge-warning'">
                            {{ module.status }}
                          </span>
                        </td>
                        <td>{{ module.activeUsers }}</td>
                        <td>
                          <button mat-icon-button [routerLink]="module.route">
                            <mat-icon>arrow_forward</mat-icon>
                          </button>
                        </td>
                      </tr>
                    </tbody>
                  </table>
                </div>
              </div>
            </div>
          </div>

          <div class="col-md-4">
            <div class="card">
              <div class="card-header card-header-warning">
                <h4 class="card-title">Quick Actions</h4>
                <p class="card-category">Admin tools</p>
              </div>
              <div class="card-body">
                <div class="list-group">
                  <button mat-raised-button color="primary" class="mb-2" [routerLink]="['/admin/users/new']">
                    <mat-icon>person_add</mat-icon>
                    Add New User
                  </button>
                  <button mat-raised-button color="accent" class="mb-2" [routerLink]="['/admin/permissions']">
                    <mat-icon>security</mat-icon>
                    Manage Permissions
                  </button>
                  <button mat-raised-button class="mb-2" [routerLink]="['/admin/reference-data']">
                    <mat-icon>settings</mat-icon>
                    Reference Data
                  </button>
                  <button mat-raised-button class="mb-2" [routerLink]="['/reports']">
                    <mat-icon>assessment</mat-icon>
                    System Reports
                  </button>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .title {
      margin-top: 10px;
      margin-bottom: 5px;
    }
    .category {
      color: #999;
      margin-bottom: 20px;
    }
    .mb-2 {
      margin-bottom: 0.5rem;
      width: 100%;
    }
    .badge {
      padding: 4px 8px;
      border-radius: 12px;
      font-size: 12px;
    }
    .badge-success {
      background-color: #4caf50;
      color: white;
    }
    .badge-warning {
      background-color: #ff9800;
      color: white;
    }
  `]
})
export class AdminDashboardComponent implements OnInit {
  stats = {
    totalUsers: 0,
    inventoryItems: 0,
    systemAlerts: 0
  };

  moduleStatus = [
    { name: 'Inventory', icon: 'inventory_2', status: 'Active', activeUsers: 12, route: '/inventory' },
    { name: 'Procurement', icon: 'shopping_cart', status: 'Active', activeUsers: 8, route: '/procurement/dashboard' },
    { name: 'Workforce', icon: 'people', status: 'Active', activeUsers: 15, route: '/hr/dashboard' },
    { name: 'Fleet', icon: 'local_shipping', status: 'Active', activeUsers: 5, route: '/fleet/dashboard' },
    { name: 'Finance', icon: 'account_balance', status: 'Active', activeUsers: 4, route: '/finance/dashboard' },
    { name: 'Reports', icon: 'assessment', status: 'Active', activeUsers: 10, route: '/reports' },
    { name: 'Admin', icon: 'settings', status: 'Active', activeUsers: 2, route: '/admin' }
  ];

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit() {
    this.loadStats();
  }

  loadStats() {
    // TODO: Load actual stats from API
    this.stats = {
      totalUsers: 45,
      inventoryItems: 234,
      systemAlerts: 3
    };
  }
}
