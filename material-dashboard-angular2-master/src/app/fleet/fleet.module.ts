import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

// Material Modules
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTabsModule } from '@angular/material/tabs';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatChipsModule } from '@angular/material/chips';
import { MatMenuModule } from '@angular/material/menu';
import { MatBadgeModule } from '@angular/material/badge';

// Fleet Components
import { VehicleListComponent } from './vehicles/vehicle-list.component';
import { VehicleFormComponent } from './vehicles/vehicle-form.component';
import { VehicleDetailComponent } from './vehicles/vehicle-detail.component';
import { GPSTrackingMapComponent } from './gps/gps-tracking-map.component';
import { FleetDashboardComponent } from './dashboard/fleet-dashboard.component';
import { DriverAssignmentFormComponent } from './driver-assignment/driver-assignment-form/driver-assignment-form.component';
import { DriverAssignmentListComponent } from './driver-assignment/driver-assignment-list/driver-assignment-list.component';

@NgModule({
  declarations: [
    VehicleListComponent,
    VehicleFormComponent,
    VehicleDetailComponent,
    GPSTrackingMapComponent,
    FleetDashboardComponent,
    DriverAssignmentFormComponent,
    DriverAssignmentListComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatInputModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatIconModule,
    MatCardModule,
    MatFormFieldModule,
    MatSnackBarModule,
    MatProgressSpinnerModule,
    MatTabsModule,
    MatCheckboxModule,
    MatTooltipModule,
    MatChipsModule,
    MatMenuModule,
    MatBadgeModule
  ]
})
export class FleetModule { }
