import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';

import { ReportsRoutes } from './reports.routing';
import { ReportsComponent } from './reports.component';
import { PurchaseOrderReportComponent } from './purchase-order-report/purchase-order-report.component';
import { InventoryValuationComponent } from './inventory-valuation/inventory-valuation.component';
import { SupplierTransactionsComponent } from './supplier-transactions/supplier-transactions.component';
import { ExpenseReportComponent } from './expense-report/expense-report.component';
import { GoodsReceivedReportComponent } from './goods-received-report/goods-received-report.component';

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild(ReportsRoutes),
    FormsModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatInputModule,
    MatFormFieldModule,
    MatSelectModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatTableModule,
    MatCardModule
  ],
  declarations: [
    ReportsComponent,
    PurchaseOrderReportComponent,
    InventoryValuationComponent,
    SupplierTransactionsComponent,
    ExpenseReportComponent,
    GoodsReceivedReportComponent
  ]
})
export class ReportsModule { }
