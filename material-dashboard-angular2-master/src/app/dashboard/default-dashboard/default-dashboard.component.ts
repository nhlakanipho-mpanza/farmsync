import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-default-dashboard',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatIconModule, MatButtonModule, RouterModule],
  template: `
    <div class="main-content">
      <div class="container-fluid">
        <div class="row">
          <div class="col-lg-12">
            <h2 class="title">Dashboard</h2>
            <p class="category">Welcome to FarmSync</p>
          </div>
        </div>

        <div class="row">
          <div class="col-lg-3 col-md-6">
            <div class="card card-stats">
              <div class="card-header card-header-warning card-header-icon">
                <div class="card-icon"><mat-icon>notifications</mat-icon></div>
                <p class="card-category">Notifications</p>
                <h3 class="card-title">{{ stats.notifications }}</h3>
              </div>
            </div>
          </div>
          <div class="col-lg-3 col-md-6">
            <div class="card card-stats">
              <div class="card-header card-header-success card-header-icon">
                <div class="card-icon"><mat-icon>assignment</mat-icon></div>
                <p class="card-category">My Tasks</p>
                <h3 class="card-title">{{ stats.myTasks }}</h3>
              </div>
            </div>
          </div>
          <div class="col-lg-3 col-md-6">
            <div class="card card-stats">
              <div class="card-header card-header-info card-header-icon">
                <div class="card-icon"><mat-icon>schedule</mat-icon></div>
                <p class="card-category">Hours Today</p>
                <h3 class="card-title">{{ stats.hoursToday }}</h3>
              </div>
            </div>
          </div>
          <div class="col-lg-3 col-md-6">
            <div class="card card-stats">
              <div class="card-header card-header-danger card-header-icon">
                <div class="card-icon"><mat-icon>warning</mat-icon></div>
                <p class="card-category">Alerts</p>
                <h3 class="card-title">{{ stats.alerts }}</h3>
              </div>
            </div>
          </div>
        </div>

        <div class="row">
          <div class="col-md-12">
            <div class="card">
              <div class="card-header card-header-primary">
                <h4 class="card-title">Quick Links</h4>
              </div>
              <div class="card-body">
                <div class="row">
                  <div class="col-md-3" *ngFor="let module of modules">
                    <button mat-raised-button class="module-button" [routerLink]="[module.route]">
                      <mat-icon>{{ module.icon }}</mat-icon>
                      <span>{{ module.name }}</span>
                    </button>
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
    .title { margin-top: 10px; margin-bottom: 5px; }
    .category { color: #999; margin-bottom: 20px; }
    .module-button { width: 100%; height: 80px; margin-bottom: 20px; display: flex; flex-direction: column; align-items: center; justify-content: center; }
    .module-button mat-icon { font-size: 32px; width: 32px; height: 32px; margin-bottom: 8px; }
  `]
})
export class DefaultDashboardComponent implements OnInit {
  stats = { notifications: 5, myTasks: 3, hoursToday: 0, alerts: 2 };
  modules = [
    { name: 'Inventory', icon: 'inventory', route: '/inventory' },
    { name: 'Procurement', icon: 'shopping_cart', route: '/procurement' },
    { name: 'Workforce', icon: 'people', route: '/hr' },
    { name: 'Fleet', icon: 'local_shipping', route: '/fleet' },
    { name: 'Reports', icon: 'assessment', route: '/reports' }
  ];
  ngOnInit() {}
}
