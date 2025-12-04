import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormArray, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { GoodsReceivedService } from '../services/goods-received.service';
import { PurchaseOrderService } from '../services/purchase-order.service';
import { PurchaseOrder } from '../models/procurement.model';

@Component({
  selector: 'app-goods-receiving-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatButtonModule,
    MatIconModule,
    MatCheckboxModule,
    MatSnackBarModule
  ],
  templateUrl: './goods-receiving-form.component.html',
  styleUrls: ['./goods-receiving-form.component.scss']
})
export class GoodsReceivingFormComponent implements OnInit {
  grForm: FormGroup;
  purchaseOrder: PurchaseOrder | null = null;
  availablePOs: PurchaseOrder[] = [];
  loading = false;
  showPOSelector = false;
  isFinalReceipt = false; // Flag to indicate if this is the final receipt with discrepancies

  constructor(
    private fb: FormBuilder,
    private goodsReceivedService: GoodsReceivedService,
    private purchaseOrderService: PurchaseOrderService,
    private route: ActivatedRoute,
    private router: Router,
    private snackBar: MatSnackBar
  ) {
    this.grForm = this.fb.group({
      purchaseOrderId: ['', Validators.required],
      receivedDate: [new Date(), Validators.required],
      discrepancyNotes: [''],
      isFinalReceipt: [false], // Checkbox to mark as final receipt
      items: this.fb.array([])
    });
  }

  ngOnInit(): void {
    const poId = this.route.snapshot.queryParamMap.get('poId');
    if (poId) {
      this.loadPurchaseOrder(poId);
    } else {
      // Load approved POs for selection
      this.showPOSelector = true;
      this.loadAvailablePOs();
    }
  }

  get items(): FormArray {
    return this.grForm.get('items') as FormArray;
  }

  loadAvailablePOs(): void {
    this.loading = true;
    this.purchaseOrderService.getAvailableForReceiving().subscribe({
      next: (pos) => {
        this.availablePOs = pos;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading purchase orders:', error);
        this.snackBar.open('Error loading purchase orders', 'Close', { duration: 3000 });
        this.loading = false;
      }
    });
  }

  onPOSelected(poId: string): void {
    if (poId) {
      this.showPOSelector = false;
      this.loadPurchaseOrder(poId);
    }
  }

  loadPurchaseOrder(id: string): void {
    this.loading = true;
    this.purchaseOrderService.getById(id).subscribe({
      next: (po) => {
        this.purchaseOrder = po;
        this.grForm.patchValue({ purchaseOrderId: po.id });

        // Add form controls for each PO item
        po.items.forEach(item => {
          const remainingQty = item.orderedQuantity - item.receivedQuantity;
          this.items.push(this.fb.group({
            purchaseOrderItemId: [item.id, Validators.required],
            itemName: [{ value: item.itemName, disabled: true }],
            orderedQuantity: [{ value: item.orderedQuantity, disabled: true }],
            previouslyReceived: [{ value: item.receivedQuantity, disabled: true }],
            remainingQuantity: [{ value: remainingQty, disabled: true }],
            quantityReceived: [{ value: remainingQty, disabled: true }], // Locked to remaining quantity
            quantityDamaged: [0, [Validators.min(0)]],
            quantityShortfall: [0, [Validators.min(0)]],
            condition: ['Good', Validators.required],
            notes: ['']
          }));
        });

        this.loading = false;
      },
      error: (error) => {
        this.snackBar.open('Error loading purchase order', 'Close', { duration: 3000 });
        this.router.navigate(['/procurement/purchase-orders']);
        this.loading = false;
      }
    });
  }
  onDamagedChange(index: number): void {
    const item = this.items.at(index);
    const damaged = item.get('quantityDamaged')?.value || 0;
    const quantityReceived = item.get('quantityReceived')?.value || 0;
    
    // Auto-calculate shortfall when damaged is entered
    const actualReceived = quantityReceived - damaged;
    const remainingQty = this.purchaseOrder?.items[index].orderedQuantity || 0;
    const previouslyReceived = this.purchaseOrder?.items[index].receivedQuantity || 0;
    const expectedQty = remainingQty - previouslyReceived;
    
    const shortfall = Math.max(0, expectedQty - actualReceived);
    item.patchValue({ quantityShortfall: shortfall }, { emitEvent: false });
  }

  hasDiscrepancies(): boolean {
    return this.items.controls.some(item => 
      (item.get('quantityDamaged')?.value || 0) > 0 || 
      (item.get('quantityShortfall')?.value || 0) > 0
    );
  }

  onFinalReceiptChange(isFinal: boolean): void {
    this.isFinalReceipt = isFinal;
    if (isFinal && !this.hasDiscrepancies()) {
      this.snackBar.open('Final receipt is only for deliveries with discrepancies', 'Close', { duration: 3000 });
      this.grForm.patchValue({ isFinalReceipt: false });
    }
  }

  onSubmit(): void {
    if (this.grForm.invalid) {
      this.snackBar.open('Please fill in all required fields', 'Close', { duration: 3000 });
      return;
    }

    const formData = this.grForm.value;
    const isFinalReceipt = formData.isFinalReceipt;
    const hasDiscrepancies = this.hasDiscrepancies();

    // Validate: Final receipt should only be used with discrepancies
    if (isFinalReceipt && !hasDiscrepancies) {
      this.snackBar.open('Final receipt is only for deliveries with discrepancies. Uncheck "Final Receipt" for complete deliveries.', 'Close', { duration: 4000 });
      return;
    }

    // Prepare the payload
    const payload = {
      purchaseOrderId: formData.purchaseOrderId,
      receivedDate: formData.receivedDate instanceof Date ? formData.receivedDate.toISOString() : formData.receivedDate,
      discrepancyNotes: hasDiscrepancies ? formData.discrepancyNotes : null,
      isFinalReceipt: isFinalReceipt,
      items: this.items.getRawValue().map((item: any) => ({
        purchaseOrderItemId: item.purchaseOrderItemId,
        quantityReceived: item.quantityReceived,
        quantityDamaged: item.quantityDamaged || 0,
        quantityShortfall: item.quantityShortfall || 0,
        condition: item.condition,
        notes: item.notes || null
      }))
    };

    console.log('Submitting goods receipt:', payload);

    this.goodsReceivedService.create(payload).subscribe({
      next: () => {
        let message = 'Goods received successfully';
        if (hasDiscrepancies && isFinalReceipt) {
          message = 'Final receipt recorded with discrepancies - Purchase order closed, inventory updated with actual received quantity';
        } else if (hasDiscrepancies) {
          message = 'Goods received with discrepancies - Pending manager approval';
        }
        this.snackBar.open(message, 'Close', { duration: 4000 });
        this.router.navigate(['/procurement/purchase-orders']);
      },
      error: (error) => {
        console.error('Error creating goods receipt:', error);
        this.snackBar.open(error.error?.message || 'Error recording goods receipt', 'Close', { duration: 3000 });
      }
    });
  }

  cancel(): void {
    this.router.navigate(['/procurement/purchase-orders']);
  }
}
