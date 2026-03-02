import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormArray, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { forkJoin, of } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { PurchaseOrderService } from '../services/purchase-order.service';
import { SupplierService } from '../services/supplier.service';
import { InventoryService } from '../../core/services/inventory.service';
import { AuthService } from '../../core/services/auth.service';
import { Supplier } from '../models/procurement.model';
import { InventoryItem } from '../../core/models/inventory.model';
import { DocumentUploadComponent } from '../../components/document-upload/document-upload.component';
import { ComponentsModule } from '../../components/components.module';

@Component({
  selector: 'app-purchase-order-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatAutocompleteModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatButtonModule,
    MatIconModule,
    MatSnackBarModule,
    ComponentsModule
  ],
  templateUrl: './purchase-order-form.component.html',
  styleUrls: ['./purchase-order-form.component.scss']
})
export class PurchaseOrderFormComponent implements OnInit {
  @ViewChild(DocumentUploadComponent) documentUpload!: DocumentUploadComponent;
  
  poForm: FormGroup;
  suppliers: Supplier[] = [];
  inventoryItems: InventoryItem[] = [];
  filteredInventoryItems: InventoryItem[][] = [];
  isEditMode = false;
  poId: string | null = null;
  loading = false;
  currentUserRole: string = '';

  constructor(
    private fb: FormBuilder,
    private purchaseOrderService: PurchaseOrderService,
    private supplierService: SupplierService,
    private inventoryService: InventoryService,
    private authService: AuthService,
    private route: ActivatedRoute,
    private router: Router,
    private snackBar: MatSnackBar
  ) {
    this.poForm = this.fb.group({
      supplierId: ['', Validators.required],
      orderDate: [new Date(), Validators.required],
      expectedDeliveryDate: [''],
      notes: [''],
      items: this.fb.array([])
    });
    
    // Get current user role
    const currentUser = this.authService.currentUserValue;
    this.currentUserRole = currentUser?.roles?.[0] || '';
  }

  ngOnInit(): void {
    this.poId = this.route.snapshot.paramMap.get('id');
    this.isEditMode = !!this.poId;
    this.loadFormData();
  }

  get items(): FormArray {
    return this.poForm.get('items') as FormArray;
  }

  loadFormData(): void {
    this.loading = true;
    forkJoin({
      suppliers: this.supplierService.getAll(),
      inventoryItems: this.inventoryService.getAll()
    }).subscribe({
      next: (data) => {
        this.suppliers = data.suppliers.filter(s => s.isActive);
        this.inventoryItems = data.inventoryItems.filter(i => i.isActive);
        
        if (this.isEditMode && this.poId) {
          this.loadPurchaseOrder(this.poId);
        } else {
          this.addItem();
        }
        this.loading = false;
      },
      error: (error) => {
        this.snackBar.open('Error loading form data', 'Close', { duration: 3000 });
        this.loading = false;
      }
    });
  }

  loadPurchaseOrder(id: string): void {
    this.purchaseOrderService.getById(id).subscribe({
      next: (po) => {
        this.poForm.patchValue({
          supplierId: po.supplierId,
          orderDate: new Date(po.orderDate),
          expectedDeliveryDate: po.expectedDeliveryDate ? new Date(po.expectedDeliveryDate) : null,
          notes: po.notes
        });

        po.items.forEach((item, index) => {
          const itemGroup = this.fb.group({
            inventoryItemId: [item.inventoryItemId, Validators.required],
            orderedQuantity: [item.orderedQuantity, [Validators.required, Validators.min(1)]],
            unitPrice: [item.unitPrice, [Validators.required, Validators.min(0)]],
            lineTotal: [item.orderedQuantity * item.unitPrice],
            description: [item.description]
          });
          
          // Set up price calculation for loaded items
          this.setupPriceCalculation(itemGroup);
          
          this.items.push(itemGroup);
          
          // Initialize filtered inventory items for each existing item
          this.filteredInventoryItems[index] = [...this.inventoryItems];
        });
      },
      error: (error) => {
        this.snackBar.open('Error loading purchase order', 'Close', { duration: 3000 });
        this.router.navigate(['/procurement/purchase-orders']);
      }
    });
  }

