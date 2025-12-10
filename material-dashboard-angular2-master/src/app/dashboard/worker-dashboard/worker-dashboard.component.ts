import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-worker-dashboard',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatIconModule, MatButtonModule, RouterModule],
  template: `
    <div class="main-content">
      <div class="container-fluid">
        <div class="row">
          <div class="col-lg-12">
            <h2 class="title">Worker Dashboard</h2>
            <p class="category">My Tasks & Schedule</p>
          </div>
        </div>

        <!-- Time Clock -->
        <div class="row">
          <div class="col-md-4">
            <div class="card">
              <div class="card-header" [ngClass]="isClockedIn ? 'card-header-success' : 'card-header-warning'">
                <h4 class="card-title">Time Clock</h4>
              </div>
              <div class="card-body text-center">
                <mat-icon style="font-size: 64px; height: 64px; width: 64px;">
                  {{ isClockedIn ? 'access_time' : 'timer_off' }}
                </mat-icon>
                <h3 class="mt-3">{{ isClockedIn ? 'Clocked In' : 'Clocked Out' }}</h3>
                <p *ngIf="isClockedIn">Since: {{ clockInTime | date:'short' }}</p>
                <p *ngIf="isClockedIn">Hours Today: {{ hoursToday }}</p>
                <button mat-raised-button 
                        [color]="isClockedIn ? 'warn' : 'primary'" 
                        class="w-100 mt-3"
                        (click)="toggleClock()">
                  <mat-icon>{{ isClockedIn ? 'logout' : 'login' }}</mat-icon>
                  {{ isClockedIn ? 'Clock Out' : 'Clock In' }}
                </button>
              </div>
            </div>
          </div>

          <div class="col-md-8">
            <div class="card">
              <div class="card-header card-header-info">
                <h4 class="card-title">My Tasks Today</h4>
                <p class="card-category">{{ completedTasks }}/{{ myTasks.length }} Completed</p>
              </div>
              <div class="card-body">
                <div class="task-list">
                  <div *ngFor="let task of myTasks" class="task-item" [ngClass]="{'completed': task.completed}">
                    <div class="task-checkbox">
                      <mat-icon>{{ task.completed ? 'check_circle' : 'radio_button_unchecked' }}</mat-icon>
                    </div>
                    <div class="task-details">
                      <h5>{{ task.name }}</h5>
                      <p>{{ task.description }}</p>
                      <small>{{ task.location }} • Assigned by: {{ task.assignedBy }}</small>
                    </div>
                    <div class="task-actions">
                      <button mat-icon-button *ngIf="!task.completed" (click)="completeTask(task)">
                        <mat-icon>check</mat-icon>
                      </button>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>

        <!-- Equipment Checked Out -->
        <div class="row">
          <div class="col-md-6">
            <div class="card">
              <div class="card-header card-header-warning">
                <h4 class="card-title">Equipment Checked Out</h4>
              </div>
              <div class="card-body">
                <div class="table-responsive">
                  <table class="table">
                    <thead>
                      <tr>
                        <th>Item</th>
                        <th>Issued</th>
                        <th>Return By</th>
                      </tr>
                    </thead>
                    <tbody>
                      <tr *ngFor="let item of checkedOutEquipment">
                        <td>{{ item.name }}</td>
                        <td>{{ item.issuedDate | date:'short' }}</td>
                        <td>{{ item.returnDate | date:'short' }}</td>
                      </tr>
                      <tr *ngIf="checkedOutEquipment.length === 0">
                        <td colspan="3" class="text-center">No equipment checked out</td>
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
                <h4 class="card-title">Upcoming Schedule</h4>
              </div>
              <div class="card-body">
                <div class="timeline">
                  <div *ngFor="let event of upcomingSchedule" class="timeline-item">
                    <div class="timeline-marker"></div>
                    <div class="timeline-content">
                      <h6>{{ event.date | date:'EEE, MMM d' }}</h6>
                      <p>{{ event.task }}</p>
                      <small>{{ event.team }}</small>
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
    .w-100 {
      width: 100%;
    }
    .mt-3 {
      margin-top: 1rem;
    }
    .task-list {
      max-height: 400px;
      overflow-y: auto;
    }
    .task-item {
      display: flex;
      align-items: center;
      padding: 15px;
      border-bottom: 1px solid #e0e0e0;
      transition: background-color 0.3s;
    }
    .task-item:hover {
      background-color: #f5f5f5;
    }
    .task-item.completed {
      opacity: 0.6;
    }
    .task-item.completed .task-details h5 {
      text-decoration: line-through;
    }
    .task-checkbox {
      margin-right: 15px;
    }
    .task-checkbox mat-icon {
      font-size: 32px;
      height: 32px;
      width: 32px;
      color: #4caf50;
    }
    .task-details {
      flex: 1;
    }
    .task-details h5 {
      margin: 0 0 5px 0;
    }
    .task-details p {
      margin: 0 0 5px 0;
      color: #666;
    }
    .task-details small {
      color: #999;
    }
    .timeline {
      position: relative;
      padding-left: 30px;
    }
    .timeline-item {
      position: relative;
      padding-bottom: 20px;
    }
    .timeline-marker {
      position: absolute;
      left: -30px;
      width: 12px;
      height: 12px;
      border-radius: 50%;
      background-color: #00bcd4;
      border: 2px solid #fff;
      box-shadow: 0 0 0 2px #00bcd4;
    }
    .timeline-content h6 {
      margin: 0 0 5px 0;
      font-weight: 600;
    }
    .timeline-content p {
      margin: 0 0 3px 0;
    }
    .timeline-content small {
      color: #999;
    }
  `]
})
export class WorkerDashboardComponent implements OnInit {
  isClockedIn = false;
  clockInTime: Date | null = null;
  hoursToday = '0h 0m';

