import { Directive, Input, TemplateRef, ViewContainerRef, OnInit, OnDestroy } from '@angular/core';
import { PermissionService } from '../services/permission.service';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { AuthService } from '../services/auth.service';

@Directive({
  selector: '[hasRole]'
})
export class HasRoleDirective implements OnInit, OnDestroy {
  private roles: string | string[] = '';
  private destroy$ = new Subject<void>();

  constructor(
    private templateRef: TemplateRef<any>,
    private viewContainer: ViewContainerRef,
    private permissionService: PermissionService,
    private authService: AuthService
  ) { }

  @Input() set hasRole(roles: string | string[]) {
    this.roles = roles;
    this.updateView();
  }

  ngOnInit() {
    // Re-check roles when user changes
    this.authService.currentUser
      .pipe(takeUntil(this.destroy$))
      .subscribe(() => {
        this.updateView();
      });
  }

  private updateView() {
    if (this.permissionService.hasRole(this.roles)) {
      this.viewContainer.createEmbeddedView(this.templateRef);
    } else {
      this.viewContainer.clear();
    }
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
