import { Injectable } from '@angular/core';
import { HttpClient, HttpEvent, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

export interface DocumentDto {
  id: string;
  fileName: string;
  originalFileName: string;
  fileUrl: string;
  fileSize: number;
  mimeType: string;
  entityType: string;
  entityId: string;
  documentType: string;
  expiryDate?: string;
  uploadedBy: string;
  uploadedAt: string;
  notes?: string;
  isActive: boolean;
  createdAt: string;
}

export interface DocumentUploadResponse {
  documentId: string;
  fileUrl: string;
  message: string;
}

@Injectable({
  providedIn: 'root'
})
export class DocumentService {
  private apiUrl = `${environment.apiUrl}/documents`;

  constructor(private http: HttpClient) {}

  uploadDocument(
    file: File,
    entityType: string,
    entityId: string,
    documentTypeId: string,
    expiryDate?: Date,
    notes?: string
  ): Observable<DocumentUploadResponse> {
    const formData = new FormData();
    formData.append('file', file);
    formData.append('entityType', entityType);
    formData.append('entityId', entityId);
    formData.append('documentTypeId', documentTypeId);
    
    if (expiryDate) {
      formData.append('expiryDate', expiryDate.toISOString());
    }
    if (notes) {
      formData.append('notes', notes);
    }

    return this.http.post<DocumentUploadResponse>(`${this.apiUrl}/upload`, formData);
  }

  getEntityDocuments(entityType: string, entityId: string): Observable<DocumentDto[]> {
    return this.http.get<DocumentDto[]>(`${this.apiUrl}/${entityType}/${entityId}`);
  }

  getDocument(id: string): Observable<DocumentDto> {
    return this.http.get<DocumentDto>(`${this.apiUrl}/${id}`);
  }

  downloadDocument(id: string): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/${id}/download`, {
      responseType: 'blob'
    });
  }

  updateDocument(id: string, dto: { expiryDate?: Date; notes?: string; isActive: boolean }): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, dto);
  }

  deleteDocument(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  getExpiringDocuments(daysAhead: number = 30): Observable<DocumentDto[]> {
    return this.http.get<DocumentDto[]>(`${this.apiUrl}/expiring?daysAhead=${daysAhead}`);
  }

  getExpiredDocuments(): Observable<DocumentDto[]> {
    return this.http.get<DocumentDto[]>(`${this.apiUrl}/expired`);
  }

  downloadFile(doc: DocumentDto): void {
    this.downloadDocument(doc.id).subscribe(blob => {
      const url = window.URL.createObjectURL(blob);
      const link = window.document.createElement('a');
      link.href = url;
      link.download = doc.originalFileName;
      link.click();
      window.URL.revokeObjectURL(url);
    });
  }

  getFileIcon(mimeType: string): string {
    if (mimeType.includes('pdf')) return 'picture_as_pdf';
    if (mimeType.includes('image')) return 'image';
    if (mimeType.includes('word') || mimeType.includes('document')) return 'description';
    if (mimeType.includes('excel') || mimeType.includes('spreadsheet')) return 'table_chart';
    return 'insert_drive_file';
  }

  getFileIconColor(mimeType: string): string {
    if (mimeType.includes('pdf')) return '#f44336';
    if (mimeType.includes('image')) return '#4CAF50';
    if (mimeType.includes('word') || mimeType.includes('document')) return '#2196F3';
    if (mimeType.includes('excel') || mimeType.includes('spreadsheet')) return '#4CAF50';
    return '#9E9E9E';
  }

  formatFileSize(bytes: number): string {
    if (bytes === 0) return '0 Bytes';
    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB', 'GB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return Math.round(bytes / Math.pow(k, i) * 100) / 100 + ' ' + sizes[i];
  }
}
