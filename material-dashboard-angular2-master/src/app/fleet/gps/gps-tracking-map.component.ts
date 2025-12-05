import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { GPSTrackingService } from '../../core/services/gps-tracking.service';
import { VehicleService } from '../../core/services/vehicle.service';
import { GPSLocation, Vehicle } from '../../core/models/fleet.model';

declare var google: any;

@Component({
  selector: 'app-gps-tracking-map',
  templateUrl: './gps-tracking-map.component.html',
  styleUrls: ['./gps-tracking-map.component.css']
})
export class GPSTrackingMapComponent implements OnInit, OnDestroy {
  map: any;
  markers: Map<string, any> = new Map();
  vehicles: Vehicle[] = [];
  selectedVehicleId: string | null = null;
  refreshInterval: any;

  constructor(
    private gpsService: GPSTrackingService,
    private vehicleService: VehicleService,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    // Check if specific vehicle was requested
    this.route.queryParams.subscribe(params => {
      this.selectedVehicleId = params['vehicleId'] || null;
    });

    // Initialize Google Map
    this.initMap();

    // Load active vehicles and their locations
    this.loadVehicleLocations();

    // Set up auto-refresh every 30 seconds
    this.refreshInterval = setInterval(() => {
      this.loadVehicleLocations();
    }, 30000);
  }

  ngOnDestroy() {
    if (this.refreshInterval) {
      clearInterval(this.refreshInterval);
    }
  }

  initMap() {
    const mapOptions = {
      center: { lat: -29.8587, lng: 31.0218 }, // KZN, South Africa
      zoom: 10,
      mapTypeId: google.maps.MapTypeId.ROADMAP
    };

    this.map = new google.maps.Map(document.getElementById('map'), mapOptions);
  }

  loadVehicleLocations() {
    this.gpsService.getActiveVehicleLocations().subscribe({
      next: (locations) => {
        this.updateMarkers(locations);
      },
      error: (error) => {
        console.error('Error loading GPS locations:', error);
      }
    });
  }

  updateMarkers(locations: GPSLocation[]) {
    // Clear existing markers
    this.markers.forEach(marker => marker.setMap(null));
    this.markers.clear();

    // Add new markers
    locations.forEach(location => {
      if (location.latitude && location.longitude) {
        const position = { lat: location.latitude, lng: location.longitude };

        const marker = new google.maps.Marker({
          position: position,
          map: this.map,
          title: location.vehicleRegistration,
          icon: {
            url: this.getMarkerIcon(location.speed || 0),
            scaledSize: new google.maps.Size(40, 40)
          }
        });

        const infoWindow = new google.maps.InfoWindow({
          content: this.getInfoWindowContent(location)
        });

        marker.addListener('click', () => {
          infoWindow.open(this.map, marker);
        });

        this.markers.set(location.vehicleId, marker);

        // If this is the selected vehicle, center map and open info window
        if (location.vehicleId === this.selectedVehicleId) {
          this.map.setCenter(position);
          this.map.setZoom(14);
          infoWindow.open(this.map, marker);
        }
      }
    });

    // Auto-fit bounds if no specific vehicle selected
    if (!this.selectedVehicleId && this.markers.size > 0) {
      const bounds = new google.maps.LatLngBounds();
      this.markers.forEach(marker => {
        bounds.extend(marker.getPosition());
      });
      this.map.fitBounds(bounds);
    }
  }

  getMarkerIcon(speed: number): string {
    // Different colors based on speed
    if (speed > 80) {
      return 'http://maps.google.com/mapfiles/ms/icons/red-dot.png';
    } else if (speed > 40) {
      return 'http://maps.google.com/mapfiles/ms/icons/yellow-dot.png';
    } else {
      return 'http://maps.google.com/mapfiles/ms/icons/green-dot.png';
    }
  }

  getInfoWindowContent(location: GPSLocation): string {
    return `
      <div style="padding: 10px;">
        <h6 style="margin: 0 0 10px 0;"><strong>${location.vehicleRegistration}</strong></h6>
        <p style="margin: 5px 0;"><strong>Speed:</strong> ${location.speed || 0} km/h</p>
        <p style="margin: 5px 0;"><strong>Heading:</strong> ${location.heading || '-'}°</p>
        <p style="margin: 5px 0;"><strong>Last Updated:</strong> ${new Date(location.timestamp).toLocaleString()}</p>
      </div>
    `;
  }
}
