import { Component, OnInit } from '@angular/core';
import { AuthService } from 'app/core/services/auth.service';
import { User, UserRole } from 'app/core/models/auth.model';

declare const $: any;
declare interface RouteInfo {
    path: string;
    title: string;
    icon: string;
    class: string;
    roles?: string[]; // Roles that can access this route
}

export const ROUTES: RouteInfo[] = [
    { 
      path: '/dashboard', 
      title: 'Dashboard',  
      icon: 'dashboard', 
      class: '',
      roles: [UserRole.Admin, UserRole.Accountant, UserRole.Operations, UserRole.HR, UserRole.Manager, UserRole.StoreClerk]
    },
    { 
      path: '/inventory', 
      title: 'Inventory',  
      icon:'inventory_2', 
      class: '',
      roles: [UserRole.Admin, UserRole.Operations, UserRole.StoreClerk]
    },
    { 
      path: '/procurement/suppliers', 
      title: 'Suppliers',  
      icon:'business', 
      class: '',
      roles: [UserRole.Admin, UserRole.Accountant]
    },
    { 
      path: '/procurement/purchase-orders', 
      title: 'Purchase Orders',  
      icon:'shopping_cart', 
      class: '',
      roles: [UserRole.Admin, UserRole.Accountant, UserRole.Manager]
    },
    { 
      path: '/procurement/receive-goods', 
      title: 'Receive Goods',  
      icon:'local_shipping', 
      class: '',
      roles: [UserRole.Admin, UserRole.StoreClerk, UserRole.Operations]
    },
    { 
      path: '/procurement/approvals', 
      title: 'Approvals',  
      icon:'approval', 
      class: '',
      roles: [UserRole.Admin, UserRole.Manager]
    },
    { 
      path: '/users', 
      title: 'User Management',  
      icon:'people', 
      class: '',
      roles: [UserRole.Admin, UserRole.HR]
    },
    { 
      path: '/reports', 
      title: 'Reports',  
      icon:'assessment', 
      class: '',
      roles: [UserRole.Admin, UserRole.Accountant, UserRole.Manager]
    },
    { 
      path: '/user-profile', 
      title: 'User Profile',  
      icon:'person', 
      class: '',
      roles: [UserRole.Admin, UserRole.Accountant, UserRole.Operations, UserRole.HR, UserRole.Manager, UserRole.StoreClerk]
    },
];

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {
  menuItems: any[];
  currentUser: User | null;

  constructor(private authService: AuthService) { }

  ngOnInit() {
    this.authService.currentUser.subscribe(user => {
      this.currentUser = user;
      this.filterMenuItemsByRole();
    });
  }

  filterMenuItemsByRole() {
    if (!this.currentUser) {
      this.menuItems = [];
      return;
    }

    this.menuItems = ROUTES.filter(menuItem => {
      // If no roles specified, show to everyone
      if (!menuItem.roles || menuItem.roles.length === 0) {
        return true;
      }
      // Check if user has any of the required roles
      return menuItem.roles.some(role => this.currentUser.roles.includes(role));
    });
  }

  logout() {
    this.authService.logout();
  }

  isMobileMenu() {
      if ($(window).width() > 991) {
          return false;
      }
      return true;
  };
}
