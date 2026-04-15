import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { TimesheetService } from '../../services/timesheet.service';
import { WorkOrderService } from '../../services/work-order.service';
import { Timesheet, TimesheetStatus, CreateTimesheetRequest } from '../../models/timesheet.model';
import { WorkOrder } from '../../models/work-order.model';

@Component({
  selector: 'app-timesheet',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './timesheet.component.html',
  styleUrls: ['./timesheet.component.scss']
})
export class TimesheetComponent implements OnInit {
  authService = inject(AuthService);
  private timesheetService = inject(TimesheetService);
  private workOrderService = inject(WorkOrderService);
  private router = inject(Router);
  private fb = inject(FormBuilder);

  timesheets = signal<Timesheet[]>([]);
  workOrders = signal<WorkOrder[]>([]);
  isLoading = signal(true);
  showForm = signal(false);
  editingTimesheet = signal<Timesheet | null>(null);
  errorMessage = signal<string | null>(null);
  successMessage = signal<string | null>(null);

  timesheetForm: FormGroup;
  TimesheetStatus = TimesheetStatus;

  currentPage = signal(1);
  pageSize = 10;
  totalPages = signal(1);
  totalCount = signal(0);

  constructor() {
    this.timesheetForm = this.fb.group({
      date: [''],
      hours: [0],
      payTypeId: [1],
      workOrderId: [null],
      jobTypeId: [null],
      notes: ['']
    });
  }

  ngOnInit(): void {
    this.loadTimesheets();
    this.loadWorkOrders();
  }

  loadTimesheets(): void {
    const user = this.authService.currentUser();
    if (!user) return;

    this.isLoading.set(true);
    this.timesheetService.getTimesheets({
      userId: user.id,
      pageNumber: this.currentPage(),
      pageSize: this.pageSize
    }).subscribe({
      next: (response) => {
        if (response.success && response.data) {
          this.timesheets.set(response.data.items);
          this.totalPages.set(response.data.totalPages);
          this.totalCount.set(response.data.totalCount);
        }
        this.isLoading.set(false);
      },
      error: (error) => {
        console.error('Error loading timesheets:', error);
        this.errorMessage.set('Failed to load timesheets');
        this.isLoading.set(false);
      }
    });
  }

  loadWorkOrders(): void {
    this.workOrderService.getWorkOrders().subscribe({
      next: (workOrders) => {
        this.workOrders.set(workOrders);
      },
      error: (error) => {
        console.error('Error loading work orders:', error);
      }
    });
  }

  openCreateForm(): void {
    this.editingTimesheet.set(null);
    this.timesheetForm.reset({
      date: new Date().toISOString().split('T')[0],
      hours: 8,
      payTypeId: 1,
      workOrderId: null,
      jobTypeId: null,
      notes: ''
    });
    this.showForm.set(true);
    this.clearMessages();
  }

  openEditForm(timesheet: Timesheet): void {
    if (timesheet.status !== TimesheetStatus.Draft) {
      this.errorMessage.set('Only draft timesheets can be edited');
      return;
    }

    this.editingTimesheet.set(timesheet);
    this.timesheetForm.patchValue({
      date: new Date(timesheet.date).toISOString().split('T')[0],
      hours: timesheet.hours,
      payTypeId: timesheet.payTypeId,
      workOrderId: timesheet.workOrderId,
      jobTypeId: timesheet.jobTypeId,
      notes: timesheet.notes
    });
    this.showForm.set(true);
    this.clearMessages();
  }

  closeForm(): void {
    this.showForm.set(false);
    this.editingTimesheet.set(null);
    this.timesheetForm.reset();
  }

