import { AuthGuard } from './../../services/authentication/authGuard';
import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { CustomerAssetComponent } from './customerAsset/customerAsset.component';
import { MasterContactComponent } from './masterContact/masterContact.component';
import { MasterVendorComponent } from './masterVendor/masterVendor.component';
import { MasterWorkOrderComponent } from './masterWorkOrder/masterWorkOrder.component';
import { MasterWorkOrderComponentV2 } from './masterWorkOrderV2/masterWorkOrderV2.component';
import { AdvancepayreportComponent } from './advancepayreport/advancepayreport.component';
import { CustomerLoginHistoryComponent } from './customer-login-history/customer-login-history.component';
import { CustomerRatesGridComponent } from './customer-rates-grid/customer-rates-grid.component';
import { CustomerSurveysComponent } from './customer-surveys/customer-surveys.component';
import { FvrGridComponent } from './fvr-grid/fvr-grid.component';
import { InventoryCountsComponent } from './inventory-counts/inventory-counts.component';
import { InventoryUsageComponent } from './inventory-usage/inventory-usage.component';
import { InvoicingTimeComponent } from './invoicing-time/invoicing-time.component';
import { JobCostReportComponent } from './job-cost-report/job-cost-report.component';
import { MasterQuoteGridComponent } from './master-quote-grid/master-quote-grid.component';
import { CustStatusHistoryComponent } from './cust-status-history/cust-status-history.component';
import { MasterPhoneCallGridComponent } from './master-phone-call-grid/master-phone-call-grid.component';
import { MasterResponsiblilitesGridComponent } from './master-responsiblilites-grid/master-responsiblilites-grid.component';
import { PartnerSpecialtiesComponent } from './partner-specialties/partner-specialties.component';
import { PartnerSpecialtiesCustomerComponent } from './partner-specialties-customer/partner-specialties-customer.component';
import { WoLineGridComponent } from './wo-line-grid/wo-line-grid.component';
import { MasterPurchasesGridComponent } from './master-purchases-grid/master-purchases-grid.component';
import { MasterOutstandingInvoicesGridComponent } from './master-outstanding-invoices-grid/master-outstanding-invoices-grid.component';
import { IncomeStatementComponent } from './income-statement/income-statement.component';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '126/customer_assets', component: CustomerAssetComponent,
                data: { title: 'Cust Assets' }, canActivate: [AuthGuard]
            },
            {
                path: '211/master_vendors', component: MasterVendorComponent,
                data: { title: 'Master Vendors' }, canActivate: [AuthGuard]
            },
            {
                path: '120/master_contacts', component: MasterContactComponent,
                data: { title: 'Master Contacts Grid' }, canActivate: [AuthGuard]
            },
            {
                path: '77/master_workorder', component: MasterWorkOrderComponent,
                data: { title: 'Master Work Order Grid' }, canActivate: [AuthGuard]
            },
            {
                path: '77/master_workorder/:bu_id/:status', component: MasterWorkOrderComponent,
                data: { title: 'Master Work Order Grid' }, canActivate: [AuthGuard]
            },
            {
                path: '193/master_workorder2', component: MasterWorkOrderComponentV2,
                data: { title: 'Master Work Order Grid 2' }, canActivate: [AuthGuard]
            },
            {
                path: '219/advance_pay_report', component: AdvancepayreportComponent,
                data: { title: 'Advance Pay Report Grid' }, canActivate: [AuthGuard]
            },
            {
                path: '157/customer_login_history', component: CustomerLoginHistoryComponent,
                data: { title: 'Customer Login History Grid' }, canActivate: [AuthGuard]
            },
            {
                path: '125/customer_rates_grid', component: CustomerRatesGridComponent,
                data: { title: 'Customer Rates Grid' }, canActivate: [AuthGuard]
            },
            {
                path: '161/customer_surveys', component: CustomerSurveysComponent,
                data: { title: 'Customer Surveys' }, canActivate: [AuthGuard]
            },
            {
                path: '188/fvr_grid', component: FvrGridComponent,
                data: { title: 'FVR Grid' }, canActivate: [AuthGuard]
            },
            {
                path: '103/inventory_counts', component: InventoryCountsComponent,
                data: { title: 'Inventory Counts' }, canActivate: [AuthGuard]
            },
            {
                path: '97/inventory_usage', component: InventoryUsageComponent,
                data: { title: 'Inventory Usage' }, canActivate: [AuthGuard]
            },
            {
                path: '192/invoicing_time', component: InvoicingTimeComponent,
                data: { title: 'Invoicing Time' }, canActivate: [AuthGuard]
            },
            {
                path: '59/job_cost_report', component: JobCostReportComponent,
                data: { title: 'Job Cost Report' }, canActivate: [AuthGuard]
            },
            {
                path: '78/master_quote_grid', component: MasterQuoteGridComponent,
                data: { title: 'Master Quote Grid' }, canActivate: [AuthGuard]
            },
            {
                path: '141/cust_status_history', component: CustStatusHistoryComponent,
                data: { title: 'Cust Status History' }, canActivate: [AuthGuard]
            },
            {
                path: '203/master_phone_call_grid', component: MasterPhoneCallGridComponent,
                data: { title: 'Master Phone Call Grid' }, canActivate: [AuthGuard]
            },
            {
                path: '150/master_responsibilities_grid', component: MasterResponsiblilitesGridComponent,
                data: { title: 'Master Responsibilities Grid' }, canActivate: [AuthGuard]
            },
            {
                path: '196/partner_specialties/:type/:id', component: PartnerSpecialtiesCustomerComponent,
                data: { title: 'Partner Specialties' }, canActivate: [AuthGuard],
            },
            {
                path: '196/partner_specialties', component: PartnerSpecialtiesComponent,
                data: { title: 'Partner Specialties' }, canActivate: [AuthGuard],
            },
            {
                path: '105/wo_line_grid', component: WoLineGridComponent,
                data: { title: 'WO Line Grid' }, canActivate: [AuthGuard],
            },
            {
                path: '102/master_purchases_grid', component: MasterPurchasesGridComponent,
                data: { title: 'Master Purchases Grid' }, canActivate: [AuthGuard],
            },
            {
                path: '113/master_outstanding_invoicesGrid', component: MasterOutstandingInvoicesGridComponent,
                data: { title: 'Master Outstanding Invoices Grid' }, canActivate: [AuthGuard],
            },
            {
                path: '52/income_statement_report', component: IncomeStatementComponent,
                data: { title: 'Income Statement Report' }, canActivate: [AuthGuard],
            },
        ]),
    ],
    exports: [RouterModule]
})

export class ReportsRoutingModule { }

