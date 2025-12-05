import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { IssuingService } from '../../../core/services/issuing.service';
import { EmployeeService } from '../../../core/services/employee.service';
import { TeamService } from '../../../core/services/team.service';

@Component({
  selector: 'app-equipment-issue-form',
  templateUrl: './equipment-issue-form.component.html',
  styleUrls: ['./equipment-issue-form.component.css']
})
export class EquipmentIssueFormComponent implements OnInit {
  issueForm: FormGroup;
  loading = false;
  employees: any[] = [];
  teams: any[] = [];
  equipment: any[] = [];
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
      equipmentId: ['', Validators.required],
      assignedToEmployeeId: [null],
      assignedToTeamId: [null],
      purpose: ['', Validators.required],
      expectedReturnDate: ['', Validators.required],
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

    // TODO: Load available equipment from equipment service
    // For now, placeholder - you'll need to create/import EquipmentService
    this.equipment = [];
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

    this.issuingService.requestEquipmentIssue(issueData).subscribe({
      next: () => {
        this.loading = false;
        this.router.navigate(['/hr/issuing']);
      },
      error: (error) => {
        console.error('Error creating equipment issue:', error);
        this.loading = false;
      }
    });
  }

  onCancel() {
    this.router.navigate(['/hr/issuing']);
  }
}
