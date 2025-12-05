import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { VehicleService } from '../../core/services/vehicle.service';
import { ReferenceDataService } from '../../core/services/reference-data.service';
import { EmployeeService } from '../../core/services/employee.service';
import { Vehicle, CreateVehicleDTO, UpdateVehicleDTO } from '../../core/models/fleet.model';

@Component({
  selector: 'app-vehicle-form',
  templateUrl: './vehicle-form.component.html',
  styleUrls: ['./vehicle-form.component.css']
})
export class VehicleFormComponent implements OnInit {
  vehicleForm: FormGroup;
  isEditMode = false;
  vehicleId: string = '';
  loading = false;

  vehicleTypes: any[] = [];
  vehicleStatuses: any[] = [];
  fuelTypes: any[] = [];
  serviceTypes = [
    { value: 'Minor', label: 'Minor Service (10,000 - 15,000 KM)' },
    { value: 'Major', label: 'Major Service (30,000 - 45,000 KM)' }
  ];

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private vehicleService: VehicleService,
    private referenceDataService: ReferenceDataService,
    private snackBar: MatSnackBar
  ) {
    this.vehicleForm = this.fb.group({
      registrationNumber: ['', Validators.required],
      make: ['', Validators.required],
      model: ['', Validators.required],
      year: [new Date().getFullYear(), [Validators.required, Validators.min(1900), Validators.max(2100)]],
      engineNumber: [''],
      chassisNumber: [''],
      assetNumber: [''],
      currentOdometer: [0, [Validators.required, Validators.min(0)]],
      purchaseDate: [''],
      purchasePrice: [0],
      notes: [''],
      vehicleTypeId: ['', Validators.required],
      vehicleStatusId: ['', Validators.required],
      fuelTypeId: ['', Validators.required],
      // Maintenance tracking
      lastServiceDate: [''],
      lastServiceOdometer: [0],
      lastServiceType: [''], // 'Minor' or 'Major'
      // License disk renewal
      licenseDiskExpiryDate: [''],
      isActive: [true]
    });
  }

  ngOnInit() {
    this.loadReferenceData();

    this.route.params.subscribe(params => {
      if (params['id']) {
        this.isEditMode = true;
        this.vehicleId = params['id'];
        this.loadVehicle();
      }
    });
  }

  loadReferenceData() {
    this.referenceDataService.getVehicleTypes().subscribe(types => this.vehicleTypes = types);
    this.referenceDataService.getVehicleStatuses().subscribe(statuses => this.vehicleStatuses = statuses);
    this.referenceDataService.getFuelTypes().subscribe(types => this.fuelTypes = types);
  }

  loadVehicle() {
    this.loading = true;
    this.vehicleService.getById(this.vehicleId).subscribe({
      next: (vehicle) => {
        this.vehicleForm.patchValue({
          registrationNumber: vehicle.registrationNumber,
          make: vehicle.make,
          model: vehicle.model,
          year: vehicle.year,
          engineNumber: vehicle.engineNumber,
          chassisNumber: vehicle.chassisNumber,
          assetNumber: vehicle.assetNumber,
          currentOdometer: vehicle.currentOdometer,
          purchaseDate: vehicle.purchaseDate,
          purchasePrice: vehicle.purchasePrice,
          notes: vehicle.notes,
          vehicleTypeId: vehicle.vehicleTypeId,
          vehicleStatusId: vehicle.vehicleStatusId,
          fuelTypeId: vehicle.fuelTypeId,
          lastServiceDate: vehicle.lastServiceDate,
          lastServiceOdometer: vehicle.lastServiceOdometer,
          lastServiceType: vehicle.lastServiceType,
          licenseDiskExpiryDate: vehicle.licenseDiskExpiryDate,
          isActive: vehicle.isActive
        });
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading vehicle:', error);
        this.snackBar.open('Error loading vehicle', 'Close', { duration: 3000 });
        this.loading = false;
      }
    });
  }

  onSubmit() {
    if (this.vehicleForm.valid) {
      this.loading = true;
      const formValue = this.vehicleForm.value;

      if (this.isEditMode) {
        const updateData: UpdateVehicleDTO = formValue;
        this.vehicleService.update(this.vehicleId, updateData).subscribe({
          next: () => {
            this.snackBar.open('Vehicle updated successfully', 'Close', { duration: 3000 });
            this.router.navigate(['/fleet/vehicles']);
          },
          error: (error) => {
            console.error('Error updating vehicle:', error);
            this.snackBar.open('Error updating vehicle', 'Close', { duration: 3000 });
            this.loading = false;
          }
        });
      } else {
        const createData: CreateVehicleDTO = formValue;
        this.vehicleService.create(createData).subscribe({
          next: () => {
            this.snackBar.open('Vehicle created successfully', 'Close', { duration: 3000 });
            this.router.navigate(['/fleet/vehicles']);
          },
          error: (error) => {
            console.error('Error creating vehicle:', error);
            this.snackBar.open('Error creating vehicle', 'Close', { duration: 3000 });
            this.loading = false;
          }
        });
      }
    }
  }

  onCancel() {
    this.router.navigate(['/fleet/vehicles']);
  }
}
