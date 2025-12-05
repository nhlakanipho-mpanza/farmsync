import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AttendanceService } from 'app/core/services/attendance.service';
import { EmployeeService } from 'app/core/services/employee.service';
import { TeamService } from 'app/core/services/team.service';
import { Employee, Team, AttendanceSummary } from 'app/core/models/hr.model';

@Component({
  selector: 'app-attendance-dashboard',
  templateUrl: './attendance-dashboard.component.html',
  styleUrls: ['./attendance-dashboard.component.css']
})
export class AttendanceDashboardComponent implements OnInit {
  clockForm: FormGroup;
  summaryForm: FormGroup;
  loading = false;
  
  employees: Employee[] = [];
  teams: Team[] = [];
  dailySummary: AttendanceSummary | null = null;
  teamSummary: AttendanceSummary[] = [];
  
  viewMode: 'clock' | 'employee' | 'team' = 'clock';

  constructor(
    private fb: FormBuilder,
    private attendanceService: AttendanceService,
    private employeeService: EmployeeService,
    private teamService: TeamService,
    private snackBar: MatSnackBar
  ) {
    this.clockForm = this.fb.group({
      employeeId: ['', Validators.required],
      eventType: ['ClockIn', Validators.required],
      biometricId: [''],
      teamId: [''],
      notes: ['']
    });

    this.summaryForm = this.fb.group({
      employeeId: [''],
      teamId: [''],
      date: [new Date().toISOString().split('T')[0], Validators.required]
    });
  }

  ngOnInit() {
    this.loadEmployees();
    this.loadTeams();
  }

  loadEmployees() {
    this.employeeService.getActive().subscribe(data => this.employees = data);
  }

  loadTeams() {
    this.teamService.getActive().subscribe(data => this.teams = data);
  }

  onClockEvent() {
    if (this.clockForm.valid) {
      this.loading = true;
      const formValue = {
        ...this.clockForm.value,
        eventTime: new Date().toISOString()
      };

      this.attendanceService.clockEvent(formValue).subscribe({
        next: (event) => {
          const eventType = formValue.eventType === 'ClockIn' ? 'clocked in' : 'clocked out';
          this.snackBar.open(`Successfully ${eventType}`, 'Close', { duration: 3000 });
          this.clockForm.patchValue({
            eventType: formValue.eventType === 'ClockIn' ? 'ClockOut' : 'ClockIn',
            biometricId: '',
            notes: ''
          });
          this.loading = false;
        },
        error: (error) => {
          console.error('Error recording clock event:', error);
          this.snackBar.open('Error recording clock event', 'Close', { duration: 3000 });
          this.loading = false;
        }
      });
    }
  }

  onLoadEmployeeSummary() {
    const employeeId = this.summaryForm.get('employeeId').value;
    const date = this.summaryForm.get('date').value;
    
    if (employeeId && date) {
      this.loading = true;
      this.attendanceService.getDailySummary(employeeId, date).subscribe({
        next: (summary) => {
          this.dailySummary = summary;
          this.loading = false;
        },
        error: (error) => {
          console.error('Error loading summary:', error);
          this.snackBar.open('Error loading attendance summary', 'Close', { duration: 3000 });
          this.loading = false;
        }
      });
    }
  }

  onLoadTeamSummary() {
    const teamId = this.summaryForm.get('teamId').value;
    const date = this.summaryForm.get('date').value;
    
    if (teamId && date) {
      this.loading = true;
      this.attendanceService.getTeamDailySummary(teamId, date).subscribe({
        next: (summary) => {
          this.teamSummary = summary;
          this.loading = false;
        },
        error: (error) => {
          console.error('Error loading team summary:', error);
          this.snackBar.open('Error loading team attendance summary', 'Close', { duration: 3000 });
          this.loading = false;
        }
      });
    }
  }
}
