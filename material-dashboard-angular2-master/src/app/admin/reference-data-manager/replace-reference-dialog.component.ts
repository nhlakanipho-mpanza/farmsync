import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ReferenceDto } from '../../core/services/reference-data.service';

export interface ReplaceDialogData {
  itemToDelete: ReferenceDto;
  usageCount: number;
  usedInTables: string[];
  availableReplacements: ReferenceDto[];
}

@Component({
  selector: 'app-replace-reference-dialog',
  template: `
    <h2 mat-dialog-title>Replace and Delete Reference</h2>
    <mat-dialog-content>
      <div class="alert alert-warning">
        <i class="material-icons">warning</i>
        <strong>"{{ data.itemToDelete.name }}"</strong> is currently used by <strong>{{ data.usageCount }}</strong> record(s).
      </div>
      
      <p class="text-muted mb-3">
        <small>Used in: {{ data.usedInTables.join(', ') }}</small>
      </p>

      <p>Select a replacement item. All references will be updated before deletion:</p>

      <mat-form-field appearance="outline" class="w-100">
        <mat-label>Replacement Item</mat-label>
        <mat-select [(value)]="selectedReplacementId" required>
          <mat-option *ngFor="let item of data.availableReplacements" [value]="item.id">
            {{ item.name }}
            <span class="text-muted" *ngIf="item.description"> - {{ item.description }}</span>
          </mat-option>
        </mat-select>
        <mat-hint>Choose which item should replace "{{ data.itemToDelete.name }}"</mat-hint>
      </mat-form-field>
    </mat-dialog-content>

    <mat-dialog-actions align="end">
      <button mat-button (click)="onCancel()">Cancel</button>
      <button 
        mat-raised-button 
        color="warn" 
        (click)="onConfirm()" 
        [disabled]="!selectedReplacementId">
        <i class="material-icons">swap_horiz</i>
        Replace & Delete
      </button>
    </mat-dialog-actions>
  `,
  styles: [`
    .alert {
      padding: 12px;
      border-radius: 4px;
      background-color: #fff3cd;
      border: 1px solid #ffc107;
      display: flex;
      align-items: center;
      gap: 8px;
      margin-bottom: 16px;
    }
    .alert i {
      color: #ff9800;
    }
    .w-100 {
      width: 100%;
    }
    mat-dialog-content {
      min-width: 400px;
      padding: 20px 24px;
    }
    .text-muted {
      color: #999;
    }
    button i {
      font-size: 18px;
      margin-right: 4px;
      vertical-align: middle;
    }
  `]
})
export class ReplaceReferenceDialogComponent {
  selectedReplacementId: string | null = null;

  constructor(
    public dialogRef: MatDialogRef<ReplaceReferenceDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ReplaceDialogData
  ) {}

  onCancel(): void {
    this.dialogRef.close(null);
  }

  onConfirm(): void {
    if (this.selectedReplacementId) {
      this.dialogRef.close(this.selectedReplacementId);
    }
  }
}
