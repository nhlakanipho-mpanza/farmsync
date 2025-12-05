import { Routes } from '@angular/router';

import { DashboardComponent } from '../../dashboard/dashboard.component';
import { UserProfileComponent } from '../../user-profile/user-profile.component';
import { TableListComponent } from '../../table-list/table-list.component';
import { TypographyComponent } from '../../typography/typography.component';
import { IconsComponent } from '../../icons/icons.component';
import { MapsComponent } from '../../maps/maps.component';
import { NotificationsComponent } from '../../notifications/notifications.component';
import { UpgradeComponent } from '../../upgrade/upgrade.component';
import { InventoryListComponent } from 'app/inventory/inventory-list/inventory-list.component';
import { InventoryFormComponent } from 'app/inventory/inventory-form/inventory-form.component';
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
// HR Imports
import { EmployeeListComponent } from 'app/hr/employees/employee-list.component';
import { EmployeeFormComponent } from 'app/hr/employees/employee-form.component';
import { EmployeeDetailComponent } from 'app/hr/employees/employee-detail.component';
import { TeamListComponent } from 'app/hr/teams/team-list.component';
import { TeamFormComponent } from 'app/hr/teams/team-form.component';
import { AssignMembersComponent } from 'app/hr/teams/assign-members.component';
import { TaskListComponent } from 'app/hr/tasks/task-list.component';
import { TaskFormComponent } from 'app/hr/tasks/task-form.component';
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

export const AdminLayoutRoutes: Routes = [
    // {
    //   path: '',
    //   children: [ {
    //     path: 'dashboard',
    //     component: DashboardComponent
    // }]}, {
    // path: '',
    // children: [ {
    //   path: 'userprofile',
    //   component: UserProfileComponent
    // }]
    // }, {
    //   path: '',
    //   children: [ {
    //     path: 'icons',
    //     component: IconsComponent
    //     }]
    // }, {
    //     path: '',
    //     children: [ {
    //         path: 'notifications',
    //         component: NotificationsComponent
    //     }]
    // }, {
    //     path: '',
    //     children: [ {
    //         path: 'maps',
    //         component: MapsComponent
    //     }]
    // }, {
    //     path: '',
    //     children: [ {
    //         path: 'typography',
    //         component: TypographyComponent
    //     }]
    // }, {
    //     path: '',
    //     children: [ {
    //         path: 'upgrade',
    //         component: UpgradeComponent
    //     }]
    // }
    { path: 'dashboard',      component: DashboardComponent },
    { path: 'user-profile',   component: UserProfileComponent },
    { 
      path: 'inventory',      
      component: InventoryListComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.Operations] }
    },
    { 
      path: 'inventory/create', 
      component: InventoryFormComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.Operations] }
    },
    { 
      path: 'inventory/edit/:id', 
      component: InventoryFormComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.Operations] }
    },
    // Procurement routes
    { 
      path: 'procurement/suppliers',      
      component: SupplierListComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.Accountant] }
    },
    { 
      path: 'procurement/suppliers/new', 
      component: SupplierFormComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.Accountant] }
    },
    { 
      path: 'procurement/suppliers/edit/:id', 
      component: SupplierFormComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.Accountant] }
    },
    { 
      path: 'procurement/purchase-orders',      
      component: PurchaseOrderListComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.Accountant, UserRole.Manager] }
    },
    { 
      path: 'procurement/purchase-orders/new', 
      component: PurchaseOrderFormComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.Accountant] }
    },
    { 
      path: 'procurement/purchase-orders/edit/:id', 
      component: PurchaseOrderFormComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.Accountant] }
    },
    { 
      path: 'procurement/purchase-orders/view/:id', 
      component: PurchaseOrderViewComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.Accountant, UserRole.Manager] }
    },
    { 
      path: 'procurement/receive-goods', 
      component: GoodsReceivingFormComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.StoreClerk, UserRole.Operations] }
    },
    { 
      path: 'procurement/goods-received/view/:id', 
      component: GoodsReceivedViewComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.Manager, UserRole.Operations] }
    },
    { 
      path: 'procurement/approvals', 
      component: ApprovalDashboardComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.Manager] }
    },
    // HR Routes
    { 
      path: 'hr/employees',      
      component: EmployeeListComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.HR] }
    },
    { 
      path: 'hr/employees/create', 
      component: EmployeeFormComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.HR] }
    },
    { 
      path: 'hr/employees/edit/:id', 
      component: EmployeeFormComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.HR] }
    },
    { 
      path: 'hr/employees/:id', 
      component: EmployeeDetailComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.HR, UserRole.Manager] }
    },
    { 
      path: 'hr/teams',      
      component: TeamListComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.Manager] }
    },
    { 
      path: 'hr/teams/create', 
      component: TeamFormComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.Manager] }
    },
    { 
      path: 'hr/teams/edit/:id', 
      component: TeamFormComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.Manager] }
    },
    { 
      path: 'hr/teams/:id/assign', 
      component: AssignMembersComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.Manager] }
    },
    { 
      path: 'hr/tasks',      
      component: TaskListComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.Manager] }
    },
    { 
      path: 'hr/tasks/create', 
      component: TaskFormComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.Manager] }
    },
    { 
      path: 'hr/tasks/edit/:id', 
      component: TaskFormComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.Manager] }
    },
    { 
      path: 'hr/attendance',      
      component: AttendanceDashboardComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.Manager, UserRole.HR] }
    },
    { 
      path: 'hr/issuing',      
      component: IssuingDashboardComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.Manager] }
    },
    { 
      path: 'hr/issuing/inventory/create',      
      component: InventoryIssueFormComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.Manager] }
    },
    { 
      path: 'hr/issuing/equipment/create',      
      component: EquipmentIssueFormComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.Manager] }
    },
    // Fleet Routes
    { 
      path: 'fleet/dashboard', 
      component: FleetDashboardComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.Manager] }
    },
    { 
      path: 'fleet/vehicles', 
      component: VehicleListComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.Manager] }
    },
    { 
      path: 'fleet/vehicles/create', 
      component: VehicleFormComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.Manager] }
    },
    { 
      path: 'fleet/vehicles/edit/:id', 
      component: VehicleFormComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.Manager] }
    },
    { 
      path: 'fleet/vehicles/:id', 
      component: VehicleDetailComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.Manager] }
    },
    { 
      path: 'fleet/gps', 
      component: GPSTrackingMapComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.Manager] }
    },
    { 
      path: 'fleet/assignments', 
      component: DriverAssignmentListComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.Manager] }
    },
    { 
      path: 'fleet/assignments/create', 
      component: DriverAssignmentFormComponent,
      canActivate: [RoleGuard],
      data: { roles: [UserRole.Admin, UserRole.Manager] }
    },
    { path: 'table-list',     component: TableListComponent },
    { path: 'typography',     component: TypographyComponent },
    { path: 'icons',          component: IconsComponent },
    { path: 'maps',           component: MapsComponent },
    { path: 'notifications',  component: NotificationsComponent },
    { path: 'upgrade',        component: UpgradeComponent },
];
