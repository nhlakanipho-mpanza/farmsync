import { Component, OnInit } from '@angular/core';
import { VehicleService } from '../../core/services/vehicle.service';
import { Vehicle } from '../../core/models/fleet.model';

@Component({
  selector: 'app-fleet-dashboard',
  templateUrl: './fleet-dashboard.component.html',
  styleUrls: ['./fleet-dashboard.component.css']
})
export class FleetDashboardComponent implements OnInit {
  totalVehicles = 0;
  activeVehicles = 0;
  maintenanceDue = 0;
  loading = false;

  recentVehicles: Vehicle[] = [];
  maintenanceVehicles: Vehicle[] = [];

  constructor(private vehicleService: VehicleService) {}

  ngOnInit() {
    this.loadDashboardData();
  }

  loadDashboardData() {
    this.loading = true;

    // Load all vehicles count
    this.vehicleService.getAll().subscribe({
      next: (vehicles) => {
        this.totalVehicles = vehicles.length;
      }
    });

    // Load active vehicles count
    this.vehicleService.getActive().subscribe({
      next: (vehicles) => {
        this.activeVehicles = vehicles.length;
        this.recentVehicles = vehicles.slice(0, 5); // Get first 5 for recent list
        this.loading = false;
      }
    });

    // Load maintenance due count
    this.vehicleService.getDueForMaintenance().subscribe({
      next: (vehicles) => {
        this.maintenanceDue = vehicles.length;
        this.maintenanceVehicles = vehicles.slice(0, 5);
      }
    });
  }
}
