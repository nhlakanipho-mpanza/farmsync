import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { forkJoin } from 'rxjs';
import { InventoryService } from 'app/core/services/inventory.service';
import { ReferenceDataService, ReferenceItem, UnitOfMeasure } from 'app/core/services/reference-data.service';

@Component({
  selector: 'app-inventory-form',
  templateUrl: './inventory-form.component.html',
  styleUrls: ['./inventory-form.component.css']
})
export class InventoryFormComponent implements OnInit {
  inventoryForm: FormGroup;
  loading = false;
  isEditMode = false;
  itemId: string;
  categories: ReferenceItem[] = [];
  types: ReferenceItem[] = [];
  units: UnitOfMeasure[] = [];

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private inventoryService: InventoryService,
    private referenceDataService: ReferenceDataService,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit() {
    this.itemId = this.route.snapshot.params['id'];
    this.isEditMode = !!this.itemId;

    this.inventoryForm = this.formBuilder.group({
      name: ['', Validators.required],
      description: [''],
      sku: [null],
      categoryId: ['', Validators.required],
      typeId: ['', Validators.required],
      unitOfMeasureId: ['', Validators.required],
      minimumStockLevel: [0, [Validators.required, Validators.min(0)]],
      reorderLevel: [10, [Validators.required, Validators.min(0)]],
      unitPrice: [0, [Validators.min(0)]],
      isActive: [true]
    });

    this.loadReferenceData();
  }

  loadReferenceData() {
    this.loading = true;
    forkJoin({
      categories: this.referenceDataService.getCategories(),
      types: this.referenceDataService.getTypes(),
      units: this.referenceDataService.getUnitsOfMeasure()
    }).subscribe({
      next: (data) => {
        this.categories = data.categories;
        this.types = data.types;
        this.units = data.units;
        this.loading = false;
        
        if (this.isEditMode) {
          this.loadItem();
        }
      },
      error: (error) => {
        console.error('Error loading reference data:', error);
        this.loading = false;
        this.snackBar.open('Failed to load reference data', 'Close', { duration: 3000 });
      }
    });
  }

  loadItem() {
    this.loading = true;
    this.inventoryService.getById(this.itemId).subscribe({
      next: (item) => {
        // Find the IDs based on the names returned from backend
        const category = this.categories.find(c => c.name === item.categoryName);
        const type = this.types.find(t => t.name === item.typeName);
        const unit = this.units.find(u => u.name === item.unitOfMeasureName);
        
        this.inventoryForm.patchValue({
          name: item.name,
          description: item.description,
          sku: item.sku,
          categoryId: category?.id || '',
          typeId: type?.id || '',
          unitOfMeasureId: unit?.id || '',
          minimumStockLevel: item.minimumStockLevel,
          reorderLevel: item.reorderLevel,
          unitPrice: item.unitPrice,
          isActive: item.isActive
        });
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading item:', error);
        this.loading = false;
        this.snackBar.open('Failed to load item', 'Close', { duration: 3000 });
        this.router.navigate(['/inventory']);
      }
    });
  }

  onSubmit() {
    if (this.inventoryForm.invalid) {
      return;
    }

    this.loading = true;
    const formData = this.inventoryForm.value;

    if (this.isEditMode) {
      // Add id to the DTO for update
      const updateData = {
        ...formData,
        id: this.itemId
      };
      this.inventoryService.update(this.itemId, updateData).subscribe({
        next: () => {
          this.snackBar.open('Item updated successfully', 'Close', { duration: 3000 });
          this.router.navigate(['/inventory']);
        },
        error: (error) => {
          console.error('Error updating item:', error);
          this.loading = false;
          this.snackBar.open('Failed to update item', 'Close', { duration: 3000 });
        }
      });
    } else {
      this.inventoryService.create(formData).subscribe({
        next: () => {
          this.snackBar.open('Item created successfully', 'Close', { duration: 3000 });
          this.router.navigate(['/inventory']);
        },
        error: (error) => {
          console.error('Error creating item:', error);
          this.loading = false;
          this.snackBar.open('Failed to create item', 'Close', { duration: 3000 });
        }
      });
    }
  }

  cancel() {
    this.router.navigate(['/inventory']);
  }

  get f() { return this.inventoryForm.controls; }
}
