import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatDialogModule } from '@angular/material/dialog';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';

import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatRadioModule } from '@angular/material/radio';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { DragDropModule } from '@angular/cdk/drag-drop';

// Shared Components
import { ComponentsModule } from '../components/components.module';

// Employee Components
import { EmployeeListComponent } from './employees/employee-list.component';
import { EmployeeFormComponent } from './employees/employee-form.component';
import { EmployeeDetailComponent } from './employees/employee-detail.component';

// Team Components
import { TeamListComponent } from './teams/team-list.component';
import { TeamFormComponent } from './teams/team-form.component';
import { AssignMembersComponent } from './teams/assign-members.component';

// Task Components
import { TaskListComponent } from './tasks/task-list.component';
import { TaskFormComponent } from './tasks/task-form.component';
import { TaskTemplateListComponent } from './tasks/task-template-list.component';
import { TaskTemplateFormComponent } from './tasks/task-template-form.component';
import { TaskDetailComponent } from './tasks/task-detail.component';

// Attendance Components
import { AttendanceDashboardComponent } from './attendance/attendance-dashboard.component';

// Issuing Components
import { IssuingDashboardComponent } from './issuing/issuing-dashboard.component';
import { InventoryIssueFormComponent } from './issuing/inventory-issue-form/inventory-issue-form.component';
import { EquipmentIssueFormComponent } from './issuing/equipment-issue-form/equipment-issue-form.component';

@NgModule({
  declarations: [
    // Employee Components
    EmployeeListComponent,
    EmployeeFormComponent,
    EmployeeDetailComponent,
    // Team Components
    TeamListComponent,
    TeamFormComponent,
    AssignMembersComponent,
    // Task Components
    TaskListComponent,
    TaskFormComponent,
    TaskTemplateListComponent,
    TaskTemplateFormComponent,
    TaskDetailComponent,
    // Attendance Components
    AttendanceDashboardComponent,
    // Issuing Components
    IssuingDashboardComponent,
    InventoryIssueFormComponent,
    EquipmentIssueFormComponent
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
    MatIconModule,
    MatSnackBarModule,
    MatProgressSpinnerModule,
    MatSlideToggleModule,
    MatTooltipModule,
    MatDialogModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatButtonToggleModule,
    MatRadioModule,
    MatCheckboxModule,
    DragDropModule,
    ComponentsModule
  ]
})
export class HRModule { }
