import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { WorkOrderService } from '../../../services/work-order.service';

interface Milestone {
  name: string;
  complete: boolean;
  dueDate?: string;
  completedDate?: string;
  status?: 'not-started' | 'in-progress' | 'completed' | 'overdue';
}

@Component({
  selector: 'app-progress-tracking',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './progress-tracking.component.html',
  styleUrls: ['./progress-tracking.component.scss']
})
export class ProgressTrackingComponent implements OnInit {
  @Input() workOrderId!: number;
  
  milestones: Milestone[] = [];
  percentComplete: number = 0;
  loading: boolean = false;
  editing: boolean = false;
  error: string = '';
  viewMode: 'list' | 'timeline' = 'timeline';

  constructor(private workOrderService: WorkOrderService) {}

  ngOnInit(): void {
    this.loadProgress();
  }

  loadProgress(): void {
    // This would load from API in a real implementation
    // For now, initialize with sample milestones
    if (this.milestones.length === 0) {
      const today = new Date();
      this.milestones = [
        { 
          name: 'Site Survey', 
          complete: true, 
          status: 'completed',
          completedDate: new Date(today.getTime() - 7 * 24 * 60 * 60 * 1000).toISOString().split('T')[0]
        },
        { 
          name: 'Material Procurement', 
          complete: true,
          status: 'completed',
          completedDate: new Date(today.getTime() - 4 * 24 * 60 * 60 * 1000).toISOString().split('T')[0]
        },
        { 
          name: 'Installation', 
          complete: false,
          status: 'in-progress',
          dueDate: new Date(today.getTime() + 3 * 24 * 60 * 60 * 1000).toISOString().split('T')[0]
        },
        { 
          name: 'Testing', 
          complete: false,
          status: 'not-started',
          dueDate: new Date(today.getTime() + 7 * 24 * 60 * 60 * 1000).toISOString().split('T')[0]
        },
        { 
          name: 'Final Inspection', 
          complete: false,
          status: 'not-started',
          dueDate: new Date(today.getTime() + 10 * 24 * 60 * 60 * 1000).toISOString().split('T')[0]
        }
      ];
    }
    this.calculatePercentComplete();
  }

  addMilestone(): void {
    this.milestones.push({ 
      name: '', 
      complete: false,
      status: 'not-started'
    });
  }

  removeMilestone(index: number): void {
    this.milestones.splice(index, 1);
    this.calculatePercentComplete();
  }

  toggleMilestone(index: number): void {
    this.milestones[index].complete = !this.milestones[index].complete;
    if (this.milestones[index].complete) {
      this.milestones[index].status = 'completed';
      this.milestones[index].completedDate = new Date().toISOString().split('T')[0];
    } else {
      this.milestones[index].status = 'not-started';
      delete this.milestones[index].completedDate;
    }
    this.calculatePercentComplete();
  }

  getStatusClass(status?: string): string {
    switch (status) {
      case 'completed':
        return 'text-success';
      case 'in-progress':
        return 'text-primary';
      case 'overdue':
        return 'text-danger';
      default:
        return 'text-secondary';
    }
  }

  getStatusIcon(status?: string): string {
    switch (status) {
      case 'completed':
        return 'bi-check-circle-fill';
      case 'in-progress':
        return 'bi-arrow-clockwise';
      case 'overdue':
        return 'bi-exclamation-triangle-fill';
      default:
        return 'bi-circle';
    }
  }

  toggleViewMode(): void {
    this.viewMode = this.viewMode === 'list' ? 'timeline' : 'list';
  }

  calculatePercentComplete(): void {
    if (this.milestones.length === 0) {
      this.percentComplete = 0;
      return;
    }
    const completedCount = this.milestones.filter(m => m.complete).length;
    this.percentComplete = Math.round((completedCount / this.milestones.length) * 100);
  }

  saveProgress(): void {
    this.loading = true;
    this.error = '';

    const milestonesJson = JSON.stringify(this.milestones);
    
    this.workOrderService.updateProgress(this.workOrderId, milestonesJson, this.percentComplete)
      .subscribe({
        next: () => {
          alert('Progress updated successfully');
          this.editing = false;
          this.loading = false;
        },
        error: (err) => {
          this.error = 'Failed to update progress';
          this.loading = false;
          console.error(err);
        }
      });
  }

  toggleEditing(): void {
    this.editing = !this.editing;
    if (!this.editing) {
      this.loadProgress(); // Reload to discard changes
    }
  }
}
