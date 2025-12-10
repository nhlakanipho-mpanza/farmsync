import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { DocumentService, DocumentDto, DocumentUploadResponse } from '../../core/services/document.service';
import { ReferenceDataService } from '../../core/services/reference-data.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Observable, of } from 'rxjs';
import { switchMap, catchError } from 'rxjs/operators';

@Component({
  selector: 'app-document-upload',
  templateUrl: './document-upload.component.html',
  styleUrls: ['./document-upload.component.css']
})
export class DocumentUploadComponent implements OnInit {
  @Input() entityType: string = '';
  @Input() entityId: string = '';
  @Input() allowedTypes: string[] = []; // e.g., ['Driver\'s License', 'Quotation']
  @Output() uploaded = new EventEmitter<DocumentDto>();

  selectedFile: File | null = null;
  selectedDocumentTypeId: string = '';
  expiryDate: Date | null = null;
  notes: string = '';
  uploading: boolean = false;
  dragOver: boolean = false;

  documentTypes: any[] = [];
  filteredDocumentTypes: any[] = [];

  constructor(
    private documentService: DocumentService,
    private referenceDataService: ReferenceDataService,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit() {
    this.loadDocumentTypes();
  }

  loadDocumentTypes() {
    this.referenceDataService.getDocumentTypes().subscribe({
      next: (types) => {
        this.documentTypes = types;
        if (this.allowedTypes && this.allowedTypes.length > 0) {
          this.filteredDocumentTypes = types.filter(t => 
            this.allowedTypes.includes(t.name)
          );
        } else {
          this.filteredDocumentTypes = types;
        }
      },
      error: (err) => {
        console.error('Error loading document types:', err);
        this.snackBar.open('Failed to load document types', 'Close', { duration: 3000 });
      }
    });
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.selectedFile = input.files[0];
    }
  }

  onDragOver(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.dragOver = true;
  }

  onDragLeave(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.dragOver = false;
  }

  onDrop(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.dragOver = false;

    if (event.dataTransfer && event.dataTransfer.files.length > 0) {
      this.selectedFile = event.dataTransfer.files[0];
    }
  }

  uploadDocument(): Observable<any> {
    if (!this.selectedFile || !this.selectedDocumentTypeId) {
      this.snackBar.open('Please select a file and document type', 'Close', { duration: 3000 });
      return of(null);
    }

    if (!this.entityId) {
      this.snackBar.open('Entity ID is required for upload', 'Close', { duration: 3000 });
      return of(null);
    }

    this.uploading = true;

    return this.documentService.uploadDocument(
      this.selectedFile,
      this.entityType,
      this.entityId,
      this.selectedDocumentTypeId,
      this.expiryDate || undefined,
      this.notes || undefined
    ).pipe(
      switchMap((response: DocumentUploadResponse) => {
        this.snackBar.open('Document uploaded successfully', 'Close', { duration: 3000 });
        this.resetForm();
        this.uploaded.emit();
        this.uploading = false;
        return of(response);
      }),
      catchError((error) => {
        console.error('Upload error:', error);
        this.snackBar.open('Failed to upload document', 'Close', { duration: 3000 });
        this.uploading = false;
        return of(null);
      })
    );
  }

  resetForm(): void {
    this.selectedFile = null;
    this.selectedDocumentTypeId = '';
    this.expiryDate = null;
    this.notes = '';
  }

  getFileSizeDisplay(): string {
    if (!this.selectedFile) return '';
    return this.documentService.formatFileSize(this.selectedFile.size);
  }
}
