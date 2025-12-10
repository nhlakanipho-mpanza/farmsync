import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-team-leader-dashboard',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatIconModule, MatButtonModule, RouterModule],
  template: `
    <div class="main-content">
      <div class="container-fluid">
        <div class="row">
          <div class="col-lg-12">
            <h2 class="title">Team Leader Dashboard</h2>
            <p class="category">My Team Management</p>
          </div>
        </div>

        <div class="row">
          <div class="col-lg-3 col-md-6">
            <div class="card card-stats">
              <div class="card-header card-header-success card-header-icon">
                <div class="card-icon"><mat-icon>people</mat-icon></div>
                <p class="card-category">Team Members</p>
                <h3 class="card-title">{{ stats.teamMembers }}</h3>
              </div>
            </div>
          </div>
          <div class="col-lg-3 col-md-6">
            <div class="card card-stats">
              <div class="card-header card-header-info card-header-icon">
                <div class="card-icon"><mat-icon>assignment</mat-icon></div>
                <p class="card-category">Active Tasks</p>
                <h3 class="card-title">{{ stats.activeTasks }}</h3>
              </div>
            </div>
          </div>
          <div class="col-lg-3 col-md-6">
            <div class="card card-stats">
              <div class="card-header card-header-warning card-header-icon">
                <div class="card-icon"><mat-icon>schedule</mat-icon></div>
                <p class="card-category">Present Today</p>
                <h3 class="card-title">{{ stats.presentToday }}/{{ stats.teamMembers }}</h3>
              </div>
            </div>
          </div>
          <div class="col-lg-3 col-md-6">
            <div class="card card-stats">
              <div class="card-header card-header-danger card-header-icon">
                <div class="card-icon"><mat-icon>build</mat-icon></div>
                <p class="card-category">Equipment Issued</p>
                <h3 class="card-title">{{ stats.equipmentIssued }}</h3>
              </div>
            </div>
          </div>
        </div>

        <div class="row">
          <div class="col-md-8">
            <div class="card">
              <div class="card-header card-header-primary">
                <h4 class="card-title">My Team Tasks</h4>
              </div>
              <div class="card-body">
                <div class="table-responsive">
                  <table class="table">
                    <thead><tr><th>Task</th><th>Assigned To</th><th>Status</th><th>Progress</th></tr></thead>
                    <tbody>
                      <tr *ngFor="let task of teamTasks">
                        <td>{{ task.name }}</td>
                        <td>{{ task.assignedTo }}</td>
                        <td><span class="badge badge-{{ task.status }}">{{ task.status }}</span></td>
                        <td>{{ task.progress }}%</td>
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
                <button mat-raised-button color="primary" class="mb-2 w-100" [routerLink]="['/hr/tasks/new']">
                  <mat-icon>add</mat-icon> Assign Task
                </button>
                <button mat-raised-button class="mb-2 w-100" [routerLink]="['/hr/attendance']">
                  <mat-icon>how_to_reg</mat-icon> Attendance
                </button>
                <button mat-raised-button class="mb-2 w-100" [routerLink]="['/hr/teams']">
                  <mat-icon>groups</mat-icon> My Team
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
    .badge-completed { background-color: #4caf50; color: white; }
    .badge-in-progress { background-color: #00bcd4; color: white; }
    .badge-pending { background-color: #ff9800; color: white; }
  `]
})
export class TeamLeaderDashboardComponent implements OnInit {
  stats = { teamMembers: 8, activeTasks: 6, presentToday: 7, equipmentIssued: 4 };
  teamTasks = [
    { name: 'Field Irrigation', assignedTo: 'John Doe', status: 'in-progress', progress: 60 },
    { name: 'Equipment Maintenance', assignedTo: 'Jane Smith', status: 'completed', progress: 100 }
  ];
  ngOnInit() {}
}
