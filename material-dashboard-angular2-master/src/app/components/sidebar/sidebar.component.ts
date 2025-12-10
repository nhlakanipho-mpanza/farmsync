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
    children?: RouteInfo[]; // Child menu items for hierarchical navigation
    isCollapsible?: boolean; // Whether this item can expand/collapse
    isExpanded?: boolean; // Current expanded state
    isDashboard?: boolean; // Whether this is a dashboard link
}

export const ROUTES: RouteInfo[] = [
    { 
      path: '/dashboard', 
      title: 'Dashboard',  
      icon: 'dashboard', 
      class: '',
      roles: [UserRole.Admin, UserRole.OperationsManager, UserRole.AccountingManager, UserRole.OperationsClerk, UserRole.Accountant, UserRole.TeamLeader, UserRole.Driver, UserRole.GeneralWorker]
    },
    { 
      path: '/user-profile', 
      title: 'User Profile',  
      icon:'person', 
      class: '',
      roles: [UserRole.Admin, UserRole.OperationsManager, UserRole.AccountingManager, UserRole.OperationsClerk, UserRole.Accountant, UserRole.TeamLeader, UserRole.Driver, UserRole.GeneralWorker]
    },
    // Inventory Module
    { 
      path: '/inventory',
      title: 'Inventory',  
      icon:'inventory_2', 
      class: '',
      isCollapsible: true,
      isExpanded: false,
      roles: [UserRole.Admin, UserRole.OperationsManager, UserRole.OperationsClerk],
      children: [
        { 
          path: '/inventory', 
          title: 'Items',  
          icon:'list', 
          class: '',
          roles: [UserRole.Admin, UserRole.OperationsManager, UserRole.OperationsClerk]
        },
        { 
          path: '/inventory/create', 
          title: 'Add Item',  
          icon:'add_circle', 
          class: '',
          roles: [UserRole.Admin, UserRole.OperationsManager]
        }
      ]
    },
    // Procurement Module
    { 
      path: '/procurement',
      title: 'Procurement',  
      icon:'shopping_cart', 
      class: '',
      isCollapsible: true,
      isExpanded: false,
      roles: [UserRole.Admin, UserRole.AccountingManager, UserRole.OperationsManager, UserRole.OperationsClerk, UserRole.Accountant],
      children: [
        { 
          path: '/procurement/suppliers', 
          title: 'Suppliers',  
          icon:'business', 
          class: '',
          roles: [UserRole.Admin, UserRole.AccountingManager, UserRole.Accountant]
        },
        { 
          path: '/procurement/purchase-orders', 
          title: 'Purchase Orders',  
          icon:'receipt_long', 
          class: '',
          roles: [UserRole.Admin, UserRole.AccountingManager, UserRole.OperationsManager, UserRole.Accountant]
        },
        { 
          path: '/procurement/receive-goods', 
          title: 'Receive Goods',  
          icon:'local_shipping', 
          class: '',
          roles: [UserRole.Admin, UserRole.OperationsClerk, UserRole.OperationsManager]
        },
        { 
          path: '/procurement/approvals', 
          title: 'Approvals',  
          icon:'approval', 
          class: '',
          roles: [UserRole.Admin, UserRole.OperationsManager, UserRole.AccountingManager]
        }
      ]
    },
    // Workforce Module  
    { 
      path: '/hr',
      title: 'Workforce',  
      icon:'people', 
      class: '',
      isCollapsible: true,
      isExpanded: false,
      roles: [UserRole.Admin, UserRole.OperationsManager, UserRole.TeamLeader, UserRole.OperationsClerk],
      children: [
        { 
          path: '/hr/employees', 
          title: 'Employees',  
          icon:'badge', 
          class: '',
          roles: [UserRole.Admin, UserRole.OperationsManager]
        },
        { 
          path: '/hr/teams', 
          title: 'Teams',  
          icon:'groups', 
          class: '',
          roles: [UserRole.Admin, UserRole.OperationsManager, UserRole.TeamLeader]
        },
        { 
          path: '/hr/tasks', 
          title: 'Tasks',  
          icon:'assignment', 
          class: '',
          roles: [UserRole.Admin, UserRole.OperationsManager, UserRole.TeamLeader]
        },
        { 
          path: '/hr/task-templates', 
          title: 'Task Templates',  
          icon:'library_books', 
          class: '',
          roles: [UserRole.Admin, UserRole.OperationsManager]
        },
        { 
          path: '/hr/attendance', 
          title: 'Attendance',  
          icon:'access_time', 
          class: '',
          roles: [UserRole.Admin, UserRole.OperationsManager, UserRole.OperationsClerk]
        },
        { 
          path: '/hr/issuing', 
          title: 'Issuing',  
          icon:'swap_horiz', 
          class: '',
          roles: [UserRole.Admin, UserRole.OperationsManager, UserRole.OperationsClerk, UserRole.TeamLeader]
        }
      ]
    },
    // Fleet Module
    { 
      path: '/fleet',
      title: 'Fleet',  
      icon:'directions_car', 
      class: '',
      isCollapsible: true,
      isExpanded: false,
      roles: [UserRole.Admin, UserRole.OperationsManager, UserRole.Driver],
      children: [
        { 
          path: '/fleet/vehicles', 
          title: 'Vehicles',  
          icon:'local_shipping', 
          class: '',
          roles: [UserRole.Admin, UserRole.OperationsManager, UserRole.Driver]
        },
        { 
          path: '/fleet/assignments', 
          title: 'Driver Assignments',  
          icon:'assignment_ind', 
          class: '',
          roles: [UserRole.Admin, UserRole.OperationsManager]
        },
        { 
          path: '/fleet/gps', 
          title: 'GPS Tracking',  
          icon:'map', 
          class: '',
          roles: [UserRole.Admin, UserRole.OperationsManager]
        }
      ]
    },
    // Reports Module
    { 
      path: '/reports', 
      title: 'Reports',  
      icon:'assessment', 
      class: '',
      isCollapsible: true,
      roles: [UserRole.Admin, UserRole.AccountingManager, UserRole.OperationsManager, UserRole.Accountant],
      children: [
        {
          path: '/reports/purchase-orders',
          title: 'Purchase Orders',
          icon: 'shopping_cart',
          class: '',
          roles: [UserRole.Admin, UserRole.AccountingManager, UserRole.OperationsManager, UserRole.Accountant]
        },
        {
          path: '/reports/goods-received',
          title: 'Goods Received',
          icon: 'inventory_2',
          class: '',
          roles: [UserRole.Admin, UserRole.AccountingManager, UserRole.OperationsManager]
        },
        {
          path: '/reports/inventory-valuation',
          title: 'Inventory Valuation',
          icon: 'inventory',
          class: '',
          roles: [UserRole.Admin, UserRole.OperationsManager, UserRole.Accountant]
        },
        {
          path: '/reports/supplier-transactions',
          title: 'Supplier Transactions',
          icon: 'local_shipping',
          class: '',
          roles: [UserRole.Admin, UserRole.AccountingManager, UserRole.Accountant]
        },
        {
          path: '/reports/expenses',
          title: 'Expenses',
          icon: 'receipt',
          class: '',
          roles: [UserRole.Admin, UserRole.AccountingManager, UserRole.Accountant]
        }
      ]
    },
    // Administration Module
    { 
      path: '/admin',
      title: 'Administration',  
      icon:'settings', 
      class: '',
      isCollapsible: true,
      isExpanded: false,
      roles: [UserRole.Admin],
      children: [
        { 
          path: '/admin/users', 
          title: 'User Management',  
          icon:'people', 
          class: '',
          roles: [UserRole.Admin]
        },
        { 
          path: '/admin/permissions', 
          title: 'Permissions',  
          icon:'security', 
          class: '',
          roles: [UserRole.Admin]
        },
        { 
          path: '/admin/reference-data', 
          title: 'Reference Data',  
          icon:'settings_applications', 
          class: '',
          roles: [UserRole.Admin]
        }
      ]
    }
];

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {
  menuItems: any[];
  currentUser: User | null;
  expandedSections: { [key: string]: boolean } = {};

  constructor(private authService: AuthService) { }

  ngOnInit() {
    // Load expanded state from localStorage
    const savedState = localStorage.getItem('sidebar_expanded_state');
    if (savedState) {
      this.expandedSections = JSON.parse(savedState);
    }

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

    this.menuItems = ROUTES.filter(menuItem => this.hasAccess(menuItem))
      .map(item => {
        // Filter children if they exist
        if (item.children && item.children.length > 0) {
          const filteredChildren = item.children.filter(child => this.hasAccess(child));
          
          // Restore expanded state from localStorage or keep current state
          const isExpanded = this.expandedSections[item.path] !== undefined 
            ? this.expandedSections[item.path] 
            : false;
          
          return {
            ...item,
            children: filteredChildren,
            isExpanded: isExpanded
          };
        }
        return item;
      });
  }

  hasAccess(menuItem: RouteInfo): boolean {
    // If no roles specified, show to everyone
    if (!menuItem.roles || menuItem.roles.length === 0) {
      return true;
    }
    
    // Check if user is logged in and has roles
    if (!this.currentUser || !this.currentUser.roles || this.currentUser.roles.length === 0) {
      return false;
    }
    
    // Check if user has any of the required roles
    return menuItem.roles.some(role => this.currentUser.roles.includes(role));
  }

  toggleSection(menuItem: RouteInfo) {
    if (!menuItem.isCollapsible) {
      return;
    }

    const wasExpanded = menuItem.isExpanded;
    
    // Close all other expanded menus
    this.menuItems.forEach(item => {
      if (item.isCollapsible && item !== menuItem) {
        item.isExpanded = false;
        this.expandedSections[item.path] = false;
      }
    });
    
    // Toggle the clicked menu
    menuItem.isExpanded = !wasExpanded;
    this.expandedSections[menuItem.path] = menuItem.isExpanded;
    
    // Save state to localStorage
    localStorage.setItem('sidebar_expanded_state', JSON.stringify(this.expandedSections));
  }

  isChildActive(item: RouteInfo): boolean {
    if (!item.children || item.children.length === 0) {
      return false;
    }
    
    const currentPath = window.location.pathname;
    return item.children.some(child => currentPath.startsWith(child.path));
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