  myTasks = [
    { id: 1, name: 'Field Irrigation', description: 'Water section A', location: 'North Field', assignedBy: 'John Manager', completed: false },
    { id: 2, name: 'Equipment Check', description: 'Inspect tractor #3', location: 'Workshop', assignedBy: 'John Manager', completed: true },
    { id: 3, name: 'Fertilizer Application', description: 'Apply to section B', location: 'South Field', assignedBy: 'John Manager', completed: false }
  ];

  checkedOutEquipment = [
    { name: 'Irrigation Pump #2', issuedDate: new Date('2025-12-10T08:00:00'), returnDate: new Date('2025-12-10T17:00:00') },
    { name: 'Hand Tools Set', issuedDate: new Date('2025-12-10T08:00:00'), returnDate: new Date('2025-12-10T17:00:00') }
  ];

  upcomingSchedule = [
    { date: new Date('2025-12-11'), task: 'Harvesting - North Field', team: 'Field Team 1' },
    { date: new Date('2025-12-12'), task: 'Equipment Maintenance', team: 'Maintenance Team' },
    { date: new Date('2025-12-13'), task: 'Planting - South Field', team: 'Field Team 1' }
  ];

  get completedTasks(): number {
    return this.myTasks.filter(t => t.completed).length;
  }

  constructor() {}

  ngOnInit() {
    // Check if already clocked in (from localStorage or API)
    this.checkClockStatus();
  }

  toggleClock() {
    if (this.isClockedIn) {
      // Clock out
      this.isClockedIn = false;
      this.clockInTime = null;
      this.hoursToday = '0h 0m';
      // TODO: Call API to clock out
    } else {
      // Clock in
      this.isClockedIn = true;
      this.clockInTime = new Date();
      // TODO: Call API to clock in
    }
  }

  completeTask(task: any) {
    task.completed = true;
    // TODO: Call API to update task status
  }

  checkClockStatus() {
    // TODO: Check API for clock status
  }
}
