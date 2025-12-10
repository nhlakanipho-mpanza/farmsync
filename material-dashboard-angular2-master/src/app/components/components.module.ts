import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatBadgeModule } from '@angular/material/badge';
import { MatTooltipModule } from '@angular/material/tooltip';

import { FooterComponent } from './footer/footer.component';
import { NavbarComponent } from './navbar/navbar.component';
import { SidebarComponent } from './sidebar/sidebar.component';
import { DocumentUploadComponent } from './document-upload/document-upload.component';
import { DocumentListComponent } from './document-list/document-list.component';
import { NotificationBellComponent } from './notification-bell/notification-bell.component';

@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    MatIconModule,
    MatButtonModule,
    MatSelectModule,
    MatFormFieldModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatInputModule,
    MatProgressSpinnerModule,
    MatSnackBarModule,
    MatBadgeModule,
    MatTooltipModule
  ],
  declarations: [
    FooterComponent,
    NavbarComponent,
    SidebarComponent,
    DocumentUploadComponent,
    DocumentListComponent,
    NotificationBellComponent
  ],
  exports: [
    FooterComponent,
    NavbarComponent,
    SidebarComponent,
    DocumentUploadComponent,
    DocumentListComponent,
    NotificationBellComponent
  ]
})
export class ComponentsModule { }
