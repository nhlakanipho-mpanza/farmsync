import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { SupplierService } from '../services/supplier.service';

@Component({
  selector: 'app-supplier-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSlideToggleModule,
    MatSnackBarModule
  ],
  templateUrl: './supplier-form.component.html',
  styleUrls: ['./supplier-form.component.scss']
})
export class SupplierFormComponent implements OnInit {
  supplierForm: FormGroup;
  isEditMode = false;
  supplierId: string | null = null;
  loading = false;

  constructor(
    private fb: FormBuilder,
    private supplierService: SupplierService,
    private route: ActivatedRoute,
    private router: Router,
    private snackBar: MatSnackBar
  ) {
    this.supplierForm = this.fb.group({
      name: ['', Validators.required],
      contactPerson: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', Validators.required],
      address: [''],
      isActive: [true]
    });
  }

  ngOnInit(): void {
    this.supplierId = this.route.snapshot.paramMap.get('id');
    if (this.supplierId) {
      this.isEditMode = true;
      this.loadSupplier();
    }
  }

  loadSupplier(): void {
    if (!this.supplierId) return;

    this.loading = true;
    this.supplierService.getById(this.supplierId).subscribe({
      next: (supplier) => {
        this.supplierForm.patchValue({
          name: supplier.name,
          contactPerson: supplier.contactPerson,
          email: supplier.email,
          phone: supplier.phone,
          address: supplier.address,
          isActive: supplier.isActive
        });
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading supplier:', error);
        this.snackBar.open('Failed to load supplier', 'Close', { duration: 3000 });
        this.loading = false;
      }
    });
  }

  onSubmit(): void {
    if (this.supplierForm.invalid) {
      return;
    }

    this.loading = true;
    const formData = this.supplierForm.value;

    if (this.isEditMode && this.supplierId) {
      this.supplierService.update(this.supplierId, formData).subscribe({
        next: () => {
          this.snackBar.open('Supplier updated successfully', 'Close', { duration: 3000 });
          this.router.navigate(['/procurement/suppliers']);
        },
        error: (error) => {
          console.error('Error updating supplier:', error);
          this.snackBar.open('Failed to update supplier', 'Close', { duration: 3000 });
          this.loading = false;
        }
      });
    } else {
      this.supplierService.create(formData).subscribe({
        next: () => {
          this.snackBar.open('Supplier created successfully', 'Close', { duration: 3000 });
          this.router.navigate(['/procurement/suppliers']);
        },
        error: (error) => {
          console.error('Error creating supplier:', error);
          this.snackBar.open('Failed to create supplier', 'Close', { duration: 3000 });
          this.loading = false;
        }
      });
    }
  }

  cancel(): void {
    this.router.navigate(['/procurement/suppliers']);
  }
}
