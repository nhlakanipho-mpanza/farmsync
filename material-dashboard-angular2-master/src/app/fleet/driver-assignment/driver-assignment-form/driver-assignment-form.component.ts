import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { DriverAssignmentService } from '../../../core/services/driver-assignment.service';
import { VehicleService } from '../../../core/services/vehicle.service';
import { EmployeeService } from '../../../core/services/employee.service';
import { Vehicle } from '../../../core/models/fleet.model';
import { Employee } from '../../../core/models/hr.model';

@Component({
  selector: 'app-driver-assignment-form',
  templateUrl: './driver-assignment-form.component.html',
  styleUrls: ['./driver-assignment-form.component.css']
})
export class DriverAssignmentFormComponent implements OnInit {
  assignmentForm: FormGroup;
  vehicles: Vehicle[] = [];
  drivers: Employee[] = [];
  loading = false;
  vehicleId?: string;
  assignmentTypes = ['Primary', 'Temporary', 'Pool'];

  constructor(
    private fb: FormBuilder,
    private assignmentService: DriverAssignmentService,
    private vehicleService: VehicleService,
    private employeeService: EmployeeService,
    private route: ActivatedRoute,
    private router: Router,
    private snackBar: MatSnackBar
  ) {
    this.assignmentForm = this.fb.group({
      vehicleId: ['', Validators.required],
      driverId: ['', Validators.required],
      startDate: [new Date().toISOString().split('T')[0], Validators.required],
      endDate: [''],
      assignmentType: ['Primary', Validators.required],
      isPrimary: [true],
      notes: ['']
    });
  }

  ngOnInit(): void {
    this.vehicleId = this.route.snapshot.queryParams['vehicleId'];
    if (this.vehicleId) {
      this.assignmentForm.patchValue({ vehicleId: this.vehicleId });
    }
    
    this.loadReferenceData();
  }

  loadReferenceData(): void {
    this.vehicleService.getActive().subscribe(vehicles => {
      this.vehicles = vehicles;
    });

    this.employeeService.getActive().subscribe(employees => {
      this.drivers = employees;
    });
  }

  onSubmit(): void {
    if (this.assignmentForm.invalid) {
      return;
    }

    this.loading = true;
    const formValue = this.assignmentForm.value;

    // Format dates for API
    const assignmentData = {
      ...formValue,
      startDate: new Date(formValue.startDate).toISOString(),
      endDate: formValue.endDate ? new Date(formValue.endDate).toISOString() : null
    };

    this.assignmentService.assignDriver(assignmentData).subscribe({
      next: () => {
        this.snackBar.open('Driver assigned successfully', 'Close', { duration: 3000 });
        if (this.vehicleId) {
          this.router.navigate(['/fleet/vehicles', this.vehicleId]);
        } else {
          this.router.navigate(['/fleet/assignments']);
        }
      },
      error: (error) => {
        this.loading = false;
        this.snackBar.open(`Error: ${error.error?.message || error.message}`, 'Close', { duration: 5000 });
      }
    });
  }

  cancel(): void {
    if (this.vehicleId) {
      this.router.navigate(['/fleet/vehicles', this.vehicleId]);
    } else {
      this.router.navigate(['/fleet/assignments']);
    }
  }
}
