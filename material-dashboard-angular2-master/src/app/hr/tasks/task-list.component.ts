import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { WorkTaskService } from 'app/core/services/work-task.service';
import { ReferenceDataService } from 'app/core/services/reference-data.service';
import { WorkTask, TaskStatus } from 'app/core/models/hr.model';

@Component({
  selector: 'app-task-list',
  templateUrl: './task-list.component.html',
  styleUrls: ['./task-list.component.css']
})
export class TaskListComponent implements OnInit {
  tasks: WorkTask[] = [];
  filteredTasks: WorkTask[] = [];
  loading = false;
  searchTerm = '';
  selectedStatus = '';
  selectedDate = '';
  taskStatuses: TaskStatus[] = [];

  constructor(
    private workTaskService: WorkTaskService,
    private referenceDataService: ReferenceDataService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit() {
    this.loadTaskStatuses();
    this.loadTasks();
  }

  loadTaskStatuses() {
    this.referenceDataService.getTaskStatuses().subscribe(
      data => this.taskStatuses = data
    );
  }

  loadTasks() {
    this.loading = true;
    this.workTaskService.getAll().subscribe({
      next: (tasks) => {
        this.tasks = tasks;
        this.filteredTasks = tasks;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading tasks:', error);
        this.snackBar.open('Error loading tasks', 'Close', { duration: 3000 });
        this.loading = false;
      }
    });
  }

  filterTasks() {
    this.filteredTasks = this.tasks.filter(task => {
      const matchesSearch = !this.searchTerm || 
        task.taskName.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        task.description?.toLowerCase().includes(this.searchTerm.toLowerCase());
      
      const matchesStatus = !this.selectedStatus || task.taskStatusId === this.selectedStatus;
      const matchesDate = !this.selectedDate || task.scheduledDate === this.selectedDate;

      return matchesSearch && matchesStatus && matchesDate;
    });
  }

  onSearch() {
    this.filterTasks();
  }

  onFilterChange() {
    this.filterTasks();
  }

  addTask() {
    this.router.navigate(['/hr/tasks/create']);
  }

  editTask(id: string) {
    this.router.navigate(['/hr/tasks/edit', id]);
  }

  deleteTask(id: string, name: string) {
    if (confirm(`Are you sure you want to delete task "${name}"?`)) {
      this.workTaskService.delete(id).subscribe({
        next: () => {
          this.snackBar.open('Task deleted successfully', 'Close', { duration: 3000 });
          this.loadTasks();
        },
        error: (error) => {
          console.error('Error deleting task:', error);
          this.snackBar.open('Error deleting task', 'Close', { duration: 3000 });
        }
      });
    }
  }

  getStatusClass(statusName: string): string {
    switch(statusName?.toLowerCase()) {
      case 'pending': return 'badge-warning';
      case 'inprogress': return 'badge-info';
      case 'completed': return 'badge-success';
      case 'cancelled': return 'badge-secondary';
      default: return 'badge-secondary';
    }
  }
}
