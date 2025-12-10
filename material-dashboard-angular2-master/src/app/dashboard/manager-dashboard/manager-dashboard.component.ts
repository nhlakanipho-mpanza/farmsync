import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatBadgeModule } from '@angular/material/badge';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-manager-dashboard',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatIconModule, MatButtonModule, MatBadgeModule, RouterModule],
  template: `
    <div class="main-content">
      <div class="container-fluid">
        <div class="row">
          <div class="col-lg-12">
            <h2 class="title">Operations Manager Dashboard</h2>
            <p class="category">Daily Operations & Approvals Overview</p>
          </div>
        </div>

        <!-- Pending Approvals -->
        <div class="row">
          <div class="col-lg-3 col-md-6">
            <div class="card card-stats">
              <div class="card-header card-header-warning card-header-icon">
                <div class="card-icon">
                  <mat-icon [matBadge]="pendingApprovals.purchaseOrders" matBadgeColor="warn">receipt_long</mat-icon>
                </div>
                <p class="card-category">Purchase Orders</p>
                <h3 class="card-title">{{ pendingApprovals.purchaseOrders }} Pending</h3>
              </div>
              <div class="card-footer">
                <button mat-button color="primary" [routerLink]="['/procurement/approvals']">Review</button>
              </div>
            </div>
          </div>

          <div class="col-lg-3 col-md-6">
            <div class="card card-stats">
              <div class="card-header card-header-info card-header-icon">
                <div class="card-icon">
                  <mat-icon [matBadge]="pendingApprovals.goodsReceiving" matBadgeColor="warn">local_shipping</mat-icon>
                </div>
                <p class="card-category">Goods Receiving</p>
                <h3 class="card-title">{{ pendingApprovals.goodsReceiving }} Pending</h3>
              </div>
              <div class="card-footer">
                <button mat-button color="primary" [routerLink]="['/procurement/approvals']">Review</button>
              </div>
            </div>
          </div>

          <div class="col-lg-3 col-md-6">
            <div class="card card-stats">
              <div class="card-header card-header-success card-header-icon">
                <div class="card-icon">
                  <mat-icon [matBadge]="pendingApprovals.tasks" matBadgeColor="warn">assignment</mat-icon>
                </div>
                <p class="card-category">Task Completions</p>
                <h3 class="card-title">{{ pendingApprovals.tasks }} To Approve</h3>
              </div>
              <div class="card-footer">
                <button mat-button color="primary" [routerLink]="['/hr/tasks']">View Tasks</button>
              </div>
            </div>
          </div>

          <div class="col-lg-3 col-md-6">
            <div class="card card-stats">
              <div class="card-header card-header-danger card-header-icon">
                <div class="card-icon">
                  <mat-icon [matBadge]="alerts.lowStock" matBadgeColor="warn">inventory_2</mat-icon>
                </div>
                <p class="card-category">Low Stock Items</p>
                <h3 class="card-title">{{ alerts.lowStock }} Items</h3>
              </div>
              <div class="card-footer">
                <button mat-button color="primary" [routerLink]="['/inventory']">View Stock</button>
              </div>
            </div>
          </div>
        </div>

        <!-- Main Content -->
        <div class="row">
          <div class="col-md-8">
            <div class="card">
              <div class="card-header card-header-primary">
                <h4 class="card-title">Active Work Tasks</h4>
                <p class="card-category">Today's operations</p>
              </div>
              <div class="card-body">
                <div class="table-responsive">
                  <table class="table">
                    <thead>
                      <tr>
                        <th>Task</th>
                        <th>Team</th>
                        <th>Status</th>
                        <th>Progress</th>
                        <th>Actions</th>
                      </tr>
                    </thead>
                    <tbody>
                      <tr *ngFor="let task of activeTasks">
                        <td>{{ task.name }}</td>
                        <td>{{ task.team }}</td>
                        <td><span class="badge" [ngClass]="getStatusClass(task.status)">{{ task.status }}</span></td>
                        <td>{{ task.progress }}%</td>
                        <td>
                          <button mat-icon-button [routerLink]="['/hr/tasks', task.id]">
                            <mat-icon>visibility</mat-icon>
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
              <div class="card-header card-header-success">
                <h4 class="card-title">Quick Actions</h4>
              </div>
              <div class="card-body">
                <button mat-raised-button color="primary" class="mb-2 w-100" [routerLink]="['/procurement/approvals']">
                  <mat-icon>approval</mat-icon>
                  Review Approvals
                  <span class="badge-number" *ngIf="totalPendingApprovals > 0">{{ totalPendingApprovals }}</span>
                </button>
                <button mat-raised-button color="accent" class="mb-2 w-100" [routerLink]="['/hr/tasks/new']">
                  <mat-icon>add_task</mat-icon>
                  Assign New Task
                </button>
                <button mat-raised-button class="mb-2 w-100" [routerLink]="['/hr/issuing']">
                  <mat-icon>local_shipping</mat-icon>
                  Issue Equipment
                </button>
                <button mat-raised-button class="mb-2 w-100" [routerLink]="['/fleet/dashboard']">
                  <mat-icon>directions_car</mat-icon>
                  Fleet Status
                </button>
              </div>
            </div>

            <div class="card">
              <div class="card-header card-header-warning">
                <h4 class="card-title">Team Performance</h4>
              </div>
              <div class="card-body">
                <div *ngFor="let team of teamPerformance" class="mb-3">
                  <div class="d-flex justify-content-between">
                    <strong>{{ team.name }}</strong>
                    <span>{{ team.tasksCompleted }}/{{ team.totalTasks }}</span>
                  </div>
                  <div class="progress">
                    <div class="progress-bar bg-success" [style.width.%]="team.completionRate"></div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>

        <!-- Fleet Status -->
        <div class="row">
          <div class="col-md-12">
            <div class="card">
              <div class="card-header card-header-info">
                <h4 class="card-title">Fleet Status</h4>
                <p class="card-category">Active vehicles</p>
              </div>
              <div class="card-body">
                <div class="row">
                  <div class="col-md-3" *ngFor="let vehicle of activeVehicles">
                    <div class="mini-card">
                      <mat-icon>directions_car</mat-icon>
                      <h5>{{ vehicle.registration }}</h5>
                      <p>{{ vehicle.driver }}</p>
                      <span class="badge" [ngClass]="vehicle.status === 'Active' ? 'badge-success' : 'badge-warning'">
                        {{ vehicle.status }}
                      </span>
                    </div>
                  </div>
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
    }
    .w-100 {
      width: 100%;
    }
    .mb-3 {
      margin-bottom: 1rem;
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
    .badge-info {
      background-color: #00bcd4;
      color: white;
    }
    .badge-number {
      margin-left: 8px;
      background: #f44336;
      color: white;
      padding: 2px 6px;
      border-radius: 10px;
      font-size: 11px;
    }
    .progress {
      height: 6px;
      background-color: #e0e0e0;
      border-radius: 3px;
      margin-top: 4px;
    }
    .progress-bar {
      height: 100%;
      border-radius: 3px;
    }
    .mini-card {
      text-align: center;
      padding: 15px;
      border: 1px solid #e0e0e0;
      border-radius: 4px;
      margin-bottom: 15px;
    }
    .mini-card mat-icon {
      font-size: 36px;
      height: 36px;
      width: 36px;
    }
  `]
})
export class ManagerDashboardComponent implements OnInit {
  pendingApprovals = {
    purchaseOrders: 0,
    goodsReceiving: 0,
    tasks: 0
  };

