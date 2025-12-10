// Employee Models
export interface Employee {
  id: string;
  fullName: string;
  employeeNumber: string;
  dateOfBirth?: string;
  gender?: string;
  idNumber?: string;
  contactNumber?: string;
  email?: string;
  address?: string;
  hireDate?: string;
  terminationDate?: string;
  positionId: string;
  positionName?: string;
  positionIsDriverPosition?: boolean;
  rate?: number;
  employmentTypeId?: string;
  employmentTypeName?: string;
  userId: string;
  username?: string;
  roleId: string;
  roleName?: string;
  driverLicenseNumber?: string;
  driverLicenseExpiryDate?: string;
  driverLicenseDocumentId?: string;
  isActive: boolean;
  createdAt: string;
  updatedAt?: string;
}

export interface CreateEmployeeDTO {
  fullName: string;
  employeeNumber: string;
  dateOfBirth?: string;
  gender?: string;
  idNumber?: string;
  contactNumber?: string;
  email: string;
  address?: string;
  hireDate?: string;
  positionId: string;
  employmentTypeId?: string;
  roleId: string;
  driverLicenseNumber?: string;
  driverLicenseExpiryDate?: string;
  driverLicenseDocumentId?: string;
}

export interface UpdateEmployeeDTO {
  fullName: string;
  dateOfBirth?: string;
  gender?: string;
  idNumber?: string;
  contactNumber?: string;
  email?: string;
  address?: string;
  terminationDate?: string;
  positionId: string;
  employmentTypeId?: string;
  roleId: string;
  isActive: boolean;
  driverLicenseNumber?: string;
  driverLicenseExpiryDate?: string;
  driverLicenseDocumentId?: string;
}

// Emergency Contact Models
export interface EmergencyContact {
  id: string;
  employeeId: string;
  fullName: string;
  relationship: string;
  contactNumber: string;
  alternateNumber?: string;
  address?: string;
}

export interface CreateEmergencyContactDTO {
  fullName: string;
  relationship: string;
  contactNumber: string;
  alternateNumber?: string;
  address?: string;
}

// Bank Details Models
export interface BankDetails {
  id: string;
  employeeId: string;
  accountNumber: string;
  branchCode?: string;
  bankNameId?: string;
  bankName?: string;
  accountTypeId?: string;
  accountTypeName?: string;
}

export interface CreateBankDetailsDTO {
  accountNumber: string;
  branchCode?: string;
  bankNameId?: string;
  accountTypeId?: string;
}

// Biometric Enrolment Models
export interface BiometricEnrolment {
  id: string;
  employeeId: string;
  biometricId: string;
  enrolmentDate: string;
  isActive: boolean;
}

export interface CreateBiometricEnrolmentDTO {
  biometricId: string;
  enrolmentDate: string;
}

// Team Models
export interface Team {
  id: string;
  name: string;
  description?: string;
  teamTypeId?: string;
  teamTypeName?: string;
  teamLeaderId?: string;
  teamLeaderName?: string;
  isActive: boolean;
  createdAt: string;
  updatedAt?: string;
}

export interface CreateTeamDTO {
  name: string;
  description?: string;
  teamTypeId?: string;
  teamLeaderId?: string;
}

export interface UpdateTeamDTO {
  name: string;
  description?: string;
  teamTypeId?: string;
  teamLeaderId?: string;
  isActive: boolean;
}

// Team Member Models
export interface TeamMember {
  id: string;
  teamId: string;
  teamName?: string;
  employeeId: string;
  employeeName?: string;
  startDate: string;
  endDate?: string;
  isPermanent: boolean;
  notes?: string;
}

export interface CreateTeamMemberDTO {
  employeeId: string;
  startDate: string;
  endDate?: string;
  isPermanent: boolean;
  notes?: string;
}

