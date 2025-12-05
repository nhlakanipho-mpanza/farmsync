import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { VehicleService } from '../../core/services/vehicle.service';
import { Vehicle } from '../../core/models/fleet.model';

@Component({
  selector: 'app-vehicle-list',
  templateUrl: './vehicle-list.component.html',
  styleUrls: ['./vehicle-list.component.css']
})
export class VehicleListComponent implements OnInit {
  vehicles: Vehicle[] = [];
  filteredVehicles: Vehicle[] = [];
  loading = false;
  searchTerm = '';
  showActiveOnly = true;
  displayedColumns: string[] = ['registrationNumber', 'make', 'model', 'year', 'vehicleType', 'status', 'primaryDriver', 'odometer', 'actions'];

  constructor(
    private vehicleService: VehicleService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit() {
    this.loadVehicles();
  }

  loadVehicles() {
    this.loading = true;
    const request = this.showActiveOnly 
      ? this.vehicleService.getActive() 
      : this.vehicleService.getAll();
    
    request.subscribe({
      next: (vehicles) => {
        this.vehicles = vehicles;
        this.filteredVehicles = vehicles;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading vehicles:', error);
        this.snackBar.open('Error loading vehicles', 'Close', { duration: 3000 });
        this.loading = false;
      }
    });
  }

  applyFilter() {
    const term = this.searchTerm.toLowerCase();
    this.filteredVehicles = this.vehicles.filter(v =>
      v.registrationNumber.toLowerCase().includes(term) ||
      v.make.toLowerCase().includes(term) ||
      v.model.toLowerCase().includes(term) ||
      v.vehicleTypeName?.toLowerCase().includes(term)
    );
  }

  toggleActiveFilter() {
    this.showActiveOnly = !this.showActiveOnly;
    this.loadVehicles();
  }

  addVehicle() {
    this.router.navigate(['/fleet/vehicles/create']);
  }

  viewVehicle(id: string) {
    this.router.navigate(['/fleet/vehicles', id]);
  }

  editVehicle(id: string) {
    this.router.navigate(['/fleet/vehicles/edit', id]);
  }

  trackVehicle(id: string) {
    this.router.navigate(['/fleet/gps'], { queryParams: { vehicleId: id } });
  }

  deleteVehicle(id: string, registration: string) {
    if (confirm(`Are you sure you want to delete vehicle ${registration}?`)) {
      this.vehicleService.delete(id).subscribe({
        next: () => {
          this.snackBar.open('Vehicle deleted successfully', 'Close', { duration: 3000 });
          this.loadVehicles();
        },
        error: (error) => {
          console.error('Error deleting vehicle:', error);
          this.snackBar.open('Error deleting vehicle', 'Close', { duration: 3000 });
        }
      });
    }
  }
}
