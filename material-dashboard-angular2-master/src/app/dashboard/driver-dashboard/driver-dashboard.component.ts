import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-driver-dashboard',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatIconModule, MatButtonModule, RouterModule],
  template: `
    <div class="main-content">
      <div class="container-fluid">
        <div class="row">
          <div class="col-lg-12">
            <h2 class="title">Driver Dashboard</h2>
            <p class="category">My Vehicle & Trips</p>
          </div>
        </div>

        <div class="row">
          <div class="col-lg-3 col-md-6">
            <div class="card card-stats">
              <div class="card-header card-header-success card-header-icon">
                <div class="card-icon"><mat-icon>directions_car</mat-icon></div>
                <p class="card-category">Assigned Vehicle</p>
                <h3 class="card-title">{{ assignedVehicle }}</h3>
              </div>
            </div>
          </div>
          <div class="col-lg-3 col-md-6">
            <div class="card card-stats">
              <div class="card-header card-header-info card-header-icon">
                <div class="card-icon"><mat-icon>route</mat-icon></div>
                <p class="card-category">Today's Trips</p>
                <h3 class="card-title">{{ stats.tripsToday }}</h3>
              </div>
            </div>
          </div>
          <div class="col-lg-3 col-md-6">
            <div class="card card-stats">
              <div class="card-header card-header-warning card-header-icon">
                <div class="card-icon"><mat-icon>local_gas_station</mat-icon></div>
                <p class="card-category">Fuel Balance</p>
                <h3 class="card-title">R{{ stats.fuelBalance | number:'1.2-2' }}</h3>
              </div>
            </div>
          </div>
          <div class="col-lg-3 col-md-6">
            <div class="card card-stats">
              <div class="card-header card-header-danger card-header-icon">
                <div class="card-icon"><mat-icon>build</mat-icon></div>
                <p class="card-category">Maintenance Due</p>
                <h3 class="card-title">{{ stats.maintenanceDue }}</h3>
              </div>
            </div>
          </div>
        </div>

        <div class="row">
          <div class="col-md-6">
            <div class="card">
              <div class="card-header card-header-primary">
                <h4 class="card-title">Today's Trips</h4>
              </div>
              <div class="card-body">
                <div class="table-responsive">
                  <table class="table">
                    <thead><tr><th>Destination</th><th>Time</th><th>Status</th></tr></thead>
                    <tbody>
                      <tr *ngFor="let trip of todaysTrips">
                        <td>{{ trip.destination }}</td>
                        <td>{{ trip.time }}</td>
                        <td><span class="badge badge-{{ trip.status }}">{{ trip.status }}</span></td>
                      </tr>
                    </tbody>
                  </table>
                </div>
              </div>
            </div>
          </div>
          <div class="col-md-6">
            <div class="card">
              <div class="card-header card-header-success">
                <h4 class="card-title">Quick Actions</h4>
              </div>
              <div class="card-body">
                <button mat-raised-button color="primary" class="mb-2 w-100" [routerLink]="['/fleet/trips/new']">
                  <mat-icon>add_road</mat-icon> Start Trip
                </button>
                <button mat-raised-button class="mb-2 w-100" [routerLink]="['/fleet/fuel']">
                  <mat-icon>local_gas_station</mat-icon> Log Fuel
                </button>
                <button mat-raised-button class="mb-2 w-100" [routerLink]="['/fleet/vehicles']">
                  <mat-icon>directions_car</mat-icon> My Vehicle
                </button>
              </div>
            </div>
            <div class="card">
              <div class="card-header card-header-warning">
                <h4 class="card-title">Pre-Trip Checklist</h4>
              </div>
              <div class="card-body">
                <div class="checklist">
                  <div *ngFor="let item of preTrip" class="check-item">
                    <mat-icon>{{ item.checked ? 'check_circle' : 'radio_button_unchecked' }}</mat-icon>
                    {{ item.label }}
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
    .mb-2 { margin-bottom: 0.5rem; }
    .w-100 { width: 100%; }
    .badge { padding: 4px 8px; border-radius: 12px; font-size: 12px; }
    .badge-completed { background-color: #4caf50; color: white; }
    .badge-in-progress { background-color: #00bcd4; color: white; }
    .badge-pending { background-color: #ff9800; color: white; }
    .check-item { display: flex; align-items: center; margin-bottom: 8px; }
    .check-item mat-icon { margin-right: 8px; color: #4caf50; }
  `]
})
export class DriverDashboardComponent implements OnInit {
  assignedVehicle = 'ABC-123';
  stats = { tripsToday: 2, fuelBalance: 500, maintenanceDue: 0 };
  todaysTrips = [
    { destination: 'North Field', time: '08:00', status: 'completed' },
    { destination: 'Workshop', time: '14:00', status: 'pending' }
  ];
  preTrip = [
    { label: 'Check tires', checked: true },
    { label: 'Check oil level', checked: true },
    { label: 'Check lights', checked: false }
  ];
  ngOnInit() {}
}