// Work Task Models
export interface WorkTask {
  id: string;
  taskName: string;
  description?: string;
  workAreaId?: string;
  workAreaName?: string;
  scheduledDate: string;
  estimatedHours?: number;
  actualHours?: number;
  teamId?: string;
  teamName?: string;
  employeeId?: string;
  employeeName?: string;
  taskStatusId?: string;
  taskStatusName?: string;
  createdAt: string;
  updatedAt?: string;
}

export interface CreateWorkTaskDTO {
  taskName: string;
  description?: string;
  workAreaId?: string;
  scheduledDate: string;
  estimatedHours?: number;
  teamId?: string;
  employeeId?: string;
  taskStatusId?: string;
}

export interface UpdateWorkTaskDTO {
  taskName: string;
  description?: string;
  workAreaId?: string;
  scheduledDate: string;
  estimatedHours?: number;
  actualHours?: number;
  teamId?: string;
  employeeId?: string;
  taskStatusId?: string;
}

// Clock Event Models
export interface ClockEvent {
  id: string;
  employeeId: string;
  employeeName?: string;
  eventTime: string;
  eventType: string;
  biometricId?: string;
  teamId?: string;
  teamName?: string;
  notes?: string;
}

export interface CreateClockEventDTO {
  employeeId: string;
  eventTime: string;
  eventType: string;
  biometricId?: string;
  teamId?: string;
  notes?: string;
}

export interface AttendanceSummary {
  employeeId: string;
  employeeName: string;
  date: string;
  clockIn?: string;
  clockOut?: string;
  totalHours?: number;
}

// Inventory Issue Models
export interface InventoryIssue {
  id: string;
  issueNumber: string;
  inventoryItemId: string;
  inventoryItemName?: string;
  quantity: number;
  issuedDate: string;
  workTaskId?: string;
  teamId?: string;
  teamName?: string;
  employeeId?: string;
  employeeName?: string;
  issueStatusId?: string;
  issueStatusName?: string;
  issuedBy?: string;
  approvedBy?: string;
  approvedDate?: string;
  returnedDate?: string;
  returnedQuantity?: number;
  notes?: string;
}

export interface CreateInventoryIssueDTO {
  inventoryItemId: string;
  quantity: number;
  workTaskId?: string;
  teamId?: string;
  employeeId?: string;
  notes?: string;
}

export interface ApproveInventoryIssueDTO {
  approve: boolean;
  notes?: string;
}

export interface ReturnInventoryIssueDTO {
  returnedQuantity: number;
  notes?: string;
}

// Equipment Issue Models
export interface EquipmentIssue {
  id: string;
  issueNumber: string;
  equipmentId: string;
  equipmentName?: string;
  issuedDate: string;
  expectedReturnDate?: string;
  actualReturnDate?: string;
  returnCondition?: string;
  workTaskId?: string;
  teamId?: string;
  teamName?: string;
  employeeId?: string;
  employeeName?: string;
  issueStatusId?: string;
  issueStatusName?: string;
  issuedBy?: string;
  approvedBy?: string;
  approvedDate?: string;
  notes?: string;
}

export interface CreateEquipmentIssueDTO {
  equipmentId: string;
  expectedReturnDate?: string;
  workTaskId?: string;
  teamId?: string;
  employeeId?: string;
  notes?: string;
}

export interface ReturnEquipmentIssueDTO {
  returnCondition: string;
  notes?: string;
}

// Reference Data Models
export interface Position {
  id: string;
  name: string;
  rate?: number;
}

export interface EmploymentType {
  id: string;
  name: string;
}

export interface RoleType {
  id: string;
  name: string;
}

export interface TeamType {
  id: string;
  name: string;
}

export interface BankName {
  id: string;
  name: string;
}

export interface AccountType {
  id: string;
  name: string;
}

export interface TaskStatus {
  id: string;
  name: string;
}

export interface IssueStatus {
  id: string;
  name: string;
}

export interface WorkArea {
  id: string;
  name: string;
  sizeUnit?: string;
  size?: number;
}