  alerts = {
    lowStock: 0,
    overdueReturns: 0
  };

  get totalPendingApprovals(): number {
    return this.pendingApprovals.purchaseOrders + 
           this.pendingApprovals.goodsReceiving + 
           this.pendingApprovals.tasks;
  }

  activeTasks = [
    { id: 1, name: 'Field Irrigation - Section A', team: 'Field Team 1', status: 'In Progress', progress: 65 },
    { id: 2, name: 'Equipment Maintenance', team: 'Maintenance Team', status: 'In Progress', progress: 40 },
    { id: 3, name: 'Harvest - North Field', team: 'Field Team 2', status: 'Pending', progress: 0 },
    { id: 4, name: 'Fertilizer Application', team: 'Field Team 3', status: 'In Progress', progress: 80 }
  ];

  teamPerformance = [
    { name: 'Field Team 1', tasksCompleted: 8, totalTasks: 10, completionRate: 80 },
    { name: 'Field Team 2', tasksCompleted: 6, totalTasks: 8, completionRate: 75 },
    { name: 'Maintenance Team', tasksCompleted: 5, totalTasks: 6, completionRate: 83 }
  ];

  activeVehicles = [
    { registration: 'ABC-123', driver: 'John Doe', status: 'Active' },
    { registration: 'XYZ-789', driver: 'Jane Smith', status: 'Active' },
    { registration: 'DEF-456', driver: 'Mike Johnson', status: 'Maintenance' }
  ];

  constructor(private router: Router) {}

  ngOnInit() {
    this.loadDashboardData();
  }

  loadDashboardData() {
    // TODO: Load from API
    this.pendingApprovals = {
      purchaseOrders: 3,
      goodsReceiving: 2,
      tasks: 5
    };

    this.alerts = {
      lowStock: 7,
      overdueReturns: 2
    };
  }

  getStatusClass(status: string): string {
    switch(status.toLowerCase()) {
      case 'completed': return 'badge-success';
      case 'in progress': return 'badge-info';
      case 'pending': return 'badge-warning';
      default: return 'badge-secondary';
    }
  }
}
