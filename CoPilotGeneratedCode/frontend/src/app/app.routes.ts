import { Routes } from '@angular/router';
import { authGuard } from './guards/auth.guard';
import { LoginComponent } from './components/login/login.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { TimesheetComponent } from './components/timesheet/timesheet.component';
import { TimesheetReviewComponent } from './components/timesheet-review/timesheet-review.component';
import { QuoteListComponent } from './components/quote/quote-list.component';
import { QuoteFormComponent } from './components/quote/quote-form.component';
import { QuoteDetailComponent } from './components/quote/quote-detail.component';
import { WorkOrderListComponent } from './components/work-order/work-order-list.component';
import { WorkOrderDetailComponent } from './components/work-order/work-order-detail.component';
import { CustomerListComponent } from './components/customer/customer-list.component';
import { CustomerFormComponent } from './components/customer/customer-form.component';
import { CustomerDetailComponent } from './components/customer/customer-detail.component';

export const routes: Routes = [
  { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'dashboard', component: DashboardComponent, canActivate: [authGuard] },
  { path: 'timesheets', component: TimesheetComponent, canActivate: [authGuard] },
  { path: 'timesheet-review', component: TimesheetReviewComponent, canActivate: [authGuard] },
  { path: 'customers', component: CustomerListComponent, canActivate: [authGuard] },
  { path: 'customers/create', component: CustomerFormComponent, canActivate: [authGuard] },
  { path: 'customers/edit/:id', component: CustomerFormComponent, canActivate: [authGuard] },
  { path: 'customers/:id', component: CustomerDetailComponent, canActivate: [authGuard] },
  { path: 'quotes', component: QuoteListComponent, canActivate: [authGuard] },
  { path: 'quotes/create', component: QuoteFormComponent, canActivate: [authGuard] },
  { path: 'quotes/edit/:id', component: QuoteFormComponent, canActivate: [authGuard] },
  { path: 'quotes/:id', component: QuoteDetailComponent, canActivate: [authGuard] },
  { path: 'work-orders', component: WorkOrderListComponent, canActivate: [authGuard] },
  { path: 'work-orders/:id', component: WorkOrderDetailComponent, canActivate: [authGuard] },
  { path: '**', redirectTo: '/dashboard' }
];
