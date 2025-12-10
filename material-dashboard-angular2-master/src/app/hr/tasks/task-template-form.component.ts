import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormArray, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { TaskTemplateService } from '../../core/services/task-template.service';
import { InventoryService } from '../../core/services/inventory.service';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';

@Component({
  selector: 'app-task-template-form',
  templateUrl: './task-template-form.component.html',
  styleUrls: ['./task-template-form.component.css']
})
export class TaskTemplateFormComponent implements OnInit {
  templateForm: FormGroup;
  loading = false;
  isEditMode = false;
  templateId: string;

  categories = ['Field Work', 'Equipment', 'Livestock', 'General'];
  recurrencePatterns = ['Daily', 'Weekly', 'Monthly', 'Quarterly', 'Annually'];
  allocationMethods = [
    { value: 'PerTask', label: 'Per Task (fixed quantity)' },
    { value: 'PerTeamMember', label: 'Per Team Member' },
    { value: 'Custom', label: 'Custom (enter at assignment)' }
  ];
  
  inventoryItems: any[] = [];

  constructor(
    private fb: FormBuilder,
    private templateService: TaskTemplateService,
    private inventoryService: InventoryService,
    private router: Router,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit() {
    this.initForm();
    this.loadInventoryItems();
    
    this.route.params.subscribe(params => {
      if (params['id']) {
        this.isEditMode = true;
        this.templateId = params['id'];
        this.loadTemplate(this.templateId);
      }
    });
  }

  initForm() {
    this.templateForm = this.fb.group({
      name: ['', Validators.required],
      description: [''],
      category: ['General'],
      estimatedHours: [null],
      isRecurring: [false],
      recurrencePattern: [''],
      recurrenceInterval: [null],
      instructions: [''],
      isActive: [true],
      checklistItems: this.fb.array([]),
      inventoryItems: this.fb.array([])
    });
  }

  get checklistItems(): FormArray {
    return this.templateForm.get('checklistItems') as FormArray;
  }

  get inventoryItemsArray(): FormArray {
    return this.templateForm.get('inventoryItems') as FormArray;
  }

  loadTemplate(id: string) {
    this.loading = true;
    this.templateService.getById(id).subscribe({
      next: (template) => {
        this.templateForm.patchValue({
          name: template.name,
          description: template.description,
          category: template.category,
          estimatedHours: template.estimatedHours,
          isRecurring: template.isRecurring,
          recurrencePattern: template.recurrencePattern,
          recurrenceInterval: template.recurrenceInterval,
          instructions: template.instructions,
          isActive: template.isActive
        });

        // Load checklist items
        this.checklistItems.clear();
        if (template.checklistItems && template.checklistItems.length > 0) {
          template.checklistItems
            .sort((a, b) => a.sequence - b.sequence)
            .forEach(item => {
              this.addChecklistItem(item);
            });
        }

        // Load inventory items
        this.inventoryItemsArray.clear();
        if (template.inventoryItems && template.inventoryItems.length > 0) {
          template.inventoryItems
            .sort((a, b) => a.sequence - b.sequence)
            .forEach(item => {
              this.addInventoryItem(item);
            });
        }

        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading template:', error);
        this.snackBar.open('Error loading template', 'Close', { duration: 3000 });
        this.loading = false;
      }
    });  }

  loadInventoryItems() {
    this.inventoryService.getAll().subscribe({
      next: (items) => {
        this.inventoryItems = items;
      },
      error: (error) => {
        console.error('Error loading inventory items:', error);
      }
    });
  }

  addChecklistItem(item?: any) {
    const checklistItem = this.fb.group({
      sequence: [this.checklistItems.length + 1],
      description: [item?.description || '', Validators.required],
      isRequired: [item?.isRequired ?? true],
      notes: [item?.notes || '']
    });
    this.checklistItems.push(checklistItem);
  }

  removeChecklistItem(index: number) {
    this.checklistItems.removeAt(index);
    this.reorderChecklistItems();
  }

  dropChecklistItem(event: CdkDragDrop<any[]>) {
    moveItemInArray(this.checklistItems.controls, event.previousIndex, event.currentIndex);
    this.checklistItems.updateValueAndValidity();
    this.reorderChecklistItems();
  }

  reorderChecklistItems() {
    this.checklistItems.controls.forEach((control, index) => {
      control.patchValue({ sequence: index + 1 });
    });
  }

  addInventoryItem(item?: any) {
    const inventoryItem = this.fb.group({
      sequence: [this.inventoryItemsArray.length + 1],
      inventoryItemId: [item?.inventoryItemId || '', Validators.required],
      quantityPerUnit: [item?.quantityPerUnit || 1, [Validators.required, Validators.min(0)]],
      allocationMethod: [item?.allocationMethod || 'Custom', Validators.required],
      notes: [item?.notes || '']
    });
    this.inventoryItemsArray.push(inventoryItem);
  }

  removeInventoryItem(index: number) {
    this.inventoryItemsArray.removeAt(index);
    this.reorderInventoryItems();
  }

  dropInventoryItem(event: CdkDragDrop<any[]>) {
    moveItemInArray(this.inventoryItemsArray.controls, event.previousIndex, event.currentIndex);
    this.inventoryItemsArray.updateValueAndValidity();
    this.reorderInventoryItems();
  }

  reorderInventoryItems() {
    this.inventoryItemsArray.controls.forEach((control, index) => {
      control.patchValue({ sequence: index + 1 });
    });
  }

  getInventoryItemName(itemId: string): string {
    const item = this.inventoryItems.find(i => i.id === itemId);
    return item ? `${item.name} (${item.sku || 'N/A'})` : 'Select item';
  }

  onSubmit() {
    if (this.templateForm.invalid) {
      this.snackBar.open('Please fill in all required fields', 'Close', { duration: 3000 });
      return;
    }

    this.loading = true;
    const formValue = this.templateForm.value;

    if (this.isEditMode) {
      this.templateService.update(this.templateId, formValue).subscribe({
        next: () => {
          this.snackBar.open('Template updated successfully', 'Close', { duration: 3000 });
          this.router.navigate(['/hr/task-templates']);
        },
        error: (error) => {
          console.error('Error updating template:', error);
          this.snackBar.open('Error updating template', 'Close', { duration: 3000 });
          this.loading = false;
        }
      });
    } else {
      this.templateService.create(formValue).subscribe({
        next: () => {
          this.snackBar.open('Template created successfully', 'Close', { duration: 3000 });
          this.router.navigate(['/hr/task-templates']);
        },
        error: (error) => {
          console.error('Error creating template:', error);
          this.snackBar.open('Error creating template', 'Close', { duration: 3000 });
          this.loading = false;
        }
      });
    }
  }

  cancel() {
    this.router.navigate(['/hr/task-templates']);
  }
}
