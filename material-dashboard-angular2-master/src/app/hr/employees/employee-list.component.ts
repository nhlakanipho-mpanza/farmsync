import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { EmployeeService } from 'app/core/services/employee.service';
import { Employee } from 'app/core/models/hr.model';

@Component({
  selector: 'app-employee-list',
  templateUrl: './employee-list.component.html',
  styleUrls: ['./employee-list.component.css']
})
export class EmployeeListComponent implements OnInit {
  employees: Employee[] = [];
  filteredEmployees: Employee[] = [];
  loading = false;
  searchTerm = '';
  selectedPosition = '';
  selectedEmploymentType = '';
  showActiveOnly = true;

  constructor(
    private employeeService: EmployeeService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit() {
    this.loadEmployees();
  }

  loadEmployees() {
    this.loading = true;
    const request = this.showActiveOnly 
      ? this.employeeService.getActive() 
      : this.employeeService.getAll();
    
    request.subscribe({
      next: (employees) => {
        this.employees = employees;
        this.filteredEmployees = employees;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading employees:', error);
        this.snackBar.open('Error loading employees', 'Close', { duration: 3000 });
        this.loading = false;
      }
    });
  }

  filterEmployees() {
    this.filteredEmployees = this.employees.filter(emp => {
      const matchesSearch = !this.searchTerm || 
        emp.fullName.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        emp.employeeNumber.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        emp.email?.toLowerCase().includes(this.searchTerm.toLowerCase());
      
      const matchesPosition = !this.selectedPosition || emp.positionName === this.selectedPosition;
      const matchesEmploymentType = !this.selectedEmploymentType || emp.employmentTypeName === this.selectedEmploymentType;

      return matchesSearch && matchesPosition && matchesEmploymentType;
    });
  }

  onSearch() {
    this.filterEmployees();
  }

  onFilterChange() {
    this.filterEmployees();
  }

  onActiveToggle() {
    this.loadEmployees();
  }

  addEmployee() {
    this.router.navigate(['/hr/employees/create']);
  }

  viewEmployee(id: string) {
    this.router.navigate(['/hr/employees', id]);
  }

  editEmployee(id: string) {
    this.router.navigate(['/hr/employees/edit', id]);
  }

  deleteEmployee(id: string, name: string) {
    if (confirm(`Are you sure you want to delete ${name}?`)) {
      this.employeeService.delete(id).subscribe({
        next: () => {
          this.snackBar.open('Employee deleted successfully', 'Close', { duration: 3000 });
          this.loadEmployees();
        },
        error: (error) => {
          console.error('Error deleting employee:', error);
          this.snackBar.open('Error deleting employee', 'Close', { duration: 3000 });
        }
      });
    }
  }

  get uniquePositions(): string[] {
    return [...new Set(this.employees.map(e => e.positionName).filter(Boolean))];
  }

  get uniqueEmploymentTypes(): string[] {
    return [...new Set(this.employees.map(e => e.employmentTypeName).filter(Boolean))];
  }
}
