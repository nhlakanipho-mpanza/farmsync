/**
 * Permission Constants
 * Defines granular permissions for each module and role mappings
 * Role names match backend format (with spaces)
 */

export interface ModulePermissions {
  [action: string]: string[];
}

export interface PermissionMap {
  [module: string]: {
    [subModule: string]: ModulePermissions;
  };
}

export const MODULE_PERMISSIONS: PermissionMap = {
  // INVENTORY MANAGEMENT MODULE
  inventory: {
    items: {
      view: ['Admin', 'Operations Manager', 'Operations Clerk', 'Accountant', 'Accounting Manager'],
      create: ['Admin', 'Operations Manager'],
      edit: ['Admin', 'Operations Manager'],
      delete: ['Admin']
    },
    locations: {
      view: ['Admin', 'Operations Manager', 'Operations Clerk'],
      manage: ['Admin', 'Operations Manager']
    },
    stock_levels: {
      view: ['Admin', 'Operations Manager', 'Operations Clerk'],
      adjust: ['Admin', 'Operations Manager']
    },
    issuing: {
      view: ['Admin', 'Operations Manager', 'Operations Clerk', 'Team Leader'],
      create_request: ['Team Leader', 'Operations Clerk', 'Operations Manager'],
      approve_issue: ['Operations Manager', 'Admin'],
      receive_return: ['Operations Clerk', 'Operations Manager', 'Admin']
    }
  },

  // PROCUREMENT MODULE
  procurement: {
    suppliers: {
      view: ['Admin', 'Accounting Manager', 'Accountant', 'Operations Manager'],
      create: ['Admin', 'Accounting Manager'],
      edit: ['Admin', 'Accounting Manager'],
      delete: ['Admin']
    },
    purchase_orders: {
      view: ['Admin', 'Accounting Manager', 'Accountant', 'Operations Manager'],
      create: ['Admin', 'Accounting Manager', 'Accountant'],
      edit: ['Admin', 'Accounting Manager', 'Accountant'],
      approve: ['Accounting Manager', 'Operations Manager'],
      cancel: ['Admin', 'Accounting Manager']
    },
    goods_receiving: {
      view: ['Admin', 'Operations Manager', 'Operations Clerk'],
      create: ['Admin', 'Operations Clerk', 'Operations Manager'],
      approve: ['Operations Manager', 'Admin'],
      reject: ['Operations Manager', 'Admin']
    }
  },

  // WORKFORCE MANAGEMENT MODULE
  workforce: {
    employees: {
      view: ['Admin', 'Operations Manager', 'Accounting Manager'],
      create: ['Admin', 'Operations Manager'],
      edit: ['Admin', 'Operations Manager'],
      delete: ['Admin']
    },
    teams: {
      view: ['Admin', 'Operations Manager', 'Team Leader'],
      create: ['Admin', 'Operations Manager'],
      manage_members: ['Admin', 'Operations Manager'],
      view_own_team: ['Team Leader']
    },
    tasks: {
      view: ['Admin', 'Operations Manager', 'Team Leader', 'General Worker'],
      create: ['Admin', 'Operations Manager', 'Team Leader'],
      assign: ['Admin', 'Operations Manager', 'Team Leader'],
      update_status: ['Team Leader', 'General Worker', 'Operations Manager'],
      approve_completion: ['Operations Manager', 'Team Leader']
    },
    attendance: {
      view: ['Admin', 'Operations Manager', 'Accounting Manager'],
      clock_in_out: ['Admin', 'Operations Manager', 'Operations Clerk', 'Team Leader', 'Driver', 'General Worker'],
      manage: ['Admin', 'Operations Manager'],
      export_payroll: ['Accounting Manager', 'Admin']
    }
  },

  // FLEET & VEHICLES MODULE
  fleet: {
    vehicles: {
      view: ['Admin', 'Operations Manager', 'Driver', 'Operations Clerk'],
      create: ['Admin', 'Operations Manager'],
      edit: ['Admin', 'Operations Manager'],
      delete: ['Admin']
    },
    driver_assignments: {
      view: ['Admin', 'Operations Manager', 'Driver'],
      create: ['Admin', 'Operations Manager'],
      approve: ['Operations Manager', 'Admin'],
      view_own: ['Driver']
    },
    fuel_logs: {
      view: ['Admin', 'Operations Manager', 'Accounting Manager'],
      create: ['Driver', 'Operations Clerk'],
      approve: ['Operations Manager', 'Admin'],
      edit: ['Admin', 'Operations Manager'],
      view_own: ['Driver']
    },
    maintenance: {
      view: ['Admin', 'Operations Manager', 'Driver'],
      schedule: ['Admin', 'Operations Manager'],
      log_completion: ['Operations Clerk', 'Operations Manager'],
      approve: ['Operations Manager', 'Admin']
    },
    trip_logs: {
      view: ['Admin', 'Operations Manager'],
      create: ['Driver'],
      approve: ['Operations Manager', 'Admin'],
      view_own: ['Driver']
    },
    gps_tracking: {
      view: ['Admin', 'Operations Manager'],
      view_own: ['Driver']
    }
  },

  // FINANCE & ACCOUNTING MODULE
  finance: {
    expenses: {
      view: ['Admin', 'Accounting Manager', 'Accountant'],
      create: ['Accountant', 'Operations Manager', 'Accounting Manager'],
      approve: ['Accounting Manager', 'Admin'],
      reject: ['Accounting Manager', 'Admin']
    },
    budgets: {
      view: ['Admin', 'Accounting Manager', 'Operations Manager'],
      create: ['Admin', 'Accounting Manager'],
      edit: ['Admin', 'Accounting Manager']
    },
    invoices: {
      view: ['Admin', 'Accounting Manager', 'Accountant'],
      create: ['Accountant', 'Accounting Manager'],
      approve: ['Accounting Manager', 'Admin']
    },
    payments: {
      view: ['Admin', 'Accounting Manager'],
      process: ['Accounting Manager'],
      approve: ['Admin']
    }
  },

  // REPORTS & ANALYTICS MODULE
  reports: {
    inventory: {
      view: ['Admin', 'Operations Manager', 'Operations Clerk', 'Accounting Manager'],
      export: ['Admin', 'Operations Manager', 'Accounting Manager']
    },
    financial: {
      view: ['Admin', 'Accounting Manager', 'Accountant'],
      export: ['Admin', 'Accounting Manager']
    },
    workforce: {
      view: ['Admin', 'Operations Manager', 'Accounting Manager'],
      export: ['Admin', 'Operations Manager', 'Accounting Manager']
    },
    fleet: {
      view: ['Admin', 'Operations Manager', 'Accounting Manager'],
      export: ['Admin', 'Operations Manager']
    }
  },

  // ADMINISTRATION MODULE
  administration: {
    users: {
      view: ['Admin'],
      create: ['Admin'],
      edit: ['Admin'],
      delete: ['Admin'],
      reset_password: ['Admin']
    },
    permissions: {
      view: ['Admin'],
      manage: ['Admin']
    },
    reference_data: {
      view: ['Admin', 'Operations Manager'],
      manage: ['Admin']
    }
  }
};

/**
 * Role Hierarchy - Higher roles inherit permissions from lower roles
 * Role names match backend format (with spaces)
 * Admin > Operations Manager > Team Leader > General Worker
 * Admin > Accounting Manager > Accountant
 * Operations Manager > Operations Clerk
 */
export const ROLE_HIERARCHY: { [role: string]: number } = {
  'Admin': 100,
  'Operations Manager': 80,
  'Accounting Manager': 80,
  'Team Leader': 60,
  'Operations Clerk': 50,
  'Accountant': 50,
  'Driver': 40,
  'General Worker': 20
};

/**
 * Default dashboard routes per role
 * Role names match backend format (with spaces)
 */
export const ROLE_DASHBOARD_ROUTES: { [role: string]: string } = {
  'Admin': '/dashboard/admin',
  'Operations Manager': '/dashboard/manager',
  'Accounting Manager': '/dashboard/accounting',
  'Operations Clerk': '/dashboard/clerk',
  'Accountant': '/dashboard/accounting',
  'Team Leader': '/dashboard/team-leader',
  'Driver': '/dashboard/driver',
  'General Worker': '/dashboard/worker'
};
