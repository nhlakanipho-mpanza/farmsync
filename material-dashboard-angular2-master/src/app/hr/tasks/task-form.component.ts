import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { WorkTaskService } from 'app/core/services/work-task.service';
import { TeamService } from 'app/core/services/team.service';
import { EmployeeService } from 'app/core/services/employee.service';
import { ReferenceDataService } from 'app/core/services/reference-data.service';
import { TaskTemplateService } from 'app/core/services/task-template.service';
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
  
  templates: any[] = [];
  useTemplate = false;
  selectedTemplate: any = null;
  templateChecklistItems: any[] = [];
  templateInventoryItems: any[] = [];
  inventoryQuantities: { [key: string]: { quantity: number, teamMemberCount?: number } } = {};

  constructor(
    private fb: FormBuilder,
    private workTaskService: WorkTaskService,
    private teamService: TeamService,
    private employeeService: EmployeeService,
    private referenceDataService: ReferenceDataService,
    private templateService: TaskTemplateService,
    private route: ActivatedRoute,
    private router: Router,
    private snackBar: MatSnackBar
  ) {
    this.taskForm = this.fb.group({
      taskTemplateId: [''],
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
    this.templateService.getActive().subscribe(data => this.templates = data);
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

  onUseTemplateChange() {
    if (!this.useTemplate) {
      this.selectedTemplate = null;
      this.templateChecklistItems = [];
      this.taskForm.patchValue({
        taskTemplateId: null,
        taskName: '',
        description: '',
        estimatedHours: ''
      });
    }
  }

  onTemplateChange(templateId: string) {
    if (!templateId) {
      this.selectedTemplate = null;
      this.templateChecklistItems = [];
      return;
    }

    this.templateService.getById(templateId).subscribe({
      next: (template) => {
        this.selectedTemplate = template;
        this.templateChecklistItems = template.checklistItems || [];
        this.templateInventoryItems = template.inventoryItems || [];
        
        // Initialize inventory quantities with template defaults
        this.inventoryQuantities = {};
        this.templateInventoryItems.forEach(item => {
          this.inventoryQuantities[item.id] = {
            quantity: item.quantityPerUnit,
            teamMemberCount: item.allocationMethod === 'PerTeamMember' ? 1 : undefined
          };
        });
        
        this.taskForm.patchValue({
          taskTemplateId: template.id,
          taskName: template.name,
          description: template.instructions || '',
          estimatedHours: template.estimatedHours || ''
        });
      },
      error: (error) => {
        console.error('Error loading template:', error);
        this.snackBar.open('Error loading template', 'Close', { duration: 3000 });
      }
    });
  }

  getCalculatedQuantity(item: any): number {
    const config = this.inventoryQuantities[item.id];
    if (!config) return item.quantityPerUnit;

    if (item.allocationMethod === 'PerTeamMember' && config.teamMemberCount) {
      return config.quantity * config.teamMemberCount;
    }
    return config.quantity;
  }

  onTeamMemberCountChange(itemId: string, count: number) {
    if (this.inventoryQuantities[itemId]) {
      this.inventoryQuantities[itemId].teamMemberCount = count;
    }
  }

  onInventoryQuantityChange(itemId: string, quantity: number) {
    if (this.inventoryQuantities[itemId]) {
      this.inventoryQuantities[itemId].quantity = quantity;
    }
  }

  buildInventoryAllocations(): any[] {
    return this.templateInventoryItems.map(item => {
      const config = this.inventoryQuantities[item.id];
      return {
        taskTemplateInventoryItemId: item.id,
        inventoryItemId: item.inventoryItemId,
        plannedQuantity: this.getCalculatedQuantity(item),
        allocationMethod: item.allocationMethod,
        teamMemberCount: config?.teamMemberCount,
        notes: item.notes
      };
    });
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
        // If using template, call create-from-template endpoint
        if (this.useTemplate && formValue.taskTemplateId) {
          const templateDto = {
            taskTemplateId: formValue.taskTemplateId,
            workAreaId: formValue.workAreaId,
            teamId: formValue.teamId,
            employeeId: formValue.employeeId,
            taskStatusId: formValue.taskStatusId,
            scheduledDate: formValue.scheduledDate,
            additionalNotes: formValue.description,
            inventoryAllocations: this.buildInventoryAllocations()
          };
          
          this.workTaskService.createFromTemplate(templateDto).subscribe({
            next: () => {
              this.snackBar.open('Task created from template successfully', 'Close', { duration: 3000 });
              this.router.navigate(['/hr/tasks']);
            },
            error: (error) => {
              console.error('Error creating task from template:', error);
              this.snackBar.open('Error creating task from template', 'Close', { duration: 3000 });
              this.loading = false;
            }
          });
        } else {
          // Regular task creation
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
  }

  onCancel() {
    this.router.navigate(['/hr/tasks']);
  }
}
