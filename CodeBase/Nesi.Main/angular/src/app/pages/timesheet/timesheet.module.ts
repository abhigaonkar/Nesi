import {
  BlockUIModule, OverlayPanelModule,  FileUploadModule,
  InputTextModule, RadioButtonModule, KeyFilterModule, ProgressBarModule
} from 'primeng/primeng';
import {ProgressSpinnerModule} from 'primeng/progressspinner';

import {
  RatingModule, TabViewModule,
  CalendarModule, DataTableModule, DialogModule,
  ButtonModule, DropdownModule, InputTextareaModule
} from 'primeng/primeng';
import { NgModule } from '@angular/core';
import { TimesheetListComponent } from './timesheet-list/timesheet-list.component';
import { TimesheetRoutingModule } from './timesheet.routing.module';
import { TimesheetService } from '../../services/pages/timesheet.service';
import { TimeSheetVisibleBusinessUnitDropDownComponent } from './visibleBusinessUnitDropDown/visibleBusinessUnitDropDown.component';
import { CommonModule } from '@angular/common';
import { FormsModule, NgModel, ReactiveFormsModule } from '@angular/forms';
import { TimesheetUserDropDownComponent } from './timesheetUserDropDown/timesheetUserDropDown.component';
import { TimesheetWorkOrderComponent } from './timesheetWorkOrder/timesheetWorkOrder.component';
import { TimesheetWorkOrderCustomerComponent } from './timesheetWorkOrderCustomer/timesheetWorkOrderCustomer.component';
import { TimesheetWorkOrderWoComponent } from './timesheetWorkOrderWo/timesheetWorkOrderWo.component';
import { TimesheetWorkOrderCommentComponent } from './timesheetWorkOrderComment/timesheetWorkOrderComment.component';
import { TimesheetWorkOrderHourTypeComponent } from './timesheetWorkOrderHourType/timesheetWorkOrderHourType.component';
import { TimesheetQuoteComponent } from './timesheetQuote/timesheetQuote.component';
import { CompletePercentComponent } from './CompletePercent/CompletePercent.component';
import { TimesheetQuoteQuoteComponent } from './timesheetQuoteQuote/timesheetQuoteQuote.component';
import { TimesheetTelemComponent } from './timesheetTelem/timesheetTelem.component';
import { TimesheetShopComponent } from './timesheetShop/timesheetShop.component';
import { PhoneCommentComponent } from '../../components/shared/phoneComment/phoneComment.component';
import {
  TooltipModule, PanelModule, MultiSelectModule, AutoCompleteModule,
  CheckboxModule, MessageModule, MessagesModule
} from 'primeng/primeng';
import { TimesheetBarMenuComponent } from './timesheetBarMenu/timesheetBarMenu.component';
import { TimesheetBankComponent } from './timesheetBank/timesheetBank.component';
import { TimesheetExpenseComponent } from './timesheetExpense/timesheetExpense.component';
import { TimesheetVacationComponent } from './timesheetVacation/timesheetVacation.component';
import { TimesheetSafetyManualComponent } from './timesheetSafetyManual/timesheetSafetyManual.component';
import { DisableControlDirective } from '../../directives/disableControl';
import { TimesheetExpenseMainComponent } from './timesheetExpenseMain/timesheetExpenseMain.component';
import { TimesheetExpensePerDiemComponent } from './timesheetExpensePerDiem/timesheetExpensePerDiem.component';
import { TimesheetExpenseHitoryComponent } from './timesheetExpenseHitory/timesheetExpenseHitory.component';
import { TimesheetVacationPastComponent } from './timesheetVacationPast/timesheetVacationPast.component';
import { TimesheetVacationScheduleComponent } from './timesheetVacationSchedule/timesheetVacationSchedule.component';
import { TimesheetVacationWithdrawalComponent } from './timesheetVacationWithdrawal/timesheetVacationWithdrawal.component';
import { ReactiveDropDownModule } from '../../components/shared/BindDropDownReactive/reactiveDropDown.module';
import { FieldErrorDisplayModule } from '../../components/shared/fieldErrorDisplay/fieldErrorDisplay.module';
import { FileManagerModule } from '../../components/shared/fileManager/fileManager.module';
import { TimesheetVacationScheduleReviewComponent } from './timesheetVacationScheduleReview/timesheetVacationScheduleReview.component';
import { BreadcurmbBarModule } from 'app/components/shared/breadcrumb/breadcurmb.module';
import { BindDropDownModule } from 'app/components/shared/BindDropDown/BindDropDown.module';
import { ShowhourComponent } from './showhour/showhour.component';
import { ShowhoursComponent } from './showhours/showhours.component';
import { TimesheetPelScheduleComponent } from './timesheet-pel-schedule/timesheet-pel-schedule.component';
import { TimesheetPelComponent } from './timesheet-pel/timesheet-pel.component';
import { TimesheetPelHistoryComponent } from './timesheet-pel-history/timesheet-pel-history.component';
import { TimesheetPelReviewComponent } from './timesheet-pel-review/timesheet-pel-review.component';
import { TimesheetPelSummaryComponent } from './timesheet-pel-summary/timesheet-pel-summary.component';
import { NesiDatatableModule } from '../../components/nesi-datatable/nesi-datatable.module';
import { BreaktimeRecordComponent } from './breaktime-record/breaktime-record.component';
import { BreaktimePopupComponent } from './breaktime-popup/breaktime-popup.component';
import {SpinnerModule} from 'primeng/spinner';
import {InputSwitchModule} from 'primeng/inputswitch';
import {ToggleButtonModule} from 'primeng/togglebutton';
import { DxSelectBoxModule,DxTemplateModule,DxListModule, DxPopupModule, DxTextBoxModule,DxValidatorModule}  from 'devextreme-angular';
import { Timesheet_Shop_ProjectsComponent } from './timesheet_Shop_Projects/timesheet_Shop_Projects.component';
import { TimesheetSignoffComponent } from './timesheet-signoff/timesheet-signoff.component';
import { SignaturePadModule } from 'angular2-signaturepad';
import { SignatureFieldComponent } from './signature-field/signature-field.component';
import { NewCustomerContactModule } from '../../components/shared/new-customer-contact/new-customer-contact.module';
import { TimesheetTransferComponent } from './timesheet-transfer/timesheet-transfer.component';

