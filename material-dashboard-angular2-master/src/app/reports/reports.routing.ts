import { Routes } from '@angular/router';
import { ReportsComponent } from './reports.component';
import { PurchaseOrderReportComponent } from './purchase-order-report/purchase-order-report.component';
import { InventoryValuationComponent } from './inventory-valuation/inventory-valuation.component';
import { SupplierTransactionsComponent } from './supplier-transactions/supplier-transactions.component';
import { ExpenseReportComponent } from './expense-report/expense-report.component';
import { GoodsReceivedReportComponent } from './goods-received-report/goods-received-report.component';

export const ReportsRoutes: Routes = [
  {
    path: '',
    component: ReportsComponent
  },
  {
    path: 'purchase-orders',
    component: PurchaseOrderReportComponent
  },
  {
    path: 'inventory-valuation',
    component: InventoryValuationComponent
  },
  {
    path: 'supplier-transactions',
    component: SupplierTransactionsComponent
  },
  {
    path: 'expenses',
    component: ExpenseReportComponent
  },
  {
    path: 'goods-received',
    component: GoodsReceivedReportComponent
  }
];
