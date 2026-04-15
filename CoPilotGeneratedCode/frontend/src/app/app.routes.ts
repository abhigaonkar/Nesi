import { Routes } from '@angular/router';
import { authGuard } from './guards/auth.guard';
import { LoginComponent } from './components/login/login.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { TimesheetComponent } from './components/timesheet/timesheet.component';
import { TimesheetReviewComponent } from './components/timesheet-review/timesheet-review.component';

export const routes: Routes = [
  { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'dashboard', component: DashboardComponent, canActivate: [authGuard] },
  { path: 'timesheets', component: TimesheetComponent, canActivate: [authGuard] },
  { path: 'timesheet-review', component: TimesheetReviewComponent, canActivate: [authGuard] },
  { path: '**', redirectTo: '/dashboard' }
];
