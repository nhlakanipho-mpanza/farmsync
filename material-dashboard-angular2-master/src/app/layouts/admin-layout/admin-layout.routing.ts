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
    { path: 'table-list',     component: TableListComponent },
    { path: 'typography',     component: TypographyComponent },
    { path: 'icons',          component: IconsComponent },
    { path: 'maps',           component: MapsComponent },
    { path: 'notifications',  component: NotificationsComponent },
    { path: 'upgrade',        component: UpgradeComponent },
];
