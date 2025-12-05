// Vehicle Models
export interface Vehicle {
  id: string;
  registrationNumber: string;
  make: string;
  model: string;
  year: number;
  engineNumber?: string;
  chassisNumber?: string;
  assetNumber?: string;
  currentOdometer: number;
  purchaseDate?: string;
  purchasePrice?: number;
  // Maintenance tracking
  lastServiceDate?: string;
  lastServiceOdometer?: number;
  lastServiceType?: string; // 'Minor' or 'Major'
  nextServiceOdometer?: number;
  // License disk
  licenseDiskExpiryDate?: string;
  notes?: string;
  isActive: boolean;
  vehicleTypeId: string;
  vehicleStatusId: string;
  fuelTypeId: string;
  vehicleTypeName?: string;
  vehicleStatusName?: string;
  fuelTypeName?: string;
  createdAt: string;
  updatedAt?: string;
}

export interface CreateVehicleDTO {
  registrationNumber: string;
  make: string;
  model: string;
  year: number;
  engineNumber?: string;
  chassisNumber?: string;
  assetNumber?: string;
  currentOdometer: number;
  purchaseDate?: string;
  purchasePrice?: number;
  // Maintenance tracking
  lastServiceDate?: string;
  lastServiceOdometer?: number;
  lastServiceType?: string;
  // License disk
  licenseDiskExpiryDate?: string;
  notes?: string;
  vehicleTypeId: string;
  vehicleStatusId: string;
  fuelTypeId: string;
}

export interface UpdateVehicleDTO {
  registrationNumber: string;
  make: string;
  model: string;
  year: number;
  engineNumber?: string;
  chassisNumber?: string;
  assetNumber?: string;
  currentOdometer: number;
  // Maintenance tracking
  lastServiceDate?: string;
  lastServiceOdometer?: number;
  lastServiceType?: string;
  nextServiceOdometer?: number;
  // License disk
  licenseDiskExpiryDate?: string;
  notes?: string;
  isActive: boolean;
  vehicleTypeId: string;
  vehicleStatusId: string;
  fuelTypeId: string;
}

// GPS & Trip Models
export interface GPSLocation {
  id: string;
  timestamp: string;
  latitude: number;
  longitude: number;
  altitude?: number;
  speed?: number;
  heading?: number;
  odometer?: number;
  vehicleId: string;
  tripLogId?: string;
  vehicleRegistration?: string;
}

export interface TripLog {
  id: string;
  startTime: string;
  endTime?: string;
  startOdometer: number;
  endOdometer?: number;
  distanceTraveled?: number;
  startLocation?: string;
  endLocation?: string;
  startLatitude?: number;
  startLongitude?: number;
  endLatitude?: number;
  endLongitude?: number;
  purpose?: string;
  notes?: string;
  isCompleted: boolean;
  isActive: boolean;
  vehicleId: string;
  driverId: string;
  transportTaskId?: string;
  vehicleRegistration?: string;
  driverName?: string;
  taskNumber?: string;
  createdAt: string;
}

export interface CreateTripLogDTO {
  startTime: string;
  startOdometer: number;
  startLocation?: string;
  startLatitude?: number;
  startLongitude?: number;
  purpose?: string;
  notes?: string;
  vehicleId: string;
  driverId: string;
  transportTaskId?: string;
}

export interface UpdateTripLogDTO {
  endTime?: string;
  endOdometer?: number;
  endLocation?: string;
  endLatitude?: number;
  endLongitude?: number;
  notes?: string;
  isCompleted: boolean;
}

// Maintenance Models
export interface MaintenanceRecord {
  id: string;
  scheduledDate: string;
  completedDate?: string;
  odometerReading: number;
  engineHours?: number;
  description?: string;
  partsReplaced?: string;
  laborCost?: number;
  partsCost?: number;
  totalCost: number;
  mechanicNotes?: string;
  nextServiceOdometer?: number;
  nextServiceHours?: number;
  isCompleted: boolean;
  isActive: boolean;
  vehicleId: string;
  maintenanceTypeId: string;
  performedById?: string;
  vehicleRegistration?: string;
  maintenanceTypeName?: string;
  performedByName?: string;
  createdAt: string;
}

export interface CreateMaintenanceRecordDTO {
  scheduledDate: string;
  odometerReading: number;
  engineHours?: number;
  description?: string;
  vehicleId: string;
  maintenanceTypeId: string;
}

export interface UpdateMaintenanceRecordDTO {
  completedDate?: string;
  odometerReading: number;
  engineHours?: number;
  description?: string;
  partsReplaced?: string;
  laborCost?: number;
  partsCost?: number;
  mechanicNotes?: string;
  nextServiceOdometer?: number;
  nextServiceHours?: number;
  isCompleted: boolean;
  performedById?: string;
}

// Fuel Models
export interface FuelLog {
  id: string;
  fuelDate: string;
  quantity: number;
  unitPrice: number;
  totalCost: number;
  odometerReading: number;
  station?: string;
  receiptNumber?: string;
  isFull: boolean;
  notes?: string;
  isActive: boolean;
  vehicleId: string;
  filledById?: string;
  vehicleRegistration?: string;
  filledByName?: string;
  createdAt: string;
}

export interface CreateFuelLogDTO {
  fuelDate: string;
  quantity: number;
  unitPrice: number;
  odometerReading: number;
  station?: string;
  receiptNumber?: string;
  isFull: boolean;
  notes?: string;
  vehicleId: string;
  filledById?: string;
}

export interface UpdateFuelLogDTO {
  fuelDate: string;
  quantity: number;
  unitPrice: number;
  odometerReading: number;
  station?: string;
  receiptNumber?: string;
  isFull: boolean;
  notes?: string;
}

// Incident Models
export interface IncidentReport {
  id: string;
  incidentDate: string;
  incidentType: string;
  description: string;
  location?: string;
  latitude?: number;
  longitude?: number;
  severity?: string;
  photoUrl?: string;
  estimatedCost?: number;
  actualCost?: number;
  status: string;
  resolvedDate?: string;
  resolutionNotes?: string;
  isActive: boolean;
  vehicleId: string;
  reportedById: string;
  assignedToId?: string;
  vehicleRegistration?: string;
  reportedByName?: string;
  assignedToName?: string;
  createdAt: string;
}

// Reference Data Models
export interface VehicleType {
  id: string;
  name: string;
  description?: string;
  isActive: boolean;
}

export interface VehicleStatus {
  id: string;
  name: string;
  description?: string;
  isActive: boolean;
}

export interface FuelType {
  id: string;
  name: string;
  description?: string;
  isActive: boolean;
}

export interface MaintenanceType {
  id: string;
  name: string;
  description?: string;
  isActive: boolean;
}

// Driver Assignment Models
export interface DriverAssignment {
  id: string;
  vehicleId: string;
  vehicleRegistration?: string;
  vehicleMake?: string;
  vehicleModel?: string;
  driverId: string;
  driverName?: string;
  driverEmployeeNumber?: string;
  startDate: string;
  endDate?: string;
  assignmentType: string; // 'Primary', 'Temporary', 'Pool'
  isPrimary: boolean;
  notes?: string;
  isCurrentAssignment: boolean;
  assignedById?: string;
  assignedByName?: string;
  createdAt: string;
  updatedAt?: string;
}

export interface CreateDriverAssignmentDTO {
  vehicleId: string;
  driverId: string;
  startDate: string;
  endDate?: string;
  assignmentType: string;
  isPrimary: boolean;
  notes?: string;
}

export interface UpdateDriverAssignmentDTO {
  endDate?: string;
  notes?: string;
}
