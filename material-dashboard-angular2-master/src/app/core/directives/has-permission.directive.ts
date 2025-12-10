import { Directive, Input, TemplateRef, ViewContainerRef, OnInit, OnDestroy } from '@angular/core';
import { PermissionService } from '../services/permission.service';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { AuthService } from '../services/auth.service';

@Directive({
  selector: '[hasPermission]'
})
export class HasPermissionDirective implements OnInit, OnDestroy {
  private permission: string = '';
  private destroy$ = new Subject<void>();

  constructor(
    private templateRef: TemplateRef<any>,
    private viewContainer: ViewContainerRef,
    private permissionService: PermissionService,
    private authService: AuthService
  ) { }

  @Input() set hasPermission(permission: string) {
    this.permission = permission;
    this.updateView();
  }

  ngOnInit() {
    // Re-check permissions when user changes
    this.authService.currentUser
      .pipe(takeUntil(this.destroy$))
      .subscribe(() => {
        this.updateView();
      });
  }

  private updateView() {
    this.viewContainer.clear(); // Clear any existing view first
    if (this.permissionService.can(this.permission)) {
      this.viewContainer.createEmbeddedView(this.templateRef);
    }
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
