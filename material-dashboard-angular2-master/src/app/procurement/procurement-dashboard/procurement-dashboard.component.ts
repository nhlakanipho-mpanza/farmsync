import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-procurement-dashboard',
  templateUrl: './procurement-dashboard.component.html',
  styleUrls: ['./procurement-dashboard.component.css']
})
export class ProcurementDashboardComponent implements OnInit {
  // Dashboard metrics
  activePOs: number = 0;
  pendingApprovals: number = 0;
  totalSuppliers: number = 0;
  monthlySpend: number = 0;

  constructor() { }

  ngOnInit() {
    this.loadDashboardData();
  }

  loadDashboardData() {
    // TODO: Load actual data from procurement service
    this.activePOs = 23;
    this.pendingApprovals = 5;
    this.totalSuppliers = 47;
    this.monthlySpend = 1850000;
  }
}
