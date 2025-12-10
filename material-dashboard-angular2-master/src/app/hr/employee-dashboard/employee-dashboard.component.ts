import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-employee-dashboard',
  templateUrl: './employee-dashboard.component.html',
  styleUrls: ['./employee-dashboard.component.css']
})
export class EmployeeDashboardComponent implements OnInit {
  // Dashboard metrics
  totalEmployees: number = 0;
  activeEmployees: number = 0;
  totalTeams: number = 0;
  recentHires: number = 0;

  constructor() { }

  ngOnInit() {
    this.loadDashboardData();
  }

  loadDashboardData() {
    // TODO: Load actual data from employee service
    this.totalEmployees = 86;
    this.activeEmployees = 82;
    this.totalTeams = 12;
    this.recentHires = 4;
  }
}
