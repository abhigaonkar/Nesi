import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterOutlet } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [CommonModule, RouterOutlet],
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent {
  authService = inject(AuthService);
  router = inject(Router);

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  navigateToTimesheets(): void {
    this.router.navigate(['/timesheets']);
  }

  navigateToTimesheetReview(): void {
    this.router.navigate(['/timesheet-review']);
  }

  navigateToCustomers(): void {
    this.router.navigate(['/customers']);
  }

  navigateToQuotes(): void {
    this.router.navigate(['/quotes']);
  }

  navigateToWorkOrders(): void {
    this.router.navigate(['/work-orders']);
  }

  navigateToDashboard(): void {
    this.router.navigate(['/dashboard']);
  }

  isActive(route: string): boolean {
    return this.router.url.startsWith(route);
  }
}
