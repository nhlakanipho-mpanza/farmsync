import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HasPermissionDirective } from './directives/has-permission.directive';
import { HasRoleDirective } from './directives/has-role.directive';

@NgModule({
  declarations: [
    HasPermissionDirective,
    HasRoleDirective
  ],
  imports: [
    CommonModule
  ],
  exports: [
    HasPermissionDirective,
    HasRoleDirective
  ]
})
export class CoreModule { }
