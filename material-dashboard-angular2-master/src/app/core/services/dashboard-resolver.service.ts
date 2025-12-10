import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { PermissionService } from './permission.service';
import { ROLE_DASHBOARD_ROUTES } from '../constants/permission.constants';

/**
 * Service to route users to their role-specific dashboard
 */
@Injectable({
  providedIn: 'root'
})
export class DashboardResolverService {
  constructor(
    private permissionService: PermissionService,
    private router: Router
  ) {}

  /**
   * Navigate to appropriate dashboard based on user's primary role
   */
  navigateToDashboard(): void {
    const primaryRole = this.permissionService.getPrimaryRole();
    
    console.log('[DashboardResolver] Primary role:', primaryRole);
    
    if (!primaryRole) {
      console.error('[DashboardResolver] No role found for user');
      this.router.navigate(['/login']);
      return;
    }

    const dashboardRoute = ROLE_DASHBOARD_ROUTES[primaryRole];
    
    console.log('[DashboardResolver] Dashboard route for role', primaryRole, ':', dashboardRoute);
    
    if (!dashboardRoute) {
      console.warn(`[DashboardResolver] No dashboard route configured for role: ${primaryRole}`);
      // Fallback to generic dashboard
      this.router.navigate(['/dashboard/default']);
      return;
    }

    console.log('[DashboardResolver] Navigating to:', dashboardRoute);
    this.router.navigate([dashboardRoute]);
  }

  /**
   * Get dashboard route for a specific role
   */
  getDashboardRoute(role: string): string {
    return ROLE_DASHBOARD_ROUTES[role] || '/dashboard/default';
  }

  /**
   * Get dashboard route for current user's primary role
   */
  getCurrentUserDashboardRoute(): string {
    const primaryRole = this.permissionService.getPrimaryRole();
    return primaryRole ? this.getDashboardRoute(primaryRole) : '/login';
  }
}
