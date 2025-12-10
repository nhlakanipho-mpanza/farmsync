import { Routes } from '@angular/router';

import { DashboardComponent } from '../../dashboard/dashboard.component';
import { UserProfileComponent } from '../../user-profile/user-profile.component';
// Role-based Dashboard Imports
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
import { RoleGuard } from 'app/core/guards/role.guard';
import { UserRole } from 'app/core/models/auth.model';
import { PurchaseOrderListComponent } from 'app/procurement/purchase-order-list/purchase-order-list.component';
import { PurchaseOrderFormComponent } from 'app/procurement/purchase-order-form/purchase-order-form.component';
import { PurchaseOrderViewComponent } from 'app/procurement/purchase-order-view/purchase-order-view.component';
import { GoodsReceivingFormComponent } from 'app/procurement/goods-receiving-form/goods-receiving-form.component';
import { GoodsReceivedViewComponent } from 'app/procurement/goods-received-view/goods-received-view.component';
import { SupplierListComponent } from 'app/procurement/supplier-list/supplier-list.component';
import { SupplierFormComponent } from 'app/procurement/supplier-form/supplier-form.component';
import { ApprovalDashboardComponent } from 'app/procurement/approval-dashboard/approval-dashboard.component';
import { ProcurementDashboardComponent } from 'app/procurement/procurement-dashboard/procurement-dashboard.component';
// HR Imports
import { EmployeeListComponent } from 'app/hr/employees/employee-list.component';
import { EmployeeFormComponent } from 'app/hr/employees/employee-form.component';
import { EmployeeDetailComponent } from 'app/hr/employees/employee-detail.component';
import { EmployeeDashboardComponent } from 'app/hr/employee-dashboard/employee-dashboard.component';
import { TeamListComponent } from 'app/hr/teams/team-list.component';
import { TeamFormComponent } from 'app/hr/teams/team-form.component';
import { AssignMembersComponent } from 'app/hr/teams/assign-members.component';
import { TaskListComponent } from 'app/hr/tasks/task-list.component';
import { TaskFormComponent } from 'app/hr/tasks/task-form.component';
import { TaskTemplateListComponent } from 'app/hr/tasks/task-template-list.component';
import { TaskTemplateFormComponent } from 'app/hr/tasks/task-template-form.component';
import { TaskDetailComponent } from 'app/hr/tasks/task-detail.component';
import { TasksDashboardComponent } from 'app/hr/tasks-dashboard/tasks-dashboard.component';
import { AttendanceDashboardComponent } from 'app/hr/attendance/attendance-dashboard.component';
import { IssuingDashboardComponent } from 'app/hr/issuing/issuing-dashboard.component';
import { InventoryIssueFormComponent } from 'app/hr/issuing/inventory-issue-form/inventory-issue-form.component';
import { EquipmentIssueFormComponent } from 'app/hr/issuing/equipment-issue-form/equipment-issue-form.component';
// Fleet Imports
import { FleetDashboardComponent } from 'app/fleet/dashboard/fleet-dashboard.component';
import { VehicleListComponent } from 'app/fleet/vehicles/vehicle-list.component';
import { VehicleFormComponent } from 'app/fleet/vehicles/vehicle-form.component';
import { VehicleDetailComponent } from 'app/fleet/vehicles/vehicle-detail.component';
import { GPSTrackingMapComponent } from 'app/fleet/gps/gps-tracking-map.component';
import { DriverAssignmentListComponent } from 'app/fleet/driver-assignment/driver-assignment-list/driver-assignment-list.component';
import { DriverAssignmentFormComponent } from 'app/fleet/driver-assignment/driver-assignment-form/driver-assignment-form.component';
// Admin Imports
import { UserManagementComponent } from 'app/admin/user-management/user-management.component';
import { PermissionsManagementComponent } from 'app/admin/permissions-management/permissions-management.component';
import { ReferenceDataManagerComponent } from 'app/admin/reference-data-manager/reference-data-manager.component';
// Reports Import
import { ReportsComponent } from 'app/reports/reports.component';

