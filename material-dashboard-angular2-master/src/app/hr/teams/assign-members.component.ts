import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { TeamService } from '../../core/services/team.service';
import { EmployeeService } from '../../core/services/employee.service';
import { Team, Employee } from '../../core/models/hr.model';

@Component({
  selector: 'app-assign-members',
  templateUrl: './assign-members.component.html',
  styleUrls: ['./assign-members.component.css']
})
export class AssignMembersComponent implements OnInit {
  assignForm: FormGroup;
  teamId: string = '';
  team: Team | null = null;
  availableEmployees: Employee[] = [];
  currentMembers: any[] = [];
  loading = false;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private teamService: TeamService,
    private employeeService: EmployeeService,
    private snackBar: MatSnackBar
  ) {
    this.assignForm = this.fb.group({
      employeeId: ['', Validators.required],
      startDate: [new Date().toISOString().split('T')[0], Validators.required],
      isPermanent: [false],
      notes: ['']
    });
  }

  ngOnInit() {
    this.route.params.subscribe(params => {
      if (params['id']) {
        this.teamId = params['id'];
        this.loadTeam();
        this.loadAvailableEmployees();
        this.loadCurrentMembers();
      }
    });
  }

  loadTeam() {
    this.teamService.getById(this.teamId).subscribe({
      next: (team) => {
        this.team = team;
      },
      error: (error) => {
        console.error('Error loading team:', error);
        this.snackBar.open('Error loading team', 'Close', { duration: 3000 });
      }
    });
  }

  loadAvailableEmployees() {
    this.employeeService.getActive().subscribe({
      next: (employees) => {
        this.availableEmployees = employees;
      },
      error: (error) => {
        console.error('Error loading employees:', error);
      }
    });
  }

  loadCurrentMembers() {
    this.teamService.getTeamMembers(this.teamId).subscribe({
      next: (members) => {
        this.currentMembers = members;
        // Filter out employees who are already members
        const memberIds = members.map(m => m.employeeId);
        this.availableEmployees = this.availableEmployees.filter(e => !memberIds.includes(e.id));
      },
      error: (error) => {
        console.error('Error loading team members:', error);
      }
    });
  }

  onAssign() {
    if (this.assignForm.valid) {
      this.loading = true;
      const formValue = this.assignForm.value;

      const memberData = {
        employeeId: formValue.employeeId,
        startDate: formValue.startDate,
        isPermanent: formValue.isPermanent,
        notes: formValue.notes
      };

      this.teamService.addTeamMember(this.teamId, memberData).subscribe({
        next: () => {
          this.snackBar.open('Employee assigned successfully', 'Close', { duration: 3000 });
          this.assignForm.reset({ isPermanent: false });
          this.loadCurrentMembers();
          this.loadAvailableEmployees();
          this.loading = false;
        },
        error: (error) => {
          console.error('Error assigning employee:', error);
          this.snackBar.open('Error assigning employee', 'Close', { duration: 3000 });
          this.loading = false;
        }
      });
    }
  }

  onRemoveMember(memberId: string) {
    if (confirm('Are you sure you want to remove this member from the team?')) {
      this.teamService.removeTeamMember(this.teamId, memberId).subscribe({
        next: () => {
          this.snackBar.open('Member removed successfully', 'Close', { duration: 3000 });
          this.loadCurrentMembers();
          this.loadAvailableEmployees();
        },
        error: (error) => {
          console.error('Error removing member:', error);
          this.snackBar.open('Error removing member', 'Close', { duration: 3000 });
        }
      });
    }
  }

  onBack() {
    this.router.navigate(['/hr/teams']);
  }
}
