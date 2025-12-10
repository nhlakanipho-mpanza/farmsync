import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AdminLayoutRoutes } from './admin-layout.routing';
import { DashboardComponent } from '../../dashboard/dashboard.component';
import { UserProfileComponent } from '../../user-profile/user-profile.component';
// Role-based Dashboards
import { DashboardRouterComponent } from 'app/dashboard/dashboard-router/dashboard-router.component';
import { AdminDashboardComponent } from 'app/dashboard/admin-dashboard/admin-dashboard.component';
import { ManagerDashboardComponent } from 'app/dashboard/manager-dashboard/manager-dashboard.component';
import { AccountingDashboardComponent } from 'app/dashboard/accounting-dashboard/accounting-dashboard.component';
import { ClerkDashboardComponent } from 'app/dashboard/clerk-dashboard/clerk-dashboard.component';
import { TeamLeaderDashboardComponent } from 'app/dashboard/team-leader-dashboard/team-leader-dashboard.component';
import { DriverDashboardComponent } from 'app/dashboard/driver-dashboard/driver-dashboard.component';
import { WorkerDashboardComponent } from 'app/dashboard/worker-dashboard/worker-dashboard.component';
import { DefaultDashboardComponent } from 'app/dashboard/default-dashboard/default-dashboard.component';
import { InventoryListComponent } from 'app/inventory/inventory-list/inventory-list.component';
import { InventoryFormComponent } from 'app/inventory/inventory-form/inventory-form.component';
import { InventoryDashboardComponent } from 'app/inventory/inventory-dashboard/inventory-dashboard.component';
import { PurchaseOrderListComponent } from 'app/procurement/purchase-order-list/purchase-order-list.component';
import { PurchaseOrderFormComponent } from 'app/procurement/purchase-order-form/purchase-order-form.component';
import { GoodsReceivingFormComponent } from 'app/procurement/goods-receiving-form/goods-receiving-form.component';
import { SupplierListComponent } from 'app/procurement/supplier-list/supplier-list.component';
import { SupplierFormComponent } from 'app/procurement/supplier-form/supplier-form.component';
import { ApprovalDashboardComponent } from 'app/procurement/approval-dashboard/approval-dashboard.component';
import { ProcurementDashboardComponent } from 'app/procurement/procurement-dashboard/procurement-dashboard.component';
import { EmployeeDashboardComponent } from 'app/hr/employee-dashboard/employee-dashboard.component';
import { TasksDashboardComponent } from 'app/hr/tasks-dashboard/tasks-dashboard.component';
import { UserManagementComponent } from 'app/admin/user-management/user-management.component';
import { PermissionsManagementComponent } from 'app/admin/permissions-management/permissions-management.component';
import { ReferenceDataManagerComponent } from 'app/admin/reference-data-manager/reference-data-manager.component';
import { ReplaceReferenceDialogComponent } from 'app/admin/reference-data-manager/replace-reference-dialog.component';
import { HRModule } from 'app/hr/hr.module';
import { FleetModule } from 'app/fleet/fleet.module';
import { CoreModule } from 'app/core/core.module';
import {MatButtonModule} from '@angular/material/button';
import {MatInputModule} from '@angular/material/input';
import {MatRippleModule} from '@angular/material/core';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatTooltipModule} from '@angular/material/tooltip';
import {MatSelectModule} from '@angular/material/select';
import {MatIconModule} from '@angular/material/icon';
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';
import {MatSnackBarModule} from '@angular/material/snack-bar';
import {MatDatepickerModule} from '@angular/material/datepicker';
import {MatNativeDateModule} from '@angular/material/core';
import {MatTableModule} from '@angular/material/table';
import {MatSlideToggleModule} from '@angular/material/slide-toggle';
import {MatChipsModule} from '@angular/material/chips';
import {MatCardModule} from '@angular/material/card';
import {MatTabsModule} from '@angular/material/tabs';
import {MatCheckboxModule} from '@angular/material/checkbox';
import {MatListModule} from '@angular/material/list';
import {MatDialogModule} from '@angular/material/dialog';

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild(AdminLayoutRoutes),
    FormsModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatRippleModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatTooltipModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatSnackBarModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatTableModule,
    MatSlideToggleModule,
    MatChipsModule,
    MatCardModule,
    MatTabsModule,
    MatCheckboxModule,
    MatListModule,
    MatDialogModule,
    CoreModule,
    HRModule,
    FleetModule,
    // Standalone procurement components
    PurchaseOrderListComponent,
    PurchaseOrderFormComponent,
    GoodsReceivingFormComponent,
    SupplierListComponent,
    SupplierFormComponent,
    ApprovalDashboardComponent,
    // Standalone role-based dashboard components
    AdminDashboardComponent,
    ManagerDashboardComponent,
    AccountingDashboardComponent,
    ClerkDashboardComponent,
    TeamLeaderDashboardComponent,
    DriverDashboardComponent,
    WorkerDashboardComponent,
    DefaultDashboardComponent
  ],
  declarations: [
    DashboardComponent,
    DashboardRouterComponent,
    UserProfileComponent,
    InventoryListComponent,
    InventoryFormComponent,
    InventoryDashboardComponent,
    ProcurementDashboardComponent,
    EmployeeDashboardComponent,
    TasksDashboardComponent,
    UserManagementComponent,
    PermissionsManagementComponent,
    ReferenceDataManagerComponent,
    ReplaceReferenceDialogComponent
  ]
})

export class AdminLayoutModule {}
