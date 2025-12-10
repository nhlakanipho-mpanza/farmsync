import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-accounting-dashboard',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatIconModule, MatButtonModule, RouterModule],
  template: `
    <div class="main-content">
      <div class="container-fluid">
        <div class="row">
          <div class="col-lg-12">
            <h2 class="title">Accounting Dashboard</h2>
            <p class="category">Financial Overview & Approvals</p>
          </div>
        </div>

        <div class="row">
          <div class="col-lg-3 col-md-6">
            <div class="card card-stats">
              <div class="card-header card-header-warning card-header-icon">
                <div class="card-icon"><mat-icon>pending_actions</mat-icon></div>
                <p class="card-category">Pending PO Approvals</p>
                <h3 class="card-title">{{ stats.pendingPOs }}</h3>
              </div>
              <div class="card-footer">
                <button mat-button color="primary" [routerLink]="['/procurement/approvals']">Review</button>
              </div>
            </div>
          </div>
          <div class="col-lg-3 col-md-6">
            <div class="card card-stats">
              <div class="card-header card-header-success card-header-icon">
                <div class="card-icon"><mat-icon>account_balance_wallet</mat-icon></div>
                <p class="card-category">Monthly Spend</p>
                <h3 class="card-title">R{{ stats.monthlySpend | number:'1.2-2' }}</h3>
              </div>
            </div>
          </div>
          <div class="col-lg-3 col-md-6">
            <div class="card card-stats">
              <div class="card-header card-header-danger card-header-icon">
                <div class="card-icon"><mat-icon>receipt</mat-icon></div>
                <p class="card-category">Unpaid Invoices</p>
                <h3 class="card-title">{{ stats.unpaidInvoices }}</h3>
              </div>
            </div>
          </div>
          <div class="col-lg-3 col-md-6">
            <div class="card card-stats">
              <div class="card-header card-header-info card-header-icon">
                <div class="card-icon"><mat-icon>trending_up</mat-icon></div>
                <p class="card-category">Budget vs Actual</p>
                <h3 class="card-title">{{ stats.budgetVariance }}%</h3>
              </div>
            </div>
          </div>
        </div>

        <div class="row">
          <div class="col-md-8">
            <div class="card">
              <div class="card-header card-header-primary">
                <h4 class="card-title">Recent Transactions</h4>
              </div>
              <div class="card-body">
                <div class="table-responsive">
                  <table class="table">
                    <thead><tr><th>Date</th><th>Supplier</th><th>Amount</th><th>Status</th></tr></thead>
                    <tbody>
                      <tr *ngFor="let txn of recentTransactions">
                        <td>{{ txn.date | date:'short' }}</td>
                        <td>{{ txn.supplier }}</td>
                        <td>R{{ txn.amount | number:'1.2-2' }}</td>
                        <td><span class="badge badge-{{ txn.status }}">{{ txn.status }}</span></td>
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
                <button mat-raised-button color="primary" class="mb-2 w-100" [routerLink]="['/procurement/purchase-orders/new']">
                  <mat-icon>add</mat-icon> Create Purchase Order
                </button>
                <button mat-raised-button class="mb-2 w-100" [routerLink]="['/reports']">
                  <mat-icon>assessment</mat-icon> Financial Reports
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
    .badge { padding: 4px 8px; border-radius: 12px; font-size: 12px; }
    .badge-approved { background-color: #4caf50; color: white; }
    .badge-pending { background-color: #ff9800; color: white; }
  `]
})
export class AccountingDashboardComponent implements OnInit {
  stats = { pendingPOs: 3, monthlySpend: 125000, unpaidInvoices: 5, budgetVariance: 8 };
  recentTransactions = [
    { date: new Date(), supplier: 'Fertilizer Co', amount: 15000, status: 'approved' },
    { date: new Date(), supplier: 'Seed Suppliers', amount: 22000, status: 'pending' }
  ];
  ngOnInit() {}
}