  addItem(): void {
    const index = this.items.length;
    const itemGroup = this.fb.group({
      inventoryItemId: ['', Validators.required],
      orderedQuantity: [1, [Validators.required, Validators.min(1)]],
      unitPrice: [0, [Validators.required, Validators.min(0)]],
      lineTotal: [0],
      description: ['']
    });
    
    // Set up value change listeners for auto-calculation
    this.setupPriceCalculation(itemGroup);
    
    this.items.push(itemGroup);
    
    // Initialize filtered inventory items for this new row
    this.filteredInventoryItems[index] = [...this.inventoryItems];
  }

  setupPriceCalculation(itemGroup: FormGroup): void {
    // When quantity or unit price changes, update line total
    itemGroup.get('orderedQuantity')?.valueChanges.subscribe(() => {
      this.updateLineTotal(itemGroup);
    });
    
    itemGroup.get('unitPrice')?.valueChanges.subscribe(() => {
      this.updateLineTotal(itemGroup);
    });
    
    // When line total is manually changed, calculate unit price
    itemGroup.get('lineTotal')?.valueChanges.subscribe((total) => {
      const quantity = itemGroup.get('orderedQuantity')?.value || 0;
      if (quantity > 0 && total !== null && total !== undefined) {
        const calculatedUnitPrice = total / quantity;
        // Only update if it's different to avoid circular updates
        const currentUnitPrice = itemGroup.get('unitPrice')?.value;
        if (Math.abs(calculatedUnitPrice - currentUnitPrice) > 0.01) {
          itemGroup.get('unitPrice')?.setValue(calculatedUnitPrice, { emitEvent: false });
        }
      }
    });
  }

  updateLineTotal(itemGroup: FormGroup): void {
    const quantity = itemGroup.get('orderedQuantity')?.value || 0;
    const unitPrice = itemGroup.get('unitPrice')?.value || 0;
    const total = quantity * unitPrice;
    itemGroup.get('lineTotal')?.setValue(total, { emitEvent: false });
  }

  removeItem(index: number): void {
    this.items.removeAt(index);
  }

  calculateLineTotal(index: number): number {
    const item = this.items.at(index);
    const quantity = item.get('orderedQuantity')?.value || 0;
    const price = item.get('unitPrice')?.value || 0;
    return quantity * price;
  }

  calculateGrandTotal(): number {
    let total = 0;
    for (let i = 0; i < this.items.length; i++) {
      total += this.calculateLineTotal(i);
    }
    return total;
  }

