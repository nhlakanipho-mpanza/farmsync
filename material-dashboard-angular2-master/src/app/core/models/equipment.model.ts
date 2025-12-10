export interface Equipment {
  id: string;
  name: string;
  description?: string;
  serialNumber?: string;
  model?: string;
  manufacturer?: string;
  conditionId: string;
  conditionName: string;
  locationId?: string;
  locationName?: string;
  purchaseDate?: Date;
  purchasePrice?: number;
  lastMaintenanceDate?: Date;
  nextMaintenanceDue?: Date;
  isActive: boolean;
}

export interface CreateEquipmentDto {
  name: string;
  description?: string;
  serialNumber?: string;
  model?: string;
  manufacturer?: string;
  conditionId: string;
  locationId?: string;
  purchaseDate?: Date;
  purchasePrice?: number;
}

export interface UpdateEquipmentDto {
  id: string;
  name: string;
  description?: string;
  serialNumber?: string;
  model?: string;
  manufacturer?: string;
  conditionId: string;
  locationId?: string;
  purchaseDate?: Date;
  purchasePrice?: number;
  lastMaintenanceDate?: Date;
  nextMaintenanceDue?: Date;
  isActive: boolean;
}
