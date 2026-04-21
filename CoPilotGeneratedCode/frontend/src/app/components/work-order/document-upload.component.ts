import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { WorkOrderService } from '../../../services/work-order.service';

@Component({
  selector: 'app-document-upload',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './document-upload.component.html',
  styleUrls: ['./document-upload.component.scss']
})
export class DocumentUploadComponent {
  @Input() workOrderId!: number;
  
  selectedFile: File | null = null;
  documentType: string = 'Photo';
  description: string = '';
  uploading: boolean = false;
  uploadProgress: number = 0;
  error: string = '';
  previewUrl: string | null = null;
  
  documentTypes = ['Photo', 'Plan', 'Report', 'Invoice', 'Other'];

  constructor(private workOrderService: WorkOrderService) {}

  onFileSelected(event: any): void {
    const file = event.target.files[0];
    if (file) {
      // Validate file size (max 10MB)
      if (file.size > 10 * 1024 * 1024) {
        this.error = 'File size must be less than 10MB';
        this.selectedFile = null;
        this.previewUrl = null;
        return;
      }
      
      this.selectedFile = file;
      this.error = '';
      
      // Generate preview for images
      if (file.type.startsWith('image/')) {
        const reader = new FileReader();
        reader.onload = (e: any) => {
          this.previewUrl = e.target.result;
        };
        reader.readAsDataURL(file);
      } else {
        this.previewUrl = null;
      }
    }
  }

  uploadDocument(): void {
    if (!this.selectedFile) {
      this.error = 'Please select a file';
      return;
    }

    this.uploading = true;
    this.uploadProgress = 0;
    this.error = '';

    const formData = new FormData();
    formData.append('file', this.selectedFile);
    formData.append('documentType', this.documentType);
    formData.append('description', this.description);

    // Simulate upload progress (in a real implementation, this would use HttpEvent)
    const progressInterval = setInterval(() => {
      if (this.uploadProgress < 90) {
        this.uploadProgress += 10;
      }
    }, 200);

    this.workOrderService.uploadDocument(this.workOrderId, formData)
      .subscribe({
        next: () => {
          clearInterval(progressInterval);
          this.uploadProgress = 100;
          setTimeout(() => {
            alert('Document uploaded successfully');
            this.resetForm();
            this.uploading = false;
            this.uploadProgress = 0;
          }, 500);
        },
        error: (err) => {
          clearInterval(progressInterval);
          this.error = 'Failed to upload document';
          this.uploading = false;
          this.uploadProgress = 0;
          console.error(err);
        }
      });
  }

  resetForm(): void {
    this.selectedFile = null;
    this.documentType = 'Photo';
    this.description = '';
    this.previewUrl = null;
    this.uploadProgress = 0;
    const fileInput = document.querySelector('input[type="file"]') as HTMLInputElement;
    if (fileInput) {
      fileInput.value = '';
    }
  }

  getFileIcon(file: File): string {
    const type = file.type;
    if (type.startsWith('image/')) return 'bi-file-image';
    if (type === 'application/pdf') return 'bi-file-pdf';
    if (type.includes('word')) return 'bi-file-word';
    if (type.includes('excel') || type.includes('spreadsheet')) return 'bi-file-excel';
    return 'bi-file-earmark';
  }

  formatFileSize(bytes: number): string {
    if (bytes === 0) return '0 Bytes';
    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB', 'GB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return Math.round(bytes / Math.pow(k, i) * 100) / 100 + ' ' + sizes[i];
  }
}
