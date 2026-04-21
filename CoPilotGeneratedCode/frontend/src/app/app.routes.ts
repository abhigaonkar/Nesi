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
import { PurchaseOrderListComponent } from './components/purchase-order/purchase-order-list.component';
import { PurchaseOrderFormComponent } from './components/purchase-order/purchase-order-form.component';
import { PurchaseOrderDetailComponent } from './components/purchase-order/purchase-order-detail.component';
import { ReceiptCreateComponent } from './components/purchase-order/receipt-create.component';
import { ReceiptListComponent } from './components/purchase-order/receipt-list.component';
import { VendorListComponent } from './components/vendor/vendor-list.component';
import { VendorFormComponent } from './components/vendor/vendor-form.component';
import { VendorDetailComponent } from './components/vendor/vendor-detail.component';
import { ReportsDashboardComponent } from './components/reports/reports-dashboard.component';
import { JobCostReportComponent } from './components/reports/job-cost-report.component';
import { IncomeStatementComponent } from './components/reports/income-statement.component';
import { ArAgingReportComponent } from './components/reports/ar-aging-report.component';
import { CustomerAnalysisComponent } from './components/reports/customer-analysis.component';
import { InventoryUsageComponent } from './components/reports/inventory-usage.component';

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
  
  // Purchase Orders
  { path: 'purchase-orders', component: PurchaseOrderListComponent, canActivate: [authGuard] },
  { path: 'purchase-orders/new', component: PurchaseOrderFormComponent, canActivate: [authGuard] },
  { path: 'purchase-orders/edit/:id', component: PurchaseOrderFormComponent, canActivate: [authGuard] },
  { path: 'purchase-orders/:id', component: PurchaseOrderDetailComponent, canActivate: [authGuard] },
  { path: 'purchase-orders/:id/receipts/new', component: ReceiptCreateComponent, canActivate: [authGuard] },
  { path: 'purchase-orders/:id/receipts', component: ReceiptListComponent, canActivate: [authGuard] },
  
  // Vendors
  { path: 'vendors', component: VendorListComponent, canActivate: [authGuard] },
  { path: 'vendors/new', component: VendorFormComponent, canActivate: [authGuard] },
  { path: 'vendors/edit/:id', component: VendorFormComponent, canActivate: [authGuard] },
  { path: 'vendors/:id', component: VendorDetailComponent, canActivate: [authGuard] },
  
  // Reports
  { path: 'reports', component: ReportsDashboardComponent, canActivate: [authGuard] },
  { path: 'reports/job-cost', component: JobCostReportComponent, canActivate: [authGuard] },
  { path: 'reports/income-statement', component: IncomeStatementComponent, canActivate: [authGuard] },
  { path: 'reports/ar-aging', component: ArAgingReportComponent, canActivate: [authGuard] },
  { path: 'reports/customer-analysis', component: CustomerAnalysisComponent, canActivate: [authGuard] },
  { path: 'reports/inventory-usage', component: InventoryUsageComponent, canActivate: [authGuard] },
  
  { path: '**', redirectTo: '/dashboard' }
];
