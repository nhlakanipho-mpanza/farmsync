import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { InventoryService } from 'app/core/services/inventory.service';
import { InventoryItem } from 'app/core/models/inventory.model';

@Component({
  selector: 'app-inventory-list',
  templateUrl: './inventory-list.component.html',
  styleUrls: ['./inventory-list.component.css']
})
export class InventoryListComponent implements OnInit {
  inventoryItems: InventoryItem[] = [];
  filteredItems: InventoryItem[] = [];
  loading = false;
  searchTerm = '';
  selectedCategory = '';

  constructor(
    private inventoryService: InventoryService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit() {
    this.loadInventory();
  }

  loadInventory() {
    this.loading = true;
    this.inventoryService.getAll().subscribe({
      next: (items) => {
        this.inventoryItems = items;
        this.filteredItems = items;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading inventory:', error);
        this.loading = false;
      }
    });
  }

  filterItems() {
    this.filteredItems = this.inventoryItems.filter(item => {
      const matchesSearch = !this.searchTerm || 
        item.name.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        item.description?.toLowerCase().includes(this.searchTerm.toLowerCase());
      
      const matchesCategory = !this.selectedCategory || item.categoryName === this.selectedCategory;

      return matchesSearch && matchesCategory;
    });
  }

  onCategoryChange() {
    this.filterItems();
  }

  onSearch() {
    this.filterItems();
  }

  addItem() {
    this.router.navigate(['/inventory/create']);
  }

  editItem(id: string) {
    this.router.navigate(['/inventory/edit', id]);
  }

  deleteItem(id: string) {
    if (confirm('Are you sure you want to delete this item?')) {
      this.inventoryService.delete(id).subscribe({
        next: () => {
          this.snackBar.open('Item deleted successfully', 'Close', { duration: 3000 });
          this.loadInventory();
        },
        error: (error) => {
          console.error('Error deleting item:', error);
          this.snackBar.open('Failed to delete item', 'Close', { duration: 3000 });
        }
      });
    }
  }

  isLowStock(item: InventoryItem): boolean {
    return item.totalStock <= item.reorderLevel;
  }
}
