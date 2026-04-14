
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FieldErrorDisplayModule } from '../../components/shared/fieldErrorDisplay/fieldErrorDisplay.module';
import { LoadingSpinnerModule } from '../../components/shared/loading-spinner/loading-spinner.module';
import { PipesModule } from '../../pipes/Pipes.module';
import {
    ButtonModule, DialogModule, DataTableModule, TabViewModule,
    InputTextareaModule, MultiSelectModule, DropdownModule, CheckboxModule, InputTextModule, AccordionModule, CalendarModule, AutoCompleteModule, FileUploadModule
} from 'primeng/primeng';
import { VisibleBusinessUnitDropDownModule } from 'app/components/visibleBusinessUnitDropDown/visibleBusinessUnit.module';
import { PurchaseOrderPageRouterModule } from 'app/pages/purchaseOrder/purchaseorder.routing.module';
import { PurchaseOrderSummaryComponent } from 'app/pages/purchaseOrder/purchaseOrder-summary/purchaseOrder-summary.component';
import { PurchaseOrderSummaryListComponent } from 'app/pages/purchaseOrder/purchaseOrder-summary-list/purchaseOrder-summary-list.component';
import { PurchaseOrderSummaryBusinessUnitComponent } from './purchaseOrder-Summary-BusinessUnit/purchaseOrder-Summary-BusinessUnit.component';
import { PurchaseOrderSummarySumtotalComponent } from './purchaseOrder-summary-sumtotal/purchaseOrder-summary-sumtotal.component';
import { BreadcurmbBarModule } from 'app/components/shared/breadcrumb/breadcurmb.module';
import { PurchaseOrderSummaryDetailComponent } from 'app/pages/purchaseOrder/purchaseOrder-summary-detail/purchaseOrder-summary-detail.component';
import { WorkOrderService } from 'app/services/pages/workorder.service';
import { PurchaseOrderSummaryCountValueComponent } from './purchaseOrder-summary-countValue/purchaseOrder-summary-countValue.component';
import { NesiDatatableModule } from 'app/components/nesi-datatable/nesi-datatable.module';
import { PurchaseOrderCreditCardPurchasesComponent } from 'pages/purchaseOrder/purchase-order-credit-card-purchases/purchase-order-credit-card-purchases.component';
import { ReactiveDropDownModule } from 'components/shared/BindDropDownReactive/reactiveDropDown.module';
import { TimesheetRoutingModule } from 'pages/timesheet/timesheet.routing.module';
import { TimesheetService } from 'services/pages/timesheet.service';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ButtonModule,
        ReactiveFormsModule,
        LoadingSpinnerModule,
        PipesModule,
        PurchaseOrderPageRouterModule,
        DialogModule,
        DataTableModule,
        TabViewModule,
        InputTextareaModule,
        MultiSelectModule,
        DropdownModule,
        CheckboxModule,
        InputTextModule,
        DialogModule,
        AccordionModule,
        FieldErrorDisplayModule,
        VisibleBusinessUnitDropDownModule,
        BreadcurmbBarModule,
        NesiDatatableModule,
        ReactiveDropDownModule,
        ReactiveFormsModule,
        CalendarModule,
        AutoCompleteModule,
        FileUploadModule,
    ],
    declarations: [
        PurchaseOrderSummaryComponent,
        PurchaseOrderSummaryListComponent,
        PurchaseOrderSummaryBusinessUnitComponent,
        PurchaseOrderSummarySumtotalComponent,
        PurchaseOrderSummaryDetailComponent,
        PurchaseOrderSummaryCountValueComponent,
        PurchaseOrderCreditCardPurchasesComponent
],
    providers: [
        WorkOrderService,
        TimesheetService
    ],
    exports: [
        PurchaseOrderSummaryListComponent
    ]
})
export class PurchaseOrderPageModule { }
