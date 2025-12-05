import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { DriverAssignmentService } from '../../../core/services/driver-assignment.service';
import { DriverAssignment } from '../../../core/models/fleet.model';

@Component({
  selector: 'app-driver-assignment-list',
  templateUrl: './driver-assignment-list.component.html',
  styleUrls: ['./driver-assignment-list.component.css']
})
export class DriverAssignmentListComponent implements OnInit {
  assignments: DriverAssignment[] = [];
  filteredAssignments: DriverAssignment[] = [];
  loading = false;
  searchTerm = '';

  constructor(
    private assignmentService: DriverAssignmentService,
    private router: Router,
    private snackBar: MatSnackBar
  ) { }

  ngOnInit(): void {
    this.loadAssignments();
  }

  loadAssignments(): void {
    this.loading = true;
    this.assignmentService.getCurrentAssignments().subscribe({
      next: (assignments) => {
        this.assignments = assignments;
        this.filteredAssignments = assignments;
        this.loading = false;
      },
      error: (error) => {
        this.loading = false;
        this.snackBar.open(`Error loading assignments: ${error.message}`, 'Close', { duration: 5000 });
      }
    });
  }

  filterAssignments(): void {
    const term = this.searchTerm.toLowerCase();
    this.filteredAssignments = this.assignments.filter(a =>
      a.vehicleRegistration?.toLowerCase().includes(term) ||
      a.vehicleMake?.toLowerCase().includes(term) ||
      a.vehicleModel?.toLowerCase().includes(term) ||
      a.driverName?.toLowerCase().includes(term) ||
      a.driverEmployeeNumber?.toLowerCase().includes(term)
    );
  }

  createAssignment(): void {
    this.router.navigate(['/fleet/assignments/create']);
  }

  viewVehicle(vehicleId: string): void {
    this.router.navigate(['/fleet/vehicles', vehicleId]);
  }

  endAssignment(assignment: DriverAssignment): void {
    if (confirm(`End assignment for ${assignment.driverName}?`)) {
      const endDate = new Date().toISOString();
      this.assignmentService.endAssignment(assignment.id, endDate).subscribe({
        next: () => {
          this.snackBar.open('Assignment ended successfully', 'Close', { duration: 3000 });
          this.loadAssignments();
        },
        error: (error) => {
          this.snackBar.open(`Error: ${error.message}`, 'Close', { duration: 5000 });
        }
      });
    }
  }
}