  onSubmit(): void {
    if (this.poForm.invalid) {
      this.snackBar.open('Please fill in all required fields', 'Close', { duration: 3000 });
      return;
    }

    const formData = this.poForm.value;
    
    // Format the data to match backend DTO expectations
    const formattedData = {
      supplierId: formData.supplierId,
      orderDate: formData.orderDate instanceof Date ? formData.orderDate.toISOString() : formData.orderDate,
      expectedDeliveryDate: formData.expectedDeliveryDate instanceof Date ? formData.expectedDeliveryDate.toISOString() : formData.expectedDeliveryDate,
      notes: formData.notes || null,
      items: formData.items.map((item: any) => ({
        inventoryItemId: item.inventoryItemId,
        orderedQuantity: parseFloat(item.orderedQuantity),
        unitPrice: parseFloat(item.unitPrice),
        description: item.description || null
      }))
    };

    console.log('Sending purchase order data:', formattedData);
    
    if (this.isEditMode && this.poId) {
      const updateData = {
        id: this.poId,
        ...formattedData
      };
      
      this.purchaseOrderService.update(this.poId, updateData).subscribe({
        next: () => {
          this.snackBar.open('Purchase order updated successfully', 'Close', { duration: 3000 });
          this.router.navigate(['/procurement/purchase-orders']);
        },
        error: (error) => {
          console.error('Update error:', error);
          this.snackBar.open(error.error?.message || 'Error updating purchase order', 'Close', { duration: 3000 });
        }
      });
    } else {
      this.purchaseOrderService.create(formattedData).pipe(
        switchMap((response: any) => {
          console.log('Purchase order created:', response);
          const purchaseOrderId = response.id || response.purchaseOrderId;
          
          // Upload documents if any files are selected
          if (this.documentUpload && this.documentUpload.selectedFile) {
            this.documentUpload.entityId = purchaseOrderId;
            return this.documentUpload.uploadDocument().pipe(
              switchMap(() => of(response))
            );
          }
          return of(response);
        })
      ).subscribe({
        next: () => {
          this.snackBar.open('Purchase order created successfully', 'Close', { duration: 3000 });
          this.router.navigate(['/procurement/purchase-orders']);
        },
        error: (error) => {
          console.error('Create error:', error);
          console.error('Error details:', JSON.stringify(error.error, null, 2));
          const errorMessage = error.error?.message || error.error?.title || 'Error creating purchase order';
          const validationErrors = error.error?.errors;
          
          if (validationErrors) {
            console.error('Validation errors:', validationErrors);
            const errorList = Object.entries(validationErrors)
              .map(([field, messages]: [string, any]) => `${field}: ${Array.isArray(messages) ? messages.join(', ') : messages}`)
              .join('\n');
            this.snackBar.open(`Validation errors:\n${errorList}`, 'Close', { duration: 5000 });
          } else {
            this.snackBar.open(errorMessage, 'Close', { duration: 3000 });
          }
        }
      });
    }
  }

  cancel(): void {
    this.router.navigate(['/procurement/purchase-orders']);
  }

  getItemSearchControl(index: number): any {
    const itemGroup = this.items.at(index) as FormGroup;
    if (!itemGroup.get('itemSearch')) {
      itemGroup.addControl('itemSearch', this.fb.control(''));
      
      // Initialize filtered list for this index
      if (!this.filteredInventoryItems[index]) {
        this.filteredInventoryItems[index] = [...this.inventoryItems];
      }
      
      // Listen for search changes
      itemGroup.get('itemSearch')?.valueChanges.subscribe(value => {
        if (typeof value === 'string') {
          this.filterInventoryItems(index, value);
        }
      });
    }
    return itemGroup.get('itemSearch');
  }

  filterInventoryItems(index: number, searchTerm: string): void {
    if (!searchTerm || searchTerm.trim() === '') {
      this.filteredInventoryItems[index] = [...this.inventoryItems];
      return;
    }
    
    const term = searchTerm.toLowerCase();
    this.filteredInventoryItems[index] = this.inventoryItems.filter(item =>
      item.name.toLowerCase().includes(term) ||
      (item.sku && item.sku.toLowerCase().includes(term))
    );
  }

  onItemSelected(event: any, index: number): void {
    const selectedItem = event.option.value;
    const itemGroup = this.items.at(index) as FormGroup;
    itemGroup.patchValue({
      inventoryItemId: selectedItem.id
    });
  }

  displayItem(item: InventoryItem | null): string {
    if (!item) return '';
    return item.sku ? `${item.name} (${item.sku})` : item.name;
  }

  // Helper to check if expected delivery should be shown
  canEditExpectedDelivery(): boolean {
    return this.isEditMode && (
      this.currentUserRole === 'Operations Clerk' ||
      this.currentUserRole === 'Operations Manager' ||
      this.currentUserRole === 'Accounting Manager' ||
      this.currentUserRole === 'Admin'
    );
  }

  // Get unit of measure for an item
  getItemUnitOfMeasure(index: number): string {
    const itemGroup = this.items.at(index) as FormGroup;
    const inventoryItemId = itemGroup.get('inventoryItemId')?.value;
    
    if (!inventoryItemId) return '';
    
    const selectedItem = this.inventoryItems.find(item => item.id === inventoryItemId);
    return selectedItem?.unitOfMeasureName || '';
  }
}
