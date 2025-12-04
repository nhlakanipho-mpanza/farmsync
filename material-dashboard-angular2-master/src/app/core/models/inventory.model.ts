export interface InventoryItem {
  id: string;
  name: string;
  description?: string;
  sku?: string;
  categoryName: string;
  typeName: string;
  unitOfMeasureName: string;
  minimumStockLevel: number;
  reorderLevel: number;
  unitPrice?: number;
  totalStock: number;
  isActive: boolean;
}

export interface CreateInventoryItemDto {
  name: string;
  description?: string;
  sku?: string;
  categoryId: string;
  typeId: string;
  unitOfMeasureId: string;
  minimumStockLevel: number;
  reorderLevel: number;
  unitPrice?: number;
}

export interface UpdateInventoryItemDto {
  id: string;
  name: string;
  description?: string;
  sku?: string;
  categoryId: string;
  typeId: string;
  unitOfMeasureId: string;
  minimumStockLevel: number;
  reorderLevel: number;
  unitPrice?: number;
  isActive: boolean;
}

export enum InventoryCategory {
  Seeds = 'Seeds',
  Fertilizers = 'Fertilizers',
  Pesticides = 'Pesticides',
  Tools = 'Tools',
  Equipment = 'Equipment',
  Livestock = 'Livestock',
  Feed = 'Feed',
  Other = 'Other'
}

export interface InventoryStats {
  totalItems: number;
  totalValue: number;
  lowStockItems: number;
  categories: { [key: string]: number };
}
