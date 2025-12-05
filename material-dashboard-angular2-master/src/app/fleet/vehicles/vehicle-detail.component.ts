import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { VehicleService } from '../../core/services/vehicle.service';
import { DriverAssignmentService } from '../../core/services/driver-assignment.service';
import { Vehicle, DriverAssignment } from '../../core/models/fleet.model';

@Component({
  selector: 'app-vehicle-detail',
  templateUrl: './vehicle-detail.component.html',
  styleUrls: ['./vehicle-detail.component.css']
})
export class VehicleDetailComponent implements OnInit {
  vehicle: Vehicle | null = null;
  currentAssignment: DriverAssignment | null = null;
  assignmentHistory: DriverAssignment[] = [];
  vehicleId: string = '';
  loading = false;
  loadingAssignment = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private vehicleService: VehicleService,
    private assignmentService: DriverAssignmentService,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit() {
    this.route.params.subscribe(params => {
      if (params['id']) {
        this.vehicleId = params['id'];
        this.loadVehicle();
        this.loadCurrentAssignment();
        this.loadAssignmentHistory();
      }
    });
  }

  loadVehicle() {
    this.loading = true;
    this.vehicleService.getById(this.vehicleId).subscribe({
      next: (vehicle) => {
        this.vehicle = vehicle;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading vehicle:', error);
        this.loading = false;
      }
    });
  }

  loadCurrentAssignment() {
    this.loadingAssignment = true;
    this.assignmentService.getCurrentByVehicle(this.vehicleId).subscribe({
      next: (assignment) => {
        this.currentAssignment = assignment;
        this.loadingAssignment = false;
      },
      error: () => {
        this.currentAssignment = null;
        this.loadingAssignment = false;
      }
    });
  }

  loadAssignmentHistory() {
    this.assignmentService.getByVehicle(this.vehicleId).subscribe({
      next: (assignments) => {
        this.assignmentHistory = assignments;
      },
      error: (error) => {
        console.error('Error loading assignment history:', error);
      }
    });
  }

  assignDriver() {
    this.router.navigate(['/fleet/assignments/create'], { queryParams: { vehicleId: this.vehicleId } });
  }

  endCurrentAssignment() {
    if (!this.currentAssignment) return;
    
    if (confirm(`End assignment for ${this.currentAssignment.driverName}?`)) {
      const endDate = new Date().toISOString();
      this.assignmentService.endAssignment(this.currentAssignment.id, endDate).subscribe({
        next: () => {
          this.snackBar.open('Assignment ended successfully', 'Close', { duration: 3000 });
          this.loadCurrentAssignment();
          this.loadAssignmentHistory();
        },
        error: (error) => {
          this.snackBar.open(`Error: ${error.message}`, 'Close', { duration: 5000 });
        }
      });
    }
  }

  editVehicle() {
    this.router.navigate(['/fleet/vehicles/edit', this.vehicleId]);
  }

  trackVehicle() {
    this.router.navigate(['/fleet/gps'], { queryParams: { vehicleId: this.vehicleId } });
  }

  onBack() {
    this.router.navigate(['/fleet/vehicles']);
  }
}