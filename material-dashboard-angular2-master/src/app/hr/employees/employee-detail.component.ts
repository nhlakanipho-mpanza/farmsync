import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatDialog } from '@angular/material/dialog';
import { EmployeeService } from 'app/core/services/employee.service';
import { Employee, EmergencyContact, BankDetails, BiometricEnrolment } from 'app/core/models/hr.model';

@Component({
  selector: 'app-employee-detail',
  templateUrl: './employee-detail.component.html',
  styleUrls: ['./employee-detail.component.css']
})
export class EmployeeDetailComponent implements OnInit {
  employee: Employee;
  loading = false;
  employeeId: string;

  constructor(
    private employeeService: EmployeeService,
    private route: ActivatedRoute,
    private router: Router,
    private snackBar: MatSnackBar,
    private dialog: MatDialog
  ) {}

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.employeeId = params['id'];
      this.loadEmployee();
    });
  }

  loadEmployee() {
    this.loading = true;
    this.employeeService.getById(this.employeeId).subscribe({
      next: (employee) => {
        this.employee = employee;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading employee:', error);
        this.snackBar.open('Error loading employee details', 'Close', { duration: 3000 });
        this.loading = false;
      }
    });
  }

  editEmployee() {
    this.router.navigate(['/hr/employees/edit', this.employeeId]);
  }

  goBack() {
    this.router.navigate(['/hr/employees']);
  }
}
