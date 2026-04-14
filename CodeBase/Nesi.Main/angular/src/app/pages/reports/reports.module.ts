import { NesiDatatableModule } from '../../components/nesi-datatable/nesi-datatable.module';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { CustomerAssetComponent } from './customerAsset/customerAsset.component';
import { ReportsRoutingModule } from './reports.routing.module';
import { CommonModule } from '@angular/common';
import { MasterContactComponent } from './masterContact/masterContact.component';
import { MasterVendorComponent } from './masterVendor/masterVendor.component';
import { MasterWorkOrderComponent } from './masterWorkOrder/masterWorkOrder.component';
import { MasterWorkOrderComponentV2 } from './masterWorkOrderV2/masterWorkOrderV2.component';

import {
    ButtonModule, DialogModule, DataTableModule, TabViewModule,
    InputTextareaModule, MultiSelectModule,
    DropdownModule, CheckboxModule, InputTextModule,
    FileUploadModule, BlockUIModule, MessageModule, MessagesModule, PanelModule, CalendarModule, EditorModule, ListboxModule,
    SpinnerModule, OverlayPanelModule
} from 'primeng/primeng';
import { EditorComponent } from './masterWorkOrder/editor/editor.component';
import { AdvancepayreportComponent } from './advancepayreport/advancepayreport.component';
import { CustomerLoginHistoryComponent } from './customer-login-history/customer-login-history.component';
import { CustomerRatesGridComponent } from './customer-rates-grid/customer-rates-grid.component';
import { CustomerSurveysComponent } from './customer-surveys/customer-surveys.component';
import { FvrGridComponent } from './fvr-grid/fvr-grid.component';
import { InventoryCountsComponent } from './inventory-counts/inventory-counts.component';
import { VisibleBusinessUnitDropDownModule } from 'app/components/visibleBusinessUnitDropDown/visibleBusinessUnit.module';
import { InventoryUsageComponent } from './inventory-usage/inventory-usage.component';
import { BreadcurmbBarModule } from '../../components/shared/breadcrumb/breadcurmb.module';
import { InvoicingTimeComponent } from './invoicing-time/invoicing-time.component';
import { JobCostReportComponent } from './job-cost-report/job-cost-report.component';
import { MasterQuoteGridComponent } from './master-quote-grid/master-quote-grid.component';
import { MasterQuoteEditorComponent } from './master-quote-grid/editor/editor.component';
import { CustStatusHistoryComponent } from './cust-status-history/cust-status-history.component';
import { MasterPhoneCallGridComponent } from './master-phone-call-grid/master-phone-call-grid.component';
import { MasterResponsiblilitesGridComponent } from './master-responsiblilites-grid/master-responsiblilites-grid.component';
import { PartnerSpecialtiesComponent } from './partner-specialties/partner-specialties.component';
import { PartnerSpecialtiesCustomerComponent } from './partner-specialties-customer/partner-specialties-customer.component';
import { WoLineGridComponent } from './wo-line-grid/wo-line-grid.component';
import { MasterPurchasesGridComponent } from './master-purchases-grid/master-purchases-grid.component';
import { MasterPurchasesGridEditorComponent } from './master-purchases-grid/editor/editor.component';
import { MasterOutstandingInvoicesGridComponent } from './master-outstanding-invoices-grid/master-outstanding-invoices-grid.component';
import { MasterOutstandingInvoiceEditorComponent } from './master-outstanding-invoices-grid/editor/editor.component';
import { NoteEditorComponent } from './master-outstanding-invoices-grid/note-editor/note-editor.component';
import { IncomeStatementComponent } from './income-statement/income-statement.component';
import { GridComponent } from './income-statement/grid/grid.component';
import { DxDataGridModule, DxPopupModule, DxSelectBoxModule, DxListModule, DxButtonModule, DxLoadPanelModule,
    DxResponsiveBoxModule, DxFormModule } from 'devextreme-angular';
import { ParameterComponentComponent } from './income-statement/parameter-component/parameter-component.component';
import { NetsuiteGridComponent } from './income-statement/netsuite-grid/netsuite-grid.component';
import { ReturnViewComponent } from './income-statement/return-view/return-view.component';

@NgModule({
    imports: [
        CommonModule,
        NesiDatatableModule,
        ReportsRoutingModule,
        ButtonModule,
        ReactiveFormsModule,
        FormsModule,
        DialogModule,
        DataTableModule,
        TabViewModule,
        InputTextareaModule,
        MultiSelectModule,
        DropdownModule,
        CheckboxModule,
        InputTextModule,
        DialogModule,
        FileUploadModule,
        NesiDatatableModule,
        BlockUIModule,
        MessagesModule,
        PanelModule,
        CalendarModule,
        InputTextareaModule,
        EditorModule,
        ListboxModule,
        VisibleBusinessUnitDropDownModule,
        BreadcurmbBarModule,
        SpinnerModule,
        OverlayPanelModule,
        DxDataGridModule,
        DxPopupModule,
        DxSelectBoxModule,
        DxListModule,
        DxButtonModule,
        DxLoadPanelModule,
        DxResponsiveBoxModule,
        DxFormModule,
        MessageModule
    ],
    declarations: [
        CustomerAssetComponent,
        MasterContactComponent,
        MasterVendorComponent,
        MasterWorkOrderComponent,
        MasterWorkOrderComponentV2,
        EditorComponent,
        AdvancepayreportComponent,
        CustomerLoginHistoryComponent,
        CustomerRatesGridComponent,
        CustomerSurveysComponent,
        FvrGridComponent,
        InventoryCountsComponent,
        InventoryUsageComponent,
        InvoicingTimeComponent,
        JobCostReportComponent,
        MasterQuoteGridComponent,
        MasterQuoteEditorComponent,
        CustStatusHistoryComponent,
        MasterPhoneCallGridComponent,
        MasterResponsiblilitesGridComponent,
        PartnerSpecialtiesComponent,
        PartnerSpecialtiesCustomerComponent,
        WoLineGridComponent,
        MasterPurchasesGridComponent,
        MasterPurchasesGridEditorComponent,
        MasterOutstandingInvoicesGridComponent,
        MasterOutstandingInvoiceEditorComponent,
        NoteEditorComponent,
        IncomeStatementComponent,
        GridComponent,
        ParameterComponentComponent,
        NetsuiteGridComponent,
        ReturnViewComponent,
    ],
    providers: [

    ],
})
export class ReportsModule { }

