export interface User {
  id: string;
  username: string;
  email: string;
  fullName: string;
  roles: string[];
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  username: string;
  email: string;
  fullName: string;
  roles: string[];
  expiresAt: Date;
}

export interface RegisterRequest {
  username: string;
  email: string;
  password: string;
  confirmPassword: string;
  fullName: string;
  role: string;
}

export enum UserRole {
  Admin = 'Admin',
  OperationsManager = 'Operations Manager',
  AccountingManager = 'Accounting Manager',
  OperationsClerk = 'Operations Clerk',
  Accountant = 'Accountant',
  TeamLeader = 'Team Leader',
  Driver = 'Driver',
  GeneralWorker = 'General Worker'
}