  saveTimesheet(): void {
    if (this.timesheetForm.valid) {
      const formValue = this.timesheetForm.value;
      const editing = this.editingTimesheet();

      if (editing) {
        const request = {
          id: editing.id,
          date: new Date(formValue.date),
          hours: formValue.hours,
          payTypeId: formValue.payTypeId,
          workOrderId: formValue.workOrderId || undefined,
          jobTypeId: formValue.jobTypeId || undefined,
          notes: formValue.notes
        };

        this.timesheetService.updateTimesheet(request).subscribe({
          next: (response) => {
            if (response.success) {
              this.successMessage.set('Timesheet updated successfully');
              this.closeForm();
              this.loadTimesheets();
            } else {
              this.errorMessage.set(response.message || 'Failed to update timesheet');
            }
          },
          error: (error) => {
            console.error('Error updating timesheet:', error);
            this.errorMessage.set('An error occurred while updating timesheet');
          }
        });
      } else {
        const request: CreateTimesheetRequest = {
          date: new Date(formValue.date),
          hours: formValue.hours,
          payTypeId: formValue.payTypeId,
          workOrderId: formValue.workOrderId || undefined,
          jobTypeId: formValue.jobTypeId || undefined,
          notes: formValue.notes
        };

        this.timesheetService.createTimesheet(request).subscribe({
          next: (response) => {
            if (response.success) {
              this.successMessage.set('Timesheet created successfully');
              this.closeForm();
              this.loadTimesheets();
            } else {
              this.errorMessage.set(response.message || 'Failed to create timesheet');
            }
          },
          error: (error) => {
            console.error('Error creating timesheet:', error);
            this.errorMessage.set('An error occurred while creating timesheet');
          }
        });
      }
    }
  }

  submitTimesheet(timesheet: Timesheet): void {
    if (confirm('Are you sure you want to submit this timesheet?')) {
      this.timesheetService.submitTimesheet(timesheet.id).subscribe({
        next: (response) => {
          if (response.success) {
            this.successMessage.set('Timesheet submitted successfully');
            this.loadTimesheets();
          } else {
            this.errorMessage.set(response.message || 'Failed to submit timesheet');
          }
        },
        error: (error) => {
          console.error('Error submitting timesheet:', error);
          this.errorMessage.set('An error occurred while submitting timesheet');
        }
      });
    }
  }

  approveTimesheet(timesheet: Timesheet): void {
    if (confirm('Are you sure you want to approve this timesheet?')) {
      this.timesheetService.approveTimesheet(timesheet.id).subscribe({
        next: (response) => {
          if (response.success) {
            this.successMessage.set('Timesheet approved successfully');
            this.loadTimesheets();
          } else {
            this.errorMessage.set(response.message || 'Failed to approve timesheet');
          }
        },
        error: (error) => {
          console.error('Error approving timesheet:', error);
          this.errorMessage.set('An error occurred while approving timesheet');
        }
      });
    }
  }

  rejectTimesheet(timesheet: Timesheet): void {
    if (confirm('Are you sure you want to reject this timesheet?')) {
      this.timesheetService.rejectTimesheet(timesheet.id).subscribe({
        next: (response) => {
          if (response.success) {
            this.successMessage.set('Timesheet rejected successfully');
            this.loadTimesheets();
          } else {
            this.errorMessage.set(response.message || 'Failed to reject timesheet');
          }
        },
        error: (error) => {
          console.error('Error rejecting timesheet:', error);
          this.errorMessage.set('An error occurred while rejecting timesheet');
        }
      });
    }
  }

  nextPage(): void {
    if (this.currentPage() < this.totalPages()) {
      this.currentPage.update(p => p + 1);
      this.loadTimesheets();
    }
  }

  previousPage(): void {
    if (this.currentPage() > 1) {
      this.currentPage.update(p => p - 1);
      this.loadTimesheets();
    }
  }

  getStatusClass(status: TimesheetStatus): string {
    switch (status) {
      case TimesheetStatus.Draft:
        return 'status-draft';
      case TimesheetStatus.Submitted:
        return 'status-submitted';
      case TimesheetStatus.Approved:
        return 'status-approved';
      case TimesheetStatus.Rejected:
        return 'status-rejected';
      default:
        return '';
    }
  }

  getStatusText(status: TimesheetStatus): string {
    switch (status) {
      case TimesheetStatus.Draft:
        return 'Draft';
      case TimesheetStatus.Submitted:
        return 'Submitted';
      case TimesheetStatus.Approved:
        return 'Approved';
      case TimesheetStatus.Rejected:
        return 'Rejected';
      default:
        return 'Unknown';
    }
  }

  canEdit(timesheet: Timesheet): boolean {
    return timesheet.status === TimesheetStatus.Draft;
  }

  canSubmit(timesheet: Timesheet): boolean {
    return timesheet.status === TimesheetStatus.Draft;
  }

  canApprove(timesheet: Timesheet): boolean {
    return this.authService.canApproveTimesheets() && timesheet.status === TimesheetStatus.Submitted;
  }

  canReject(timesheet: Timesheet): boolean {
    return this.authService.canApproveTimesheets() && timesheet.status === TimesheetStatus.Submitted;
  }

  clearMessages(): void {
    this.errorMessage.set(null);
    this.successMessage.set(null);
  }
}
