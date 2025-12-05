import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { TeamService } from 'app/core/services/team.service';
import { EmployeeService } from 'app/core/services/employee.service';
import { ReferenceDataService } from 'app/core/services/reference-data.service';
import { Employee, TeamType } from 'app/core/models/hr.model';

@Component({
  selector: 'app-team-form',
  templateUrl: './team-form.component.html',
  styleUrls: ['./team-form.component.css']
})
export class TeamFormComponent implements OnInit {
  teamForm: FormGroup;
  isEditMode = false;
  teamId: string;
  loading = false;
  
  employees: Employee[] = [];
  teamTypes: TeamType[] = [];

  constructor(
    private fb: FormBuilder,
    private teamService: TeamService,
    private employeeService: EmployeeService,
    private referenceDataService: ReferenceDataService,
    private route: ActivatedRoute,
    private router: Router,
    private snackBar: MatSnackBar
  ) {
    this.teamForm = this.fb.group({
      name: ['', Validators.required],
      description: [''],
      teamTypeId: [''],
      teamLeaderId: [''],
      isActive: [true]
    });
  }

  ngOnInit() {
    this.loadReferenceData();
    
    this.route.params.subscribe(params => {
      if (params['id']) {
        this.isEditMode = true;
        this.teamId = params['id'];
        this.loadTeam();
      }
    });
  }

  loadReferenceData() {
    this.employeeService.getActive().subscribe(data => this.employees = data);
    this.referenceDataService.getTeamTypes().subscribe(data => this.teamTypes = data);
  }

  loadTeam() {
    this.loading = true;
    this.teamService.getById(this.teamId).subscribe({
      next: (team) => {
        this.teamForm.patchValue({
          name: team.name,
          description: team.description,
          teamTypeId: team.teamTypeId,
          teamLeaderId: team.teamLeaderId,
          isActive: team.isActive
        });
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading team:', error);
        this.snackBar.open('Error loading team', 'Close', { duration: 3000 });
        this.loading = false;
      }
    });
  }

  onSubmit() {
    if (this.teamForm.valid) {
      this.loading = true;
      const formValue = this.teamForm.value;

      if (this.isEditMode) {
        this.teamService.update(this.teamId, formValue).subscribe({
          next: () => {
            this.snackBar.open('Team updated successfully', 'Close', { duration: 3000 });
            this.router.navigate(['/hr/teams']);
          },
          error: (error) => {
            console.error('Error updating team:', error);
            this.snackBar.open('Error updating team', 'Close', { duration: 3000 });
            this.loading = false;
          }
        });
      } else {
        this.teamService.create(formValue).subscribe({
          next: () => {
            this.snackBar.open('Team created successfully', 'Close', { duration: 3000 });
            this.router.navigate(['/hr/teams']);
          },
          error: (error) => {
            console.error('Error creating team:', error);
            this.snackBar.open('Error creating team', 'Close', { duration: 3000 });
            this.loading = false;
          }
        });
      }
    }
  }

  onCancel() {
    this.router.navigate(['/hr/teams']);
  }
}
