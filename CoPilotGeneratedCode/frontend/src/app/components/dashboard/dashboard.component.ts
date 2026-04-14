import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { TimesheetService } from '../../services/timesheet.service';
import { Timesheet, TimesheetStatus } from '../../models/timesheet.model';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  authService = inject(AuthService);
  private timesheetService = inject(TimesheetService);
  private router = inject(Router);

  recentTimesheets = signal<Timesheet[]>([]);
  stats = signal({
    draftCount: 0,
    submittedCount: 0,
    approvedCount: 0,
    totalHours: 0
  });
  isLoading = signal(true);

  TimesheetStatus = TimesheetStatus;

  ngOnInit(): void {
    this.loadDashboardData();
  }

  loadDashboardData(): void {
    const user = this.authService.currentUser();
    if (!user) return;

    this.isLoading.set(true);

    // Load recent timesheets for current user
    this.timesheetService.getTimesheets({
      userId: user.id,
      pageNumber: 1,
      pageSize: 5
    }).subscribe({
      next: (response) => {
        if (response.success && response.data) {
          this.recentTimesheets.set(response.data.items);
          this.calculateStats(response.data.items);
        }
        this.isLoading.set(false);
      },
      error: (error) => {
        console.error('Error loading dashboard data:', error);
        this.isLoading.set(false);
      }
    });
  }

  calculateStats(timesheets: Timesheet[]): void {
    const stats = {
      draftCount: 0,
      submittedCount: 0,
      approvedCount: 0,
      totalHours: 0
    };

    timesheets.forEach(ts => {
      stats.totalHours += ts.hours;
      switch (ts.status) {
        case TimesheetStatus.Draft:
          stats.draftCount++;
          break;
        case TimesheetStatus.Submitted:
          stats.submittedCount++;
          break;
        case TimesheetStatus.Approved:
          stats.approvedCount++;
          break;
      }
    });

    this.stats.set(stats);
  }

  logout(): void {
    this.authService.logout();
  }

  navigateToTimesheets(): void {
    this.router.navigate(['/timesheets']);
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
}
