import { Component, OnInit } from '@angular/core';
import { PermissionService } from '../core/services/permission.service';

interface ReportCategory {
  title: string;
  icon: string;
  reports: Report[];
  permission: string;
}

interface Report {
  name: string;
  description: string;
  icon: string;
  route?: string;
  action?: () => void;
}

@Component({
  selector: 'app-reports',
  templateUrl: './reports.component.html',
  styleUrls: ['./reports.component.css']
})
export class ReportsComponent implements OnInit {
  reportCategories: ReportCategory[] = [];

  constructor(private permissionService: PermissionService) { }

  ngOnInit() {
    this.loadReports();
  }

  loadReports() {
    const allCategories: ReportCategory[] = [
      {
        title: 'Inventory Reports',
        icon: 'inventory_2',
        permission: 'inventory.view',
        reports: [
          {
            name: 'Inventory Valuation',
            description: 'Current stock levels and valuation by category',
            icon: 'inventory',
            route: '/reports/inventory-valuation'
          },
          {
            name: 'Goods Received Report',
            description: 'Track goods received notes and inspection status',
            icon: 'inventory_2',
            route: '/reports/goods-received'
          },
          {
            name: 'Stock Levels',
            description: 'Current stock levels across all items',
            icon: 'assessment'
          },
          {
            name: 'Low Stock Alert',
            description: 'Items below minimum stock threshold',
            icon: 'warning'
          },
          {
            name: 'Stock Movement',
            description: 'Historical stock in/out movements',
            icon: 'swap_horiz'
          }
        ]
      },
      {
        title: 'Financial Reports',
        icon: 'payments',
        permission: 'reports.financial',
        reports: [
          {
            name: 'Purchase Orders Report',
            description: 'Detailed purchase order data with filtering and export',
            icon: 'shopping_cart',
            route: '/reports/purchase-orders'
          },
          {
            name: 'Supplier Transactions',
            description: 'Track supplier purchases, payments, and outstanding balances',
            icon: 'local_shipping',
            route: '/reports/supplier-transactions'
          },
          {
            name: 'Expense Report',
            description: 'Analyze expenses by category and department',
            icon: 'receipt',
            route: '/reports/expenses'
          },
          {
            name: 'Procurement Spending',
            description: 'Purchase order spending analysis',
            icon: 'account_balance'
          },
          {
            name: 'Budget vs Actual',
            description: 'Budget comparison report',
            icon: 'analytics'
          }
        ]
      },
      {
        title: 'HR Reports',
        icon: 'people',
        permission: 'reports.hr',
        reports: [
          {
            name: 'Employee Roster',
            description: 'Active employee listing',
            icon: 'badge'
          },
          {
            name: 'Attendance Summary',
            description: 'Monthly attendance report',
            icon: 'access_time'
          },
          {
            name: 'Leave Report',
            description: 'Employee leave balances and history',
            icon: 'event'
          },
          {
            name: 'Payroll Summary',
            description: 'Monthly payroll costs',
            icon: 'money'
          },
          {
            name: 'Task Completion',
            description: 'Task assignment and completion rates',
            icon: 'assignment_turned_in'
          }
        ]
      },
      {
        title: 'Fleet Reports',
        icon: 'local_shipping',
        permission: 'fleet.view',
        reports: [
          {
            name: 'Vehicle Utilization',
            description: 'Vehicle usage statistics',
            icon: 'directions_car'
          },
          {
            name: 'Fuel Consumption',
            description: 'Fuel usage and costs per vehicle',
            icon: 'local_gas_station'
          },
          {
            name: 'Maintenance Schedule',
            description: 'Upcoming and overdue maintenance',
            icon: 'build'
          },
          {
            name: 'GPS Trip Logs',
            description: 'Vehicle trip history and routes',
            icon: 'map'
          },
          {
            name: 'Driver Assignment',
            description: 'Current driver assignments',
            icon: 'assignment_ind'
          }
        ]
      },
      {
        title: 'Operational Reports',
        icon: 'analytics',
        permission: 'reports.view',
        reports: [
          {
            name: 'Daily Operations',
            description: 'Daily activity summary',
            icon: 'today'
          },
          {
            name: 'Monthly Summary',
            description: 'Comprehensive monthly report',
            icon: 'calendar_month'
          },
          {
            name: 'Equipment Issuing',
            description: 'Issued equipment tracking',
            icon: 'handyman'
          }
        ]
      }
    ];

    // Filter categories based on user permissions
    this.reportCategories = allCategories.filter(category => 
      this.permissionService.can(category.permission)
    );
  }

  generateReport(report: Report) {
    if (report.action) {
      report.action();
    } else {
      // TODO: Implement report generation
      console.log('Generating report:', report.name);
      alert(`Report "${report.name}" generation will be implemented soon.`);
    }
  }
}
