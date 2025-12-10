import { Component, OnInit } from '@angular/core';
import { DashboardResolverService } from '../../core/services/dashboard-resolver.service';

@Component({
  selector: 'app-dashboard-router',
  template: '<div>Loading...</div>',
  standalone: false
})
export class DashboardRouterComponent implements OnInit {
  constructor(private dashboardResolver: DashboardResolverService) {}

  ngOnInit() {
    this.dashboardResolver.navigateToDashboard();
  }
}
