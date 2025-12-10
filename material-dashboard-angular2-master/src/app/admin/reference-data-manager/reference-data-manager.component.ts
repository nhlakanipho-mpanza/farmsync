import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatDialog } from '@angular/material/dialog';
import { ReferenceDataService, ReferenceDto, CreateReferenceDto, UpdateReferenceDto } from '../../core/services/reference-data.service';
import { REFERENCE_CATEGORIES, CategoryConfig, TableConfig } from './reference-config';
import { ReplaceReferenceDialogComponent, ReplaceDialogData } from './replace-reference-dialog.component';
import { Observable } from 'rxjs';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-reference-data-manager',
  templateUrl: './reference-data-manager.component.html',
  styleUrls: ['./reference-data-manager.component.css']
})
export class ReferenceDataManagerComponent implements OnInit {
  categories: CategoryConfig[] = REFERENCE_CATEGORIES;
  currentCategory: CategoryConfig = this.categories[0];
  currentTables: TableConfig[] = this.categories[0].tables;
  currentTable: TableConfig = this.currentTables[0];

  data: ReferenceDto[] = [];
  filteredData: ReferenceDto[] = [];
  searchTerm = '';
  showActiveOnly = false;
  loading = false;
  editingId: string | null = null;
  editFormControl: FormControl | null = null;
  editDescriptionControl: FormControl | null = null;
  editHourlyRateControl: FormControl | null = null;
  editIsDriverPositionControl: FormControl | null = null;
  
  addForm: FormGroup;
  showAddForm = false;

  constructor(
    private referenceService: ReferenceDataService,
    private fb: FormBuilder,
    private snackBar: MatSnackBar,
    private dialog: MatDialog
  ) {
    this.addForm = this.fb.group({
      name: ['', Validators.required],
      description: [''],
      isActive: [true],
      hourlyRate: [0], // For positions table
      isDriverPosition: [false] // For positions table
    });
  }

  ngOnInit(): void {
    this.loadTableData();
  }

  selectCategory(category: CategoryConfig): void {
    this.currentCategory = category;
    this.currentTables = category.tables;
    this.selectTable(this.currentTables[0]);
  }

  selectTable(table: TableConfig): void {
    this.currentTable = table;
    this.showAddForm = false;
    this.cancelEdit();
    this.loadTableData();
  }

  loadTableData(): void {
    this.loading = true;
    this.searchTerm = '';
    this.referenceService.getTableData(this.currentTable.key)
      .pipe(finalize(() => this.loading = false))
      .subscribe({
        next: (data) => {
          this.data = data;
          this.applyFilters();
        },
        error: (error) => {
          console.error('Error loading data:', error);
          this.snackBar.open('Failed to load data', 'Close', { duration: 3000 });
        }
      });
  }

  applyFilters(): void {
    let filtered = this.data;

    // Filter by active status
    if (this.showActiveOnly) {
      filtered = filtered.filter(item => item.isActive);
    }

    // Filter by search term
    if (this.searchTerm.trim()) {
      const term = this.searchTerm.toLowerCase();
      filtered = filtered.filter(item =>
        item.name.toLowerCase().includes(term) ||
        (item.description && item.description.toLowerCase().includes(term))
      );
    }

    this.filteredData = filtered;
  }

  onActiveToggle(): void {
    this.applyFilters();
  }

  onSearch(): void {
    this.applyFilters();
  }

  toggleAddForm(): void {
    this.showAddForm = !this.showAddForm;
    if (this.showAddForm) {
      this.addForm.reset({ isActive: true, hourlyRate: 0 });
    }
  }

  addItem(): void {
    if (this.addForm.invalid) {
      return;
    }

    const dto: any = {
      name: this.addForm.value.name,
      description: this.addForm.value.description,
      isActive: this.addForm.value.isActive
    };

    // Add rate for positions
    if (this.currentTable.hasHourlyRate) {
      dto.rate = this.addForm.value.hourlyRate;
    }

    this.referenceService.create(this.currentTable.key, dto)
      .subscribe({
        next: (newItem) => {
          this.data.push(newItem);
          this.applyFilters();
          this.snackBar.open(`${this.currentTable.label} added successfully`, 'Close', { duration: 3000 });
          this.addForm.reset({ isActive: true, hourlyRate: 0, isDriverPosition: false });
          this.showAddForm = false;
        },
        error: (error) => {
          console.error('Error adding item:', error);
          this.snackBar.open('Failed to add item', 'Close', { duration: 3000 });
        }
      });
  }

  startEdit(item: ReferenceDto): void {
    this.editingId = item.id;
    this.editFormControl = new FormControl(item.name, Validators.required);
    this.editDescriptionControl = new FormControl(item.description || '');
    
    if (this.currentTable.hasHourlyRate && (item as any).rate !== undefined) {
      this.editHourlyRateControl = new FormControl((item as any).rate);
    }
    
    if (this.currentTable.hasDriverFlag && (item as any).isDriverPosition !== undefined) {
      this.editIsDriverPositionControl = new FormControl((item as any).isDriverPosition);
    }
  }