export const AdminLayoutRoutes: Routes = [
    // Role-based Dashboard Routes (no guards - DashboardRouter handles auth)
    { path: 'dashboard', component: DashboardRouterComponent },
    { path: 'dashboard/admin', component: AdminDashboardComponent },
    { path: 'dashboard/manager', component: ManagerDashboardComponent },
    { path: 'dashboard/accounting', component: AccountingDashboardComponent },
    { path: 'dashboard/clerk', component: ClerkDashboardComponent },
    { path: 'dashboard/team-leader', component: TeamLeaderDashboardComponent },
    { path: 'dashboard/driver', component: DriverDashboardComponent },
    { path: 'dashboard/worker', component: WorkerDashboardComponent },
    { path: 'dashboard/default', component: DefaultDashboardComponent },
    { path: 'user-profile', component: UserProfileComponent },
    
    // Inventory routes
    { 
      path: 'inventory/dashboard',      
      component: InventoryDashboardComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.OperationsManager, UserRole.OperationsClerk] }
    },
    { 
      path: 'inventory',      
      component: InventoryListComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.OperationsManager, UserRole.OperationsClerk] }
    },
    { 
      path: 'inventory/create', 
      component: InventoryFormComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.OperationsManager] }
    },
    { 
      path: 'inventory/edit/:id', 
      component: InventoryFormComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.OperationsManager] }
    },
    
    // Procurement routes
    { 
      path: 'procurement/dashboard',      
      component: ProcurementDashboardComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.AccountingManager, UserRole.OperationsManager] }
    },
    { 
      path: 'procurement/suppliers',      
      component: SupplierListComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.AccountingManager] }
    },
    { 
      path: 'procurement/suppliers/new', 
      component: SupplierFormComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.AccountingManager] }
    },
    { 
      path: 'procurement/suppliers/edit/:id', 
      component: SupplierFormComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.AccountingManager] }
    },
    { 
      path: 'procurement/purchase-orders',      
      component: PurchaseOrderListComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.AccountingManager, UserRole.OperationsManager] }
    },
    { 
      path: 'procurement/purchase-orders/new', 
      component: PurchaseOrderFormComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.AccountingManager] }
    },
    { 
      path: 'procurement/purchase-orders/edit/:id', 
      component: PurchaseOrderFormComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.AccountingManager] }
    },
    { 
      path: 'procurement/purchase-orders/view/:id', 
      component: PurchaseOrderViewComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.AccountingManager, UserRole.OperationsManager] }
    },
    { 
      path: 'procurement/receive-goods', 
      component: GoodsReceivingFormComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.OperationsClerk, UserRole.OperationsManager] }
    },
    { 
      path: 'procurement/goods-received/view/:id', 
      component: GoodsReceivedViewComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.OperationsManager, UserRole.OperationsManager] }
    },
    { 
      path: 'procurement/approvals', 
      component: ApprovalDashboardComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.OperationsManager] }
    },
    // HR Routes - Employees Section
    { 
      path: 'hr/dashboard',      
      component: EmployeeDashboardComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.Admin, UserRole.OperationsManager] }
    },
    { 
      path: 'hr/employees',      
      component: EmployeeListComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.Admin, UserRole.OperationsManager] }
    },
    { 
      path: 'hr/employees/create', 
      component: EmployeeFormComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.Admin] }
    },
    { 
      path: 'hr/employees/edit/:id', 
      component: EmployeeFormComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.Admin] }
    },
    { 
      path: 'hr/employees/:id', 
      component: EmployeeDetailComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.Admin, UserRole.OperationsManager] }
    },
    { 
      path: 'hr/teams',      
      component: TeamListComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.OperationsManager] }
    },
    { 
      path: 'hr/teams/create', 
      component: TeamFormComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.OperationsManager] }
    },
    { 
      path: 'hr/teams/edit/:id', 
      component: TeamFormComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.OperationsManager] }
    },
    { 
      path: 'hr/teams/:id/assign', 
      component: AssignMembersComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.OperationsManager] }
    },
    // HR Routes - Tasks Section
    { 
      path: 'hr/tasks/dashboard',      
      component: TasksDashboardComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.Admin, UserRole.OperationsManager] }
    },
    { 
      path: 'hr/tasks',      
      component: TaskListComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.OperationsManager] }
    },
    { 
      path: 'hr/tasks/create', 
      component: TaskFormComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.OperationsManager] }
    },
    { 
      path: 'hr/tasks/detail/:id', 
      component: TaskDetailComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.OperationsManager] }
    },
    { 
      path: 'hr/tasks/edit/:id', 
      component: TaskFormComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.OperationsManager] }
    },
    // Task Templates
    { 
      path: 'hr/task-templates',      
      component: TaskTemplateListComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.OperationsManager] }
    },
    { 
      path: 'hr/task-templates/new', 
      component: TaskTemplateFormComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.OperationsManager] }
    },
    { 
      path: 'hr/task-templates/edit/:id', 
      component: TaskTemplateFormComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.OperationsManager] }
    },
    { 
      path: 'hr/attendance',      
      component: AttendanceDashboardComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.OperationsManager, UserRole.Admin] }
    },
    { 
      path: 'hr/issuing',      
      component: IssuingDashboardComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.OperationsManager] }
    },
    { 
      path: 'hr/issuing/inventory/create',      
      component: InventoryIssueFormComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.OperationsManager] }
    },
    { 
      path: 'hr/issuing/equipment/create',      
      component: EquipmentIssueFormComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.OperationsManager] }
    },
    // Fleet Routes
    { 
      path: 'fleet/dashboard', 
      component: FleetDashboardComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.OperationsManager] }
    },
    { 
      path: 'fleet/vehicles', 
      component: VehicleListComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.OperationsManager] }
    },
    { 
      path: 'fleet/vehicles/create', 
      component: VehicleFormComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.OperationsManager] }
    },
    { 
      path: 'fleet/vehicles/edit/:id', 
      component: VehicleFormComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.OperationsManager] }
    },
    { 
      path: 'fleet/vehicles/:id', 
      component: VehicleDetailComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.OperationsManager] }
    },
    { 
      path: 'fleet/gps', 
      component: GPSTrackingMapComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.OperationsManager] }
    },
    { 
      path: 'fleet/assignments', 
      component: DriverAssignmentListComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.OperationsManager] }
    },
    { 
      path: 'fleet/assignments/create', 
      component: DriverAssignmentFormComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.OperationsManager] }
    },
    
    // Reports Routes
    { 
      path: 'reports',
      loadChildren: () => import('../../reports/reports.module').then(m => m.ReportsModule),
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.AccountingManager, UserRole.OperationsManager] }
    },
    
    // Administration Routes
    { 
      path: 'admin/users',      
      component: UserManagementComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.Admin] }
    },
    { 
      path: 'admin/permissions',      
      component: PermissionsManagementComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin] }
    },
    { 
      path: 'admin/reference-data',      
      component: ReferenceDataManagerComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin] }
    }
];
