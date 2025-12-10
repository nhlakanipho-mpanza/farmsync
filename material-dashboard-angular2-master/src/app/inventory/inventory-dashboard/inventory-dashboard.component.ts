import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-inventory-dashboard',
  templateUrl: './inventory-dashboard.component.html',
  styleUrls: ['./inventory-dashboard.component.css']
})
export class InventoryDashboardComponent implements OnInit {
  // Dashboard metrics
  totalItems: number = 0;
  lowStockItems: number = 0;
  outOfStockItems: number = 0;
  totalValue: number = 0;

  // Recent activity
  recentMovements: any[] = [];
  expiringItems: any[] = [];
  lowStockList: any[] = [];

  constructor() { }

  ngOnInit() {
    this.loadDashboardData();
  }

  loadDashboardData() {
    // TODO: Load actual data from inventory service
    // For now, using placeholder data
    this.totalItems = 145;
    this.lowStockItems = 12;
    this.outOfStockItems = 3;
    this.totalValue = 2450000;
  }
}
