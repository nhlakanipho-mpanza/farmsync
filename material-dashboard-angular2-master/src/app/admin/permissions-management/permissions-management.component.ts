import { Component, OnInit } from '@angular/core';

interface Permission {
  id: string;
  name: string;
  description: string;
  module: string;
}

@Component({
  selector: 'app-permissions-management',
  templateUrl: './permissions-management.component.html',
  styleUrls: ['./permissions-management.component.css']
})
export class PermissionsManagementComponent implements OnInit {
  permissions: Permission[] = [];
  groupedPermissions: { [module: string]: Permission[] } = {};
  modules: string[] = [];

  constructor() { }

  ngOnInit() {
    this.loadPermissions();
  }

  loadPermissions() {
    // TODO: Load permissions from backend
    // For now, define system permissions
    this.permissions = [
      { id: '1', name: 'inventory.view', description: 'View inventory items', module: 'Inventory' },
      { id: '2', name: 'inventory.create', description: 'Create inventory items', module: 'Inventory' },
      { id: '3', name: 'inventory.edit', description: 'Edit inventory items', module: 'Inventory' },
      { id: '4', name: 'inventory.delete', description: 'Delete inventory items', module: 'Inventory' },
      { id: '5', name: 'procurement.view', description: 'View procurement records', module: 'Procurement' },
      { id: '6', name: 'procurement.create', description: 'Create purchase orders', module: 'Procurement' },
      { id: '7', name: 'procurement.approve', description: 'Approve purchase orders', module: 'Procurement' },
      { id: '8', name: 'fleet.view', description: 'View fleet vehicles', module: 'Fleet' },
      { id: '9', name: 'fleet.edit', description: 'Edit fleet vehicles', module: 'Fleet' },
      { id: '10', name: 'fleet.assign-driver', description: 'Assign drivers to vehicles', module: 'Fleet' },
      { id: '11', name: 'hr.view', description: 'View employee records', module: 'HR' },
      { id: '12', name: 'hr.create', description: 'Create employee records', module: 'HR' },
      { id: '13', name: 'hr.edit', description: 'Edit employee records', module: 'HR' },
      { id: '14', name: 'admin.users', description: 'Manage system users', module: 'Administration' },
      { id: '15', name: 'admin.permissions', description: 'Manage permissions', module: 'Administration' }
    ];

    this.groupPermissionsByModule();
  }

  groupPermissionsByModule() {
    this.groupedPermissions = {};
    this.permissions.forEach(permission => {
      if (!this.groupedPermissions[permission.module]) {
        this.groupedPermissions[permission.module] = [];
      }
      this.groupedPermissions[permission.module].push(permission);
    });
    this.modules = Object.keys(this.groupedPermissions);
  }
}
