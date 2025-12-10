import { Component, Input, OnInit } from '@angular/core';
import { DocumentService, DocumentDto } from '../../core/services/document.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-document-list',
  templateUrl: './document-list.component.html',
  styleUrls: ['./document-list.component.css']
})
export class DocumentListComponent implements OnInit {
  @Input() entityType: string = '';
  @Input() entityId: string = '';

  documents: DocumentDto[] = [];
  loading: boolean = false;

  constructor(
    public documentService: DocumentService,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    if (this.entityType && this.entityId) {
      this.loadDocuments();
    }
  }

  loadDocuments(): void {
    this.loading = true;
    this.documentService.getEntityDocuments(this.entityType, this.entityId).subscribe({
      next: (documents) => {
        this.documents = documents;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading documents:', error);
        this.snackBar.open('Failed to load documents', 'Close', { duration: 3000 });
        this.loading = false;
      }
    });
  }

  downloadDocument(document: DocumentDto): void {
    this.documentService.downloadFile(document);
  }

  deleteDocument(document: DocumentDto): void {
    if (confirm(`Are you sure you want to delete "${document.originalFileName}"?`)) {
      this.documentService.deleteDocument(document.id).subscribe({
        next: () => {
          this.snackBar.open('Document deleted successfully', 'Close', { duration: 3000 });
          this.loadDocuments();
        },
        error: (error) => {
          console.error('Error deleting document:', error);
          this.snackBar.open('Failed to delete document', 'Close', { duration: 3000 });
        }
      });
    }
  }

  isExpiringSoon(document: DocumentDto): boolean {
    if (!document.expiryDate) return false;
    const expiryDate = new Date(document.expiryDate);
    const daysUntilExpiry = Math.floor((expiryDate.getTime() - new Date().getTime()) / (1000 * 60 * 60 * 24));
    return daysUntilExpiry <= 30 && daysUntilExpiry > 0;
  }

  isExpired(document: DocumentDto): boolean {
    if (!document.expiryDate) return false;
    const expiryDate = new Date(document.expiryDate);
    return expiryDate < new Date();
  }

  getExpiryBadgeClass(document: DocumentDto): string {
    if (this.isExpired(document)) return 'badge-danger';
    if (this.isExpiringSoon(document)) return 'badge-warning';
    return 'badge-success';
  }

  getExpiryText(document: DocumentDto): string {
    if (!document.expiryDate) return '';
    const expiryDate = new Date(document.expiryDate);
    const daysUntilExpiry = Math.floor((expiryDate.getTime() - new Date().getTime()) / (1000 * 60 * 60 * 24));
    
    if (daysUntilExpiry < 0) return 'Expired';
    if (daysUntilExpiry === 0) return 'Expires today';
    if (daysUntilExpiry <= 7) return `Expires in ${daysUntilExpiry} days`;
    return expiryDate.toLocaleDateString();
  }

  formatDate(dateString: string): string {
    return new Date(dateString).toLocaleDateString();
  }
}
