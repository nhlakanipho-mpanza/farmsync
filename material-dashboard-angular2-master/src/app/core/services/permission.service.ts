import { Injectable } from '@angular/core';
import { AuthService } from './auth.service';
import { MODULE_PERMISSIONS, ROLE_HIERARCHY } from '../constants/permission.constants';

@Injectable({
  providedIn: 'root'
})
export class PermissionService {
  constructor(private authService: AuthService) {}

  /**
   * Check if the current user has a specific permission
   * @param module - The module name (e.g., 'procurement')
   * @param subModule - The sub-module name (e.g., 'purchase_orders')
   * @param action - The action (e.g., 'approve')
   * @returns true if user has permission
   */
  hasPermission(module: string, subModule: string, action: string): boolean {
    const currentUser = this.authService.currentUserValue;
    if (!currentUser || !currentUser.roles || currentUser.roles.length === 0) {
      return false;
    }

    // Admin has all permissions
    if (currentUser.roles.includes('Admin')) {
      return true;
    }

    // Check if permission exists in MODULE_PERMISSIONS
    if (!MODULE_PERMISSIONS[module] || !MODULE_PERMISSIONS[module][subModule]) {
      return false;
    }

    const allowedRoles = MODULE_PERMISSIONS[module][subModule][action];
    if (!allowedRoles) {
      return false;
    }

    // Check if user has any of the allowed roles
    return currentUser.roles.some(role => allowedRoles.includes(role));
  }

  /**
   * Check permission using dot notation string
   * @param permission - Permission string like 'procurement.purchase_orders.approve'
   * @returns true if user has permission
   */
  hasPermissionByString(permission: string): boolean {
    const parts = permission.split('.');
    if (parts.length !== 3) {
      console.warn(`Invalid permission format: ${permission}. Expected format: 'module.subModule.action'`);
      return false;
    }
    return this.hasPermission(parts[0], parts[1], parts[2]);
  }

  /**
   * Check if user has any of the provided permissions (OR logic)
   * @param permissions - Array of permission strings
   * @returns true if user has at least one permission
   */
  hasAnyPermission(permissions: string[]): boolean {
    return permissions.some(permission => this.hasPermissionByString(permission));
  }

  /**
   * Check if user can approve a specific entity type
   * @param entityType - The entity type (e.g., 'purchase_order', 'goods_received')
   * @returns true if user can approve
   */
  canApprove(entityType: string): boolean {
    const approvalMap: { [key: string]: string } = {
      'purchase_order': 'procurement.purchase_orders.approve',
      'goods_received': 'procurement.goods_receiving.approve',
      'expense': 'finance.expenses.approve',
      'task_completion': 'workforce.tasks.approve_completion',
      'inventory_issue': 'inventory.issuing.approve_issue',
      'fuel_log': 'fleet.fuel_logs.approve',
      'maintenance': 'fleet.maintenance.approve'
    };

    const permission = approvalMap[entityType];
    if (!permission) {
      return false;
    }

    return this.hasPermissionByString(permission);
  }

  /**
   * Get the primary role for the current user (highest in hierarchy)
   * @returns The primary role name
   */
  getPrimaryRole(): string | null {
    const currentUser = this.authService.currentUserValue;
    console.log('[PermissionService] Current user:', currentUser);
    
    if (!currentUser || !currentUser.roles || currentUser.roles.length === 0) {
      console.warn('[PermissionService] No user or roles found');
      return null;
    }

    console.log('[PermissionService] User roles:', currentUser.roles);

    let primaryRole: string | null = null;
    let highestLevel = -1;

    for (const role of currentUser.roles) {
      const level = ROLE_HIERARCHY[role];
      console.log('[PermissionService] Role:', role, 'Level:', level);
      
      if (level !== undefined && level > highestLevel) {
        highestLevel = level;
        primaryRole = role;
      }
    }

    console.log('[PermissionService] Primary role determined:', primaryRole, 'Level:', highestLevel);
    return primaryRole;
  }

  /**
   * Get all permissions for the current user
   * @returns Array of permission strings
   */
  getUserPermissions(): string[] {
    const currentUser = this.authService.currentUserValue;
    if (!currentUser || !currentUser.roles || currentUser.roles.length === 0) {
      return [];
    }

    const permissions: string[] = [];

    // Admin has all permissions
    if (currentUser.roles.includes('Admin')) {
      return ['*'];
    }

    // Collect all permissions from all modules
    for (const module in MODULE_PERMISSIONS) {
      for (const subModule in MODULE_PERMISSIONS[module]) {
        for (const action in MODULE_PERMISSIONS[module][subModule]) {
          const allowedRoles = MODULE_PERMISSIONS[module][subModule][action];
          if (currentUser.roles.some(role => allowedRoles.includes(role))) {
            permissions.push(`${module}.${subModule}.${action}`);
          }
        }
      }
    }

    return permissions;
  }

  /**
   * Check if user can view a specific module
   * @param module - The module name
   * @returns true if user has any permission in the module
   */
  canViewModule(module: string): boolean {
    const currentUser = this.authService.currentUserValue;
    if (!currentUser || !currentUser.roles || currentUser.roles.length === 0) {
      return false;
    }

    // Admin can view all modules
    if (currentUser.roles.includes('Admin')) {
      return true;
    }

    // Check if user has any permission in this module
    const moduleConfig = MODULE_PERMISSIONS[module];
    if (!moduleConfig) {
      return false;
    }

    return Object.keys(moduleConfig).some(subModule => {
      return Object.keys(moduleConfig[subModule]).some(action => {
        const allowedRoles = moduleConfig[subModule][action];
        return currentUser.roles.some(role => allowedRoles.includes(role));
      });
    });
  }

  /**
   * Legacy method for backwards compatibility with old permission directive
   * @param permission - Permission string
   * @returns true if user has permission
   */
  can(permission: string): boolean {
    return this.hasPermissionByString(permission);
  }

  /**
   * Check if user has a specific role
   * @param role - Role name or array of role names
   * @returns true if user has the role(s)
   */
  hasRole(role: string | string[]): boolean {
    const currentUser = this.authService.currentUserValue;
    if (!currentUser || !currentUser.roles) {
      return false;
    }

    if (Array.isArray(role)) {
      return role.some(r => currentUser.roles?.includes(r));
    }
    
    return currentUser.roles.includes(role);
  }
}
