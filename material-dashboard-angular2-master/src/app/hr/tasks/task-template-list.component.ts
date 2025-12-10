import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { TaskTemplateService } from '../../core/services/task-template.service';

@Component({
  selector: 'app-task-template-list',
  templateUrl: './task-template-list.component.html',
  styleUrls: ['./task-template-list.component.css']
})
export class TaskTemplateListComponent implements OnInit {
  templates: any[] = [];
  filteredTemplates: any[] = [];
  loading = false;
  selectedCategory: string = 'all';
  searchQuery: string = '';

  categories: string[] = ['All', 'Field Work', 'Equipment', 'Livestock', 'General'];

  displayedColumns: string[] = ['name', 'category', 'estimatedHours', 'recurring', 'checklistItems', 'actions'];

  constructor(
    private templateService: TaskTemplateService,
    private router: Router,
    private dialog: MatDialog,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit() {
    this.loadTemplates();
  }

  loadTemplates() {
    this.loading = true;
    this.templateService.getAll().subscribe({
      next: (data) => {
        this.templates = data;
        this.applyFilters();
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading templates:', error);
        this.snackBar.open('Error loading templates', 'Close', { duration: 3000 });
        this.loading = false;
      }
    });
  }

  applyFilters() {
    this.filteredTemplates = this.templates.filter(template => {
      const matchesCategory = this.selectedCategory === 'all' || 
        template.category?.toLowerCase() === this.selectedCategory.toLowerCase();
      const matchesSearch = !this.searchQuery || 
        template.name.toLowerCase().includes(this.searchQuery.toLowerCase()) ||
        template.description?.toLowerCase().includes(this.searchQuery.toLowerCase());
      return matchesCategory && matchesSearch;
    });
  }

  onCategoryChange(category: string) {
    this.selectedCategory = category.toLowerCase();
    this.applyFilters();
  }

  onSearchChange() {
    this.applyFilters();
  }

  createTemplate() {
    this.router.navigate(['/hr/task-templates/new']);
  }

  editTemplate(id: string) {
    this.router.navigate(['/hr/task-templates/edit', id]);
  }

  viewTemplate(id: string) {
    this.router.navigate(['/hr/task-templates', id]);
  }

  deleteTemplate(id: string, name: string) {
    if (confirm(`Are you sure you want to delete template "${name}"?`)) {
      this.templateService.delete(id).subscribe({
        next: () => {
          this.snackBar.open('Template deleted successfully', 'Close', { duration: 3000 });
          this.loadTemplates();
        },
        error: (error) => {
          console.error('Error deleting template:', error);
          this.snackBar.open('Error deleting template', 'Close', { duration: 3000 });
        }
      });
    }
  }

  toggleActive(template: any) {
    const updatedTemplate = { ...template, isActive: !template.isActive };
    this.templateService.update(template.id, updatedTemplate).subscribe({
      next: () => {
        this.snackBar.open(
          `Template ${updatedTemplate.isActive ? 'activated' : 'deactivated'}`,
          'Close',
          { duration: 3000 }
        );
        this.loadTemplates();
      },
      error: (error) => {
        console.error('Error updating template:', error);
        this.snackBar.open('Error updating template', 'Close', { duration: 3000 });
      }
    });
  }
}
