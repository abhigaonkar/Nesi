import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { WorkOrderService } from '../../../services/work-order.service';

interface Milestone {
  name: string;
  complete: boolean;
  dueDate?: string;
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

  constructor(private workOrderService: WorkOrderService) {}

  ngOnInit(): void {
    this.loadProgress();
  }

  loadProgress(): void {
    // This would load from API in a real implementation
    // For now, initialize with sample milestones
    if (this.milestones.length === 0) {
      this.milestones = [
        { name: 'Site Survey', complete: false },
        { name: 'Material Procurement', complete: false },
        { name: 'Installation', complete: false },
        { name: 'Testing', complete: false },
        { name: 'Final Inspection', complete: false }
      ];
    }
    this.calculatePercentComplete();
  }

  addMilestone(): void {
    this.milestones.push({ name: '', complete: false });
  }

  removeMilestone(index: number): void {
    this.milestones.splice(index, 1);
    this.calculatePercentComplete();
  }

  toggleMilestone(index: number): void {
    this.milestones[index].complete = !this.milestones[index].complete;
    this.calculatePercentComplete();
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
