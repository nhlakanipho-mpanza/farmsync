export interface Supplier {
  id: string;
  name: string;
  contactPerson: string;
  email: string;
  phone: string;
  address?: string;
  taxNumber?: string;
  isActive: boolean;
}

export interface CreateSupplierDto {
  name: string;
  contactPerson: string;
  email: string;
  phone: string;
  address?: string;
  taxNumber?: string;
}

export interface PurchaseOrder {
  id: string;
  poNumber: string;
  supplierId: string;
  supplierName: string;
  orderDate: Date;
  expectedDeliveryDate?: Date;
  status: POStatus;
  totalAmount: number;
  notes?: string;
  approvedBy?: string;
  approvedAt?: Date;
  items: PurchaseOrderItem[];
}

export interface PurchaseOrderItem {
  id: string;
  purchaseOrderId: string;
  inventoryItemId: string;
  itemName: string;
  itemSKU: string;
  orderedQuantity: number;
  receivedQuantity: number;
  unitPrice: number;
  description?: string;
  notes?: string;
}

export interface CreatePurchaseOrderDto {
  supplierId: string;
  orderDate: Date;
  expectedDeliveryDate?: Date;
  notes?: string;
  items: CreatePurchaseOrderItemDto[];
}

export interface CreatePurchaseOrderItemDto {
  inventoryItemId: string;
  orderedQuantity: number;
  unitPrice: number;
  description?: string;
}

export interface UpdatePurchaseOrderDto {
  id: string;
  supplierId: string;
  expectedDeliveryDate?: Date;
  notes?: string;
  items: CreatePurchaseOrderItemDto[];
}

export interface GoodsReceived {
  id: string;
  receiptNumber: string;
  purchaseOrderId: string;
  poNumber: string;
  supplierName: string;
  receivedDate: Date;
  receivedBy: string;
  status: GRStatus;
  hasDiscrepancies: boolean;
  discrepancyNotes?: string;
  approvedBy?: string;
  approvedAt?: Date;
  items: GoodsReceivedItem[];
}

export interface GoodsReceivedItem {
  id: string;
  goodsReceivedId: string;
  purchaseOrderItemId: string;
  itemName: string;
  orderedQuantity: number;
  quantityReceived: number;
  quantityDamaged: number;
  quantityShortfall: number;
  condition: string;
  notes?: string;
}

export interface CreateGoodsReceivedDto {
  purchaseOrderId: string;
  receivedDate: Date;
  discrepancyNotes?: string;
  items: CreateGoodsReceivedItemDto[];
}

export interface CreateGoodsReceivedItemDto {
  purchaseOrderItemId: string;
  quantityReceived: number;
  quantityDamaged: number;
  quantityShortfall: number;
  condition: string;
  notes?: string;
}

export enum POStatus {
  Created = 'Created',
  Approved = 'Approved',
  PartiallyReceived = 'PartiallyReceived',
  FullyReceived = 'FullyReceived',
  Closed = 'Closed',
  Cancelled = 'Cancelled'
}

export enum GRStatus {
  Pending = 'Pending',
  Approved = 'Approved',
  Completed = 'Completed',
  Rejected = 'Rejected'
}
