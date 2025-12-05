import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { WorkTaskService } from 'app/core/services/work-task.service';
import { TeamService } from 'app/core/services/team.service';
import { EmployeeService } from 'app/core/services/employee.service';
import { ReferenceDataService } from 'app/core/services/reference-data.service';
import { Team, Employee, TaskStatus, WorkArea } from 'app/core/models/hr.model';

@Component({
  selector: 'app-task-form',
  templateUrl: './task-form.component.html',
  styleUrls: ['./task-form.component.css']
})
export class TaskFormComponent implements OnInit {
  taskForm: FormGroup;
  isEditMode = false;
  taskId: string;
  loading = false;
  
  teams: Team[] = [];
  employees: Employee[] = [];
  taskStatuses: TaskStatus[] = [];
  workAreas: WorkArea[] = [];
  assignmentType: 'team' | 'employee' | 'none' = 'none';

  constructor(
    private fb: FormBuilder,
    private workTaskService: WorkTaskService,
    private teamService: TeamService,
    private employeeService: EmployeeService,
    private referenceDataService: ReferenceDataService,
    private route: ActivatedRoute,
    private router: Router,
    private snackBar: MatSnackBar
  ) {
    this.taskForm = this.fb.group({
      taskName: ['', Validators.required],
      description: [''],
      workAreaId: [''],
      scheduledDate: ['', Validators.required],
      estimatedHours: [''],
      actualHours: [''],
      teamId: [''],
      employeeId: [''],
      taskStatusId: ['']
    });
  }

  ngOnInit() {
    this.loadReferenceData();
    
    this.route.params.subscribe(params => {
      if (params['id']) {
        this.isEditMode = true;
        this.taskId = params['id'];
        this.loadTask();
      }
    });
  }

  loadReferenceData() {
    this.teamService.getActive().subscribe(data => this.teams = data);
    this.employeeService.getActive().subscribe(data => this.employees = data);
    this.referenceDataService.getTaskStatuses().subscribe(data => this.taskStatuses = data);
    this.referenceDataService.getWorkAreas().subscribe(data => this.workAreas = data);
  }

  loadTask() {
    this.loading = true;
    this.workTaskService.getById(this.taskId).subscribe({
      next: (task) => {
        this.taskForm.patchValue({
          taskName: task.taskName,
          description: task.description,
          workAreaId: task.workAreaId,
          scheduledDate: task.scheduledDate,
          estimatedHours: task.estimatedHours,
          actualHours: task.actualHours,
          teamId: task.teamId,
          employeeId: task.employeeId,
          taskStatusId: task.taskStatusId
        });
        
        if (task.teamId) {
          this.assignmentType = 'team';
        } else if (task.employeeId) {
          this.assignmentType = 'employee';
        }
        
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading task:', error);
        this.snackBar.open('Error loading task', 'Close', { duration: 3000 });
        this.loading = false;
      }
    });
  }

  onAssignmentTypeChange() {
    if (this.assignmentType === 'team') {
      this.taskForm.patchValue({ employeeId: null });
    } else if (this.assignmentType === 'employee') {
      this.taskForm.patchValue({ teamId: null });
    } else {
      this.taskForm.patchValue({ teamId: null, employeeId: null });
    }
  }

  onSubmit() {
    if (this.taskForm.valid) {
      this.loading = true;
      const formValue = this.taskForm.value;

      if (this.isEditMode) {
        this.workTaskService.update(this.taskId, formValue).subscribe({
          next: () => {
            this.snackBar.open('Task updated successfully', 'Close', { duration: 3000 });
            this.router.navigate(['/hr/tasks']);
          },
          error: (error) => {
            console.error('Error updating task:', error);
            this.snackBar.open('Error updating task', 'Close', { duration: 3000 });
            this.loading = false;
          }
        });
      } else {
        this.workTaskService.create(formValue).subscribe({
          next: () => {
            this.snackBar.open('Task created successfully', 'Close', { duration: 3000 });
            this.router.navigate(['/hr/tasks']);
          },
          error: (error) => {
            console.error('Error creating task:', error);
            this.snackBar.open('Error creating task', 'Close', { duration: 3000 });
            this.loading = false;
          }
        });
      }
    }
  }

  onCancel() {
    this.router.navigate(['/hr/tasks']);
  }
}
