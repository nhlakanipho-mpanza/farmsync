import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatChipsModule } from '@angular/material/chips';
import { FormsModule } from '@angular/forms';
import { SupplierService } from '../services/supplier.service';
import { Supplier } from '../models/procurement.model';

@Component({
  selector: 'app-supplier-list',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatSnackBarModule,
    MatChipsModule
  ],
  templateUrl: './supplier-list.component.html',
  styleUrls: ['./supplier-list.component.scss']
})
export class SupplierListComponent implements OnInit {
  suppliers: Supplier[] = [];
  filteredSuppliers: Supplier[] = [];
  loading = false;
  searchTerm = '';
  displayedColumns: string[] = ['name', 'contactPerson', 'email', 'phone', 'isActive', 'actions'];

  constructor(
    private supplierService: SupplierService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.loadSuppliers();
  }

  loadSuppliers(): void {
    this.loading = true;
    this.supplierService.getAll().subscribe({
      next: (data) => {
        this.suppliers = data;
        this.filteredSuppliers = data;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading suppliers:', error);
        this.snackBar.open('Failed to load suppliers', 'Close', { duration: 3000 });
        this.loading = false;
      }
    });
  }

  createNew(): void {
    this.router.navigate(['/procurement/suppliers/new']);
  }

  onSearch(): void {
    if (!this.searchTerm.trim()) {
      this.filteredSuppliers = this.suppliers;
      return;
    }
    const term = this.searchTerm.toLowerCase();
    this.filteredSuppliers = this.suppliers.filter(supplier =>
      supplier.name.toLowerCase().includes(term) ||
      (supplier.contactPerson?.toLowerCase().includes(term)) ||
      (supplier.email?.toLowerCase().includes(term)) ||
      (supplier.phone?.toLowerCase().includes(term))
    );
  }

  editSupplier(id: string): void {
    this.router.navigate(['/procurement/suppliers/edit', id]);
  }

  deleteSupplier(id: string, name: string): void {
    if (confirm(`Are you sure you want to delete supplier "${name}"?`)) {
      this.supplierService.delete(id).subscribe({
        next: () => {
          this.snackBar.open('Supplier deleted successfully', 'Close', { duration: 3000 });
          this.loadSuppliers();
        },
        error: (error) => {
          console.error('Error deleting supplier:', error);
          this.snackBar.open('Failed to delete supplier', 'Close', { duration: 3000 });
        }
      });
    }
  }
}
