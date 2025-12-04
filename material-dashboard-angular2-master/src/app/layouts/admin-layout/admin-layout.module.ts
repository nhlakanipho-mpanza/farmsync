import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AdminLayoutRoutes } from './admin-layout.routing';
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
import { PurchaseOrderListComponent } from 'app/procurement/purchase-order-list/purchase-order-list.component';
import { PurchaseOrderFormComponent } from 'app/procurement/purchase-order-form/purchase-order-form.component';
import { GoodsReceivingFormComponent } from 'app/procurement/goods-receiving-form/goods-receiving-form.component';
import { SupplierListComponent } from 'app/procurement/supplier-list/supplier-list.component';
import { SupplierFormComponent } from 'app/procurement/supplier-form/supplier-form.component';
import { ApprovalDashboardComponent } from 'app/procurement/approval-dashboard/approval-dashboard.component';
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
    PurchaseOrderListComponent,
    PurchaseOrderFormComponent,
    GoodsReceivingFormComponent,
    SupplierListComponent,
    SupplierFormComponent,
    ApprovalDashboardComponent,
  ],
  declarations: [
    DashboardComponent,
    UserProfileComponent,
    TableListComponent,
    TypographyComponent,
    IconsComponent,
    MapsComponent,
    NotificationsComponent,
    UpgradeComponent,
    InventoryListComponent,
    InventoryFormComponent,
  ]
})

export class AdminLayoutModule {}
