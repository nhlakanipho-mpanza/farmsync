export interface TableConfig {
  key: string;
  label: string;
  hasHourlyRate?: boolean; // For positions table
  hasDriverFlag?: boolean; // For positions table
  hasBranchCode?: boolean; // For bank-names table
  hasSortOrder?: boolean; // For field-phases table
  hasRequiresDocument?: boolean; // For leave-types table
  hasDefaultDays?: boolean; // For leave-types table
}

export interface CategoryConfig {
  key: string;
  label: string;
  icon: string;
  tables: TableConfig[];
}

export const REFERENCE_CATEGORIES: CategoryConfig[] = [
  {
    key: 'hr',
    label: 'HR & Workforce',
    icon: 'people',
    tables: [
      { key: 'employment-types', label: 'Employment Types' },
      { key: 'positions', label: 'Positions', hasHourlyRate: true, hasDriverFlag: true },
      { key: 'team-types', label: 'Team Types' },
      { key: 'work-areas', label: 'Work Areas' },
      { key: 'leave-types', label: 'Leave Types', hasRequiresDocument: true, hasDefaultDays: true }
    ]
  },
  {
    key: 'banking',
    label: 'Banking Settings',
    icon: 'account_balance',
    tables: [
      { key: 'bank-names', label: 'Bank Names', hasBranchCode: true },
      { key: 'account-types', label: 'Account Types' }
    ]
  },
  {
    key: 'tasks',
    label: 'Tasks',
    icon: 'task',
    tables: [
      { key: 'task-statuses', label: 'Task Statuses' },
      { key: 'issue-statuses', label: 'Issue Statuses' },
      { key: 'locations', label: 'Locations' }
    ]
  },
  {
    key: 'maintenance',
    label: 'Maintenance',
    icon: 'build',
    tables: [
      { key: 'conditions', label: 'Equipment Conditions' },
      { key: 'maintenance-types', label: 'Maintenance Types' },
      { key: 'fleet-maintenance-types', label: 'Fleet Maintenance Types' }
    ]
  },
  {
    key: 'inventory',
    label: 'Inventory',
    icon: 'inventory_2',
    tables: [
      { key: 'units', label: 'Units of Measure' },
      { key: 'categories', label: 'Inventory Categories' },
      { key: 'types', label: 'Item Types' }
    ]
  },
  {
    key: 'fleet',
    label: 'Fleet',
    icon: 'local_shipping',
    tables: [
      { key: 'vehicle-types', label: 'Vehicle Types' },
      { key: 'vehicle-statuses', label: 'Vehicle Statuses' },
      { key: 'fuel-types', label: 'Fuel Types' },
      { key: 'fleet-task-statuses', label: 'Fleet Task Statuses' }
    ]
  },
  {
    key: 'settings',
    label: 'System Settings',
    icon: 'settings',
    tables: [
      { key: 'statuses', label: 'Transaction Statuses' },
      { key: 'document-types', label: 'Document Types' },
      { key: 'field-phases', label: 'Field Phases', hasSortOrder: true }
    ]
  }
];
