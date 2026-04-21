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
  error: string = '';
  
  documentTypes = ['Photo', 'Plan', 'Report', 'Invoice', 'Other'];

  constructor(private workOrderService: WorkOrderService) {}

  onFileSelected(event: any): void {
    const file = event.target.files[0];
    if (file) {
      // Validate file size (max 10MB)
      if (file.size > 10 * 1024 * 1024) {
        this.error = 'File size must be less than 10MB';
        this.selectedFile = null;
        return;
      }
      this.selectedFile = file;
      this.error = '';
    }
  }

  uploadDocument(): void {
    if (!this.selectedFile) {
      this.error = 'Please select a file';
      return;
    }

    this.uploading = true;
    this.error = '';

    const formData = new FormData();
    formData.append('file', this.selectedFile);
    formData.append('documentType', this.documentType);
    formData.append('description', this.description);

    this.workOrderService.uploadDocument(this.workOrderId, formData)
      .subscribe({
        next: () => {
          alert('Document uploaded successfully');
          this.resetForm();
          this.uploading = false;
        },
        error: (err) => {
          this.error = 'Failed to upload document';
          this.uploading = false;
          console.error(err);
        }
      });
  }

  resetForm(): void {
    this.selectedFile = null;
    this.documentType = 'Photo';
    this.description = '';
    const fileInput = document.querySelector('input[type="file"]') as HTMLInputElement;
    if (fileInput) {
      fileInput.value = '';
    }
  }
}
