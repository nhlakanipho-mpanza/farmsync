import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-tasks-dashboard',
  templateUrl: './tasks-dashboard.component.html',
  styleUrls: ['./tasks-dashboard.component.css']
})
export class TasksDashboardComponent implements OnInit {
  // Dashboard metrics
  activeTasks: number = 0;
  overdueTasks: number = 0;
  completedTasksMonth: number = 0;
  attendanceToday: number = 0;

  constructor() { }

  ngOnInit() {
    this.loadDashboardData();
  }

  loadDashboardData() {
    // TODO: Load actual data from tasks service
    this.activeTasks = 34;
    this.overdueTasks = 7;
    this.completedTasksMonth = 89;
    this.attendanceToday = 78;
  }
}
