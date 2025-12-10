import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatDialog } from '@angular/material/dialog';
import { WorkTaskService } from 'app/core/services/work-task.service';
import { AuthService } from 'app/core/services/auth.service';

@Component({
  selector: 'app-task-detail',
  templateUrl: './task-detail.component.html',
  styleUrls: ['./task-detail.component.css']
})
export class TaskDetailComponent implements OnInit {
  task: any = null;
  checklistItems: any[] = [];
  loading = false;
  taskId: string;
  currentUserId: string;
  completionNotes = '';
  selectedItemForNotes: string | null = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private workTaskService: WorkTaskService,
    private authService: AuthService,
    private snackBar: MatSnackBar,
    private dialog: MatDialog
  ) {}

  ngOnInit() {
    const currentUser = this.authService.currentUserValue;
    this.currentUserId = currentUser?.id || '';
    
    this.route.params.subscribe(params => {
      this.taskId = params['id'];
      this.loadTask();
      this.loadChecklist();
    });
  }

  loadTask() {
    this.loading = true;
    this.workTaskService.getById(this.taskId).subscribe({
      next: (task) => {
        this.task = task;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading task:', error);
        this.snackBar.open('Error loading task', 'Close', { duration: 3000 });
        this.loading = false;
      }
    });
  }

  loadChecklist() {
    this.workTaskService.getTaskChecklist(this.taskId).subscribe({
      next: (items) => {
        this.checklistItems = items;
      },
      error: (error) => {
        console.error('Error loading checklist:', error);
      }
    });
  }

  get checklistProgress(): { completed: number; total: number; percentage: number } {
    const total = this.checklistItems.length;
    const completed = this.checklistItems.filter(item => item.isCompleted).length;
    const percentage = total > 0 ? Math.round((completed / total) * 100) : 0;
    return { completed, total, percentage };
  }

  toggleChecklistItem(item: any) {
    if (item.isCompleted) {
      // Mark as incomplete
      this.workTaskService.markChecklistItemIncomplete(this.taskId, item.id).subscribe({
        next: () => {
          this.snackBar.open('Item marked as incomplete', 'Close', { duration: 2000 });
          this.loadChecklist();
        },
        error: (error) => {
          console.error('Error updating checklist item:', error);
          this.snackBar.open('Error updating item', 'Close', { duration: 3000 });
        }
      });
    } else {
      // Show notes dialog for completion
      this.selectedItemForNotes = item.id;
      this.completionNotes = '';
    }
  }

  saveChecklistCompletion(itemId: string) {
    const dto = {
      completedBy: this.currentUserId,
      notes: this.completionNotes
    };

    this.workTaskService.markChecklistItemComplete(this.taskId, itemId, dto).subscribe({
      next: () => {
        this.snackBar.open('Item marked as complete', 'Close', { duration: 2000 });
        this.selectedItemForNotes = null;
        this.completionNotes = '';
        this.loadChecklist();
      },
      error: (error) => {
        console.error('Error completing checklist item:', error);
        this.snackBar.open('Error completing item', 'Close', { duration: 3000 });
      }
    });
  }

  cancelNotes() {
    this.selectedItemForNotes = null;
    this.completionNotes = '';
  }

  editTask() {
    this.router.navigate(['/hr/tasks/edit', this.taskId]);
  }

  goBack() {
    this.router.navigate(['/hr/tasks']);
  }

  formatDate(date: string): string {
    if (!date) return 'N/A';
    return new Date(date).toLocaleDateString();
  }

  formatDateTime(date: string): string {
    if (!date) return 'N/A';
    return new Date(date).toLocaleString();
  }
}
