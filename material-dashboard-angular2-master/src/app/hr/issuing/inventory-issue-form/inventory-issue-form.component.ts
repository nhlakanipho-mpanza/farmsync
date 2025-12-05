import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { IssuingService } from '../../../core/services/issuing.service';
import { EmployeeService } from '../../../core/services/employee.service';
import { TeamService } from '../../../core/services/team.service';

@Component({
  selector: 'app-inventory-issue-form',
  templateUrl: './inventory-issue-form.component.html',
  styleUrls: ['./inventory-issue-form.component.css']
})
export class InventoryIssueFormComponent implements OnInit {
  issueForm: FormGroup;
  loading = false;
  employees: any[] = [];
  teams: any[] = [];
  inventoryItems: any[] = [];
  assignmentType: 'employee' | 'team' = 'employee';

  constructor(
    private fb: FormBuilder,
    private issuingService: IssuingService,
    private employeeService: EmployeeService,
    private teamService: TeamService,
    private router: Router
  ) {}

  ngOnInit() {
    this.initForm();
    this.loadReferenceData();
  }

  initForm() {
    this.issueForm = this.fb.group({
      inventoryItemId: ['', Validators.required],
      quantity: ['', [Validators.required, Validators.min(1)]],
      assignedToEmployeeId: [null],
      assignedToTeamId: [null],
      purpose: ['', Validators.required],
      notes: ['']
    });
  }

  loadReferenceData() {
    // Load employees
    this.employeeService.getAll().subscribe(data => {
      this.employees = data;
    });

    // Load teams
    this.teamService.getAll().subscribe(data => {
      this.teams = data;
    });

    // TODO: Load inventory items from inventory service
    // For now, placeholder - you'll need to create/import InventoryService
    this.inventoryItems = [];
  }

  onAssignmentTypeChange(type: 'employee' | 'team') {
    this.assignmentType = type;
    if (type === 'employee') {
      this.issueForm.patchValue({ assignedToTeamId: null });
    } else {
      this.issueForm.patchValue({ assignedToEmployeeId: null });
    }
  }

  onSubmit() {
    if (!this.issueForm.valid) {
      return;
    }

    this.loading = true;
    const issueData = {
      ...this.issueForm.value,
      tenantId: 1 // TODO: Get from auth service
    };

    this.issuingService.requestInventoryIssue(issueData).subscribe({
      next: () => {
        this.loading = false;
        this.router.navigate(['/hr/issuing']);
      },
      error: (error) => {
        console.error('Error creating inventory issue:', error);
        this.loading = false;
      }
    });
  }

  onCancel() {
    this.router.navigate(['/hr/issuing']);
  }
}
