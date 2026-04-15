import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { TimesheetService } from '../../services/timesheet.service';
import { Timesheet, TimesheetStatus } from '../../models/timesheet.model';

@Component({
  selector: 'app-timesheet-review',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './timesheet-review.component.html',
  styleUrls: ['./timesheet-review.component.scss']
})
export class TimesheetReviewComponent implements OnInit {
  authService = inject(AuthService);
  private timesheetService = inject(TimesheetService);
  private router = inject(Router);

  timesheets = signal<Timesheet[]>([]);
  isLoading = signal(true);
  errorMessage = signal<string | null>(null);
  successMessage = signal<string | null>(null);

  TimesheetStatus = TimesheetStatus;

  currentPage = signal(1);
  pageSize = 10;
  totalPages = signal(1);
  totalCount = signal(0);

  ngOnInit(): void {
    // Check if user has permission to approve timesheets
    if (!this.authService.canApproveTimesheets()) {
      this.errorMessage.set('You do not have permission to access this page');
      setTimeout(() => this.router.navigate(['/dashboard']), 2000);
      return;
    }

    this.loadSubmittedTimesheets();
  }

  loadSubmittedTimesheets(): void {
    this.isLoading.set(true);
    // Load only submitted timesheets for manager review
    this.timesheetService.getTimesheets({
      status: 'Submitted',
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
        this.errorMessage.set('Failed to load timesheets for review');
        this.isLoading.set(false);
      }
    });
  }

  approveTimesheet(timesheet: Timesheet): void {
    if (confirm(`Are you sure you want to approve the timesheet for ${timesheet.userName} on ${new Date(timesheet.date).toLocaleDateString()}?`)) {
      this.timesheetService.approveTimesheet(timesheet.id).subscribe({
        next: (response) => {
          if (response.success) {
            this.successMessage.set('Timesheet approved successfully');
            this.loadSubmittedTimesheets();
            setTimeout(() => this.clearMessages(), 3000);
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
    const reason = prompt(`Please provide a reason for rejecting ${timesheet.userName}'s timesheet:`);
    if (reason) {
      this.timesheetService.rejectTimesheet(timesheet.id).subscribe({
        next: (response) => {
          if (response.success) {
            this.successMessage.set('Timesheet rejected successfully');
            this.loadSubmittedTimesheets();
            setTimeout(() => this.clearMessages(), 3000);
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

  goToDashboard(): void {
    this.router.navigate(['/dashboard']);
  }

  logout(): void {
    this.authService.logout();
  }

  nextPage(): void {
    if (this.currentPage() < this.totalPages()) {
      this.currentPage.update(p => p + 1);
      this.loadSubmittedTimesheets();
    }
  }

  previousPage(): void {
    if (this.currentPage() > 1) {
      this.currentPage.update(p => p - 1);
      this.loadSubmittedTimesheets();
    }
  }

  clearMessages(): void {
    this.errorMessage.set(null);
    this.successMessage.set(null);
  }
}