  saveEdit(item: ReferenceDto): void {
    if (!this.editFormControl || this.editFormControl.invalid) {
      return;
    }

    const dto: any = {
      name: this.editFormControl.value,
      description: this.editDescriptionControl?.value || '',
      isActive: item.isActive
    };

    // Include rate if it exists
    if (this.currentTable.hasHourlyRate && this.editHourlyRateControl) {
      dto.rate = this.editHourlyRateControl.value;
    }
    
    // Include driver flag if it exists
    if (this.currentTable.hasDriverFlag && this.editIsDriverPositionControl !== null) {
      dto.isDriverPosition = this.editIsDriverPositionControl.value;
    }

    this.referenceService.update(this.currentTable.key, item.id, dto)
      .subscribe({
        next: (updated) => {
          const index = this.data.findIndex(i => i.id === item.id);
          if (index !== -1) {
            this.data[index] = updated;
          }
          this.applyFilters();
          this.snackBar.open('Updated successfully', 'Close', { duration: 3000 });
          this.cancelEdit();
        },
        error: (error) => {
          console.error('Error updating item:', error);
          this.snackBar.open('Failed to update item', 'Close', { duration: 3000 });
        }
      });
  }

  cancelEdit(): void {
    this.editingId = null;
    this.editFormControl = null;
    this.editDescriptionControl = null;
    this.editHourlyRateControl = null;
    this.editIsDriverPositionControl = null;
  }

  deleteItem(item: ReferenceDto): void {
    // First check usage
    this.referenceService.getUsage(this.currentTable.key, item.id)
      .subscribe({
        next: (usage) => {
          if (usage.usageCount === 0) {
            // Safe to delete
            if (confirm(`Are you sure you want to delete "${item.name}"?`)) {
              this.performDelete(item.id);
            }
          } else {
            // Has dependencies - need replacement
            // Get available replacements (all other active items)
            const availableReplacements = this.data.filter(d => 
              d.id !== item.id && d.isActive
            );

            if (availableReplacements.length === 0) {
              this.snackBar.open(
                'Cannot delete. This is the only active item and it\'s in use.',
                'Close',
                { duration: 5000 }
              );
              return;
            }

            const dialogRef = this.dialog.open(ReplaceReferenceDialogComponent, {
              width: '500px',
              data: {
                itemToDelete: item,
                usageCount: usage.usageCount,
                usedInTables: usage.usedInTables,
                availableReplacements: availableReplacements
              } as ReplaceDialogData
            });

            dialogRef.afterClosed().subscribe(replacementId => {
              if (replacementId) {
                this.performReplaceAndDelete(item.id, replacementId);
              }
            });
          }
        },
        error: (error) => {
          console.error('Error checking usage:', error);
          this.snackBar.open('Failed to check item usage', 'Close', { duration: 3000 });
        }
      });
  }

  private performDelete(id: string): void {
    this.referenceService.delete(this.currentTable.key, id)
      .subscribe({
        next: () => {
          this.data = this.data.filter(i => i.id !== id);
          this.applyFilters();
          this.snackBar.open('Deleted successfully', 'Close', { duration: 3000 });
        },
        error: (error) => {
          console.error('Error deleting item:', error);
          this.snackBar.open('Failed to delete item', 'Close', { duration: 3000 });
        }
      });
  }

  private performReplaceAndDelete(sourceId: string, targetId: string): void {
    this.referenceService.replaceAndDelete(this.currentTable.key, sourceId, targetId)
      .subscribe({
        next: (result) => {
          if (result.success) {
            this.data = this.data.filter(i => i.id !== sourceId);
            this.applyFilters();
            this.snackBar.open(result.message, 'Close', { duration: 3000 });
          } else {
            this.snackBar.open(result.message, 'Close', { duration: 5000 });
          }
        },
        error: (error) => {
          console.error('Error replacing and deleting:', error);
          this.snackBar.open('Failed to replace and delete item', 'Close', { duration: 3000 });
        }
      });
  }

  toggleActive(item: ReferenceDto): void {
    const dto: UpdateReferenceDto = {
      name: item.name,
      description: item.description,
      isActive: !item.isActive
    };

    this.referenceService.update(this.currentTable.key, item.id, dto)
      .subscribe({
        next: (updated) => {
          const index = this.data.findIndex(i => i.id === item.id);
          if (index !== -1) {
            this.data[index] = updated;
          }
          this.applyFilters();
          this.snackBar.open('Status updated successfully', 'Close', { duration: 3000 });
        },
        error: (error) => {
          console.error('Error updating status:', error);
          this.snackBar.open('Failed to update status', 'Close', { duration: 3000 });
        }
      });
  }
}