import { NgxExtendedPdfViewerModule } from 'ngx-extended-pdf-viewer';


@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    TimesheetRoutingModule,
    DataTableModule,
    ButtonModule,
    DropdownModule,
    CalendarModule,
    TabViewModule,
    InputTextareaModule,
    RatingModule,
    RadioButtonModule,
    DialogModule,
    InputTextModule,
    FileUploadModule,
    OverlayPanelModule,
    BlockUIModule,
    CheckboxModule,
    AutoCompleteModule,
    MultiSelectModule,
    PanelModule,
    TooltipModule,
    ReactiveDropDownModule,
    FieldErrorDisplayModule,
    FileManagerModule,
    BreadcurmbBarModule,
    BindDropDownModule,
    KeyFilterModule,
    ProgressBarModule,
    BlockUIModule,
    NesiDatatableModule,
    SpinnerModule,
    ToggleButtonModule,
    InputSwitchModule,
    MessageModule,
    MessagesModule,
    DxSelectBoxModule,
    DxTemplateModule,
    DxListModule, 
    DxPopupModule, 
    DxTextBoxModule,
    DxValidatorModule,
    SignaturePadModule,
    NewCustomerContactModule,
    ProgressSpinnerModule,
    NgxExtendedPdfViewerModule
  ],
  declarations: [
    TimesheetListComponent,
    TimeSheetVisibleBusinessUnitDropDownComponent,
    TimesheetUserDropDownComponent,
    TimesheetWorkOrderComponent,
    TimesheetWorkOrderCustomerComponent,
    TimesheetWorkOrderWoComponent,
    TimesheetWorkOrderCommentComponent,
    TimesheetWorkOrderHourTypeComponent,
    TimesheetQuoteComponent,
    CompletePercentComponent,
    TimesheetQuoteQuoteComponent,
    TimesheetTelemComponent,
    TimesheetShopComponent,
    PhoneCommentComponent,
    TimesheetBarMenuComponent,
    TimesheetBankComponent,
    TimesheetExpenseComponent,
    TimesheetVacationComponent,
    TimesheetSafetyManualComponent,
    TimesheetExpenseMainComponent,
    TimesheetExpensePerDiemComponent,
    TimesheetExpenseHitoryComponent,
    TimesheetVacationPastComponent,
    TimesheetVacationScheduleComponent,
    TimesheetVacationWithdrawalComponent,
    TimesheetVacationScheduleReviewComponent,
    ShowhourComponent,
    ShowhoursComponent,
    TimesheetPelScheduleComponent,
    TimesheetPelComponent,
    TimesheetPelHistoryComponent,
    TimesheetPelReviewComponent,
    TimesheetPelSummaryComponent,
    BreaktimeRecordComponent,
    BreaktimePopupComponent,
    Timesheet_Shop_ProjectsComponent,
    TimesheetSignoffComponent,
    SignatureFieldComponent,
    TimesheetTransferComponent
 
],
  providers: [
    TimesheetService,

  ]
})
export class TimesheetModule { }
