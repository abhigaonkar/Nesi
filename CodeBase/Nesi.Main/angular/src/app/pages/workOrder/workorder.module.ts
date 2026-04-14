
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FieldErrorDisplayModule } from '../../components/shared/fieldErrorDisplay/fieldErrorDisplay.module';
import { LoadingSpinnerModule } from '../../components/shared/loading-spinner/loading-spinner.module';
import { PipesModule } from '../../pipes/Pipes.module';
import {
    ButtonModule, DialogModule, DataTableModule, TabViewModule,
    InputTextareaModule, MultiSelectModule, DropdownModule, CheckboxModule,
    InputTextModule, AccordionModule, ScrollPanelModule, TooltipModule, BlockUIModule,
    FileUploadModule, MenubarModule, SidebarModule, PanelModule
} from 'primeng/primeng';
import { WorkOrderPageRouterModule } from 'app/pages/workOrder/workorder.routing.module';
import { WorkOrderSummaryComponent } from 'app/pages/workOrder/workOrder-summary/workOrder-summary.component';
import { WorkOrderSummaryBusinessUnitComponent } from './workOrder-Summary-BusinessUnit/workOrder-Summary-BusinessUnit.component';
import { WorkOrderSummaryCountValueComponent } from './workOrder-summary-countValue/workOrder-summary-countValue.component';
import { WorkOrderService } from 'app/services/pages/workorder.service';
import { WorkOrderSummarySumtotalComponent } from './workOrder-summary-sumtotal/workOrder-summary-sumtotal.component';
import { WorkOrderSummaryListComponent } from './workOrder-summary-list/workOrder-summary-list.component';
import { WorkOrderSummaryDetailComponent } from './workOrder-summary-detail/workOrder-summary-detail.component';
import { VisibleBusinessUnitDropDownModule } from 'app/components/visibleBusinessUnitDropDown/visibleBusinessUnit.module';
import { BreadcurmbBarModule } from 'app/components/shared/breadcrumb/breadcurmb.module';
import { NesiDatatableModule } from 'app/components/nesi-datatable/nesi-datatable.module';
import { WorkOrderBucketsComponent } from './workOrder-buckets/workOrder-buckets.component';
import { WorkorderBucketScannedComponent } from './_workorder-buckets/workorder-bucket-scanned/workorder-bucket-scanned.component';
import { WorkorderBucketOpenComponent } from './_workorder-buckets/workorder-bucket-open/workorder-bucket-open.component';
import { WorkorderBucketOpenItemComponent } from './_workorder-buckets/workorder-bucket-open-item/workorder-bucket-open-item.component';
// tslint:disable-next-line:max-line-length
import { WorkorderBucketScannedItemComponent } from './_workorder-buckets/workorder-bucket-scanned-item/workorder-bucket-scanned-item.component';
import { WorkorderScannedFileComponent } from './workorder-scanned-file/workorder-scanned-file.component';
import { SplitterModule } from '../../components/shared/splitter/splitter.module';
import { WorkorderSplitScannedFileComponent } from './workorder-split-scanned-file/workorder-split-scanned-file.component';
import { WorkorderShowScannedFileComponent } from './workorder-show-scanned-file/workorder-show-scanned-file.component';
import { WorkorderMainComponent } from './workorder-main/workorder-main.component';
import { WorkorderAnalysisComponent } from './workorder-analysis/workorder-analysis.component';
import { WorkorderGeneralComponent } from './workorder-general/workorder-general.component';
import { WorkorderCustomerComponent } from './workorder-customer/workorder-customer.component';
import { WorkorderLineitemsComponent } from './workorder-lineitems/workorder-lineitems.component';
import { WorkorderPosComponent } from './workorder-pos/workorder-pos.component';
import { WorkorderTimeEntriesComponent } from './workorder-time-entries/workorder-time-entries.component';
import { WorkorderHistoryComponent } from './workorder-history/workorder-history.component';
import { WorkorderChecklistComponent } from './workorder-checklist/workorder-checklist.component';
import { WorkorderScopeComponent } from './workorder-scope/workorder-scope.component';
import { WorkorderLinkedWosComponent } from './workorder-linked-wos/workorder-linked-wos.component';
import { WorkorderProjectFolderComponent } from './workorder-project-folder/workorder-project-folder.component';
import { WorkorderProjectNotesComponent } from './workorder-project-notes/workorder-project-notes.component';
import { WorkorderMainWithFileComponent } from './workorder-main-with-file/workorder-main-with-file.component';
import { WorkorderMainButtonsMenuComponent } from './_workorder_main/workorder-main-buttons-menu/workorder-main-buttons-menu.component';
import { WorkorderWocommentsComponent } from './workorder-wocomments/workorder-wocomments.component';
// tslint:disable-next-line:max-line-length
import { WorkorderWocommentsDisplayCommentsComponent } from './_workorder-wocomments/workorder-wocomments-display-comments/workorder-wocomments-display-comments.component';
import { NesiSideBarModule } from '../../components/shared/nesi-sideBar/nesi-sideBar.module';
import { WorkorderMainTabsComponent } from './_workorder_main/workorder-main-tabs/workorder-main-tabs.component';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ButtonModule,
        ReactiveFormsModule,
        LoadingSpinnerModule,
        PipesModule,
        WorkOrderPageRouterModule,
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
        ScrollPanelModule,
        TooltipModule,
        SplitterModule,
        BlockUIModule,
        FileUploadModule,
        MenubarModule,
        SidebarModule,
        NesiSideBarModule,
        PanelModule,
    ],
    declarations: [
        WorkOrderSummaryComponent,
        WorkOrderSummaryBusinessUnitComponent,
        WorkOrderSummaryCountValueComponent,
        WorkOrderSummarySumtotalComponent,
        WorkOrderSummaryListComponent,
        WorkOrderSummaryDetailComponent,
        WorkOrderBucketsComponent,
        WorkorderBucketScannedComponent,
        WorkorderBucketOpenComponent,
        WorkorderBucketOpenItemComponent,
        WorkorderBucketScannedItemComponent,
        WorkorderScannedFileComponent,
        WorkorderSplitScannedFileComponent,
        WorkorderShowScannedFileComponent,
        WorkorderMainComponent,
        WorkorderAnalysisComponent,
        WorkorderGeneralComponent,
        WorkorderCustomerComponent,
        WorkorderLineitemsComponent,
        WorkorderPosComponent,
        WorkorderTimeEntriesComponent,
        WorkorderHistoryComponent,
        WorkorderChecklistComponent,
        WorkorderScopeComponent,
        WorkorderLinkedWosComponent,
        WorkorderProjectFolderComponent,
        WorkorderProjectNotesComponent,
        WorkorderMainWithFileComponent,
        WorkorderMainButtonsMenuComponent,
        WorkorderWocommentsComponent,
        WorkorderWocommentsDisplayCommentsComponent,
        WorkorderMainTabsComponent
    ],
    providers: [
        WorkOrderService,
    ],
    exports: [
        WorkOrderSummaryComponent,
        WorkorderWocommentsComponent,
    ]
})
export class WorkOrderPageModule { }
