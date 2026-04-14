import {
  TabViewModule, AccordionModule,
  AutoCompleteModule, ListboxModule, CheckboxModule, DialogModule, DropdownModule, MessageModule, MessagesModule, EditorModule,
   BlockUIModule, MultiSelectModule, DragDropModule, TooltipModule
} from 'primeng/primeng';

import { NgModule } from '@angular/core';

import { QuotesListComponent } from './quotes-list/quotes-list.component';
import { QuotesRouterModule } from './quotes.routing.module';
import {
  SpinnerModule, CalendarModule, SharedModule,
  InputTextModule, ButtonModule, DataTableModule, InputTextareaModule
} from 'primeng/primeng';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { QuoteMenuComponent } from './quoteMenu/quoteMenu.component';
import { QuoteAddnewComponent } from './quote-addnew/quote-addnew.component';
import { QuoteSearchComponent } from './quote-search/quote-search.component';
import { QuoteListItemComponent } from './quote-list-item/quote-list-item.component';
import { QuoteDetailEditComponent } from './quote-detail-edit/quote-detail-edit.component';
import { QuoteSearchPaneComponent } from './quote-search-pane/quote-search-pane.component';
import { ReactiveDropDownModule } from '../../components/shared/BindDropDownReactive/reactiveDropDown.module';
import { FieldErrorDisplayModule } from '../../components/shared/fieldErrorDisplay/fieldErrorDisplay.module';
import { QuoteAddnewPaneComponent } from './quote-addnew-pane/quote-addnew-pane.component';
import { NewCustomerContactModule } from '../../components/shared/new-customer-contact/new-customer-contact.module';
import { ResponsiveModule } from 'ng2-responsive';
import { QuoteEditGeneralComponent } from './quote-edit-general/quote-edit-general.component';
import { QuoteEditScoreworkComponent } from './quote-edit-scorework/quote-edit-scorework.component';
import { QuoteEditNoteadddressComponent } from './quote-edit-noteadddress/quote-edit-noteadddress.component';
import { QuoteEditMaintenanceComponent } from './quote-edit-maintenance/quote-edit-maintenance.component';
import { QuoteEditStorageComponent } from './quote-edit-storage/quote-edit-storage.component';
import { QuoteEditGeneralDetailComponent } from './quote-edit-general-detail/quote-edit-general-detail.component';
import { QuoteEditGeneralCustomerComponent } from './quote-edit-general-customer/quote-edit-general-customer.component';
import { QuoteButtonAddResizeComponent } from './shared/buttonAddResize/buttonAddResize.component';
import { PanelModule, ToggleButtonModule } from 'primeng/primeng';
import { FileManagerModule } from '../../components/shared/fileManager/fileManager.module';
import { QuoteDetailNewrevisionComponent } from './quote-detail-newrevision/quote-detail-newrevision.component';
import { QuoteEditKillComponent } from './quote-edit-kill/quote-edit-kill.component';
import { QuoteDetailEditStrategyComponent } from './quote-detail-edit-strategy/quote-detail-edit-strategy.component';
import { QuoteDetailEditStrategyItemComponent } from './quote-detail-edit-strategy-item/quote-detail-edit-strategy-item.component';
// tslint:disable-next-line:max-line-length
import { QuoteDetailEditStrategyScheduleComponent } from './quote-detail-edit-strategy-schedule/quote-detail-edit-strategy-schedule.component';
import { LoadingSpinnerModule } from '../../components/shared/loading-spinner/loading-spinner.module';
import { QuoteWorksheetComponent } from './quote-worksheet/quote-worksheet.component';
import { PickListModule } from '../../components/shared/picklist/picklist.module';
import { QuoteStage1Component } from './quote-stage1/quote-stage1.component';
// tslint:disable-next-line:max-line-length
import { QuoteDetailEditStrategyQuestionComponent } from './quote-detail-edit-strategy-question/quote-detail-edit-strategy-question.component';
import { QuoteDetailEditStrategyReviewComponent } from './quote-detail-edit-strategy-review/quote-detail-edit-strategy-review.component';
import { QuoteAddSectionFromQuoteComponent } from './quote-add-section-from-quote/quote-add-section-from-quote.component';
import { BreadcurmbBarModule } from 'app/components/shared/breadcrumb/breadcurmb.module';
import { QuoteAddSectionFromSpecificComponent } from './quote-add-section-from-specific/quote-add-section-from-specific.component';
import { VisibleBusinessUnitDropDownModule } from 'app/components/visibleBusinessUnitDropDown/visibleBusinessUnit.module';
import { PipesModule } from 'app/pipes/Pipes.module';

@NgModule({
  imports: [
    QuotesRouterModule,
    ButtonModule,
    CommonModule,
    InputTextModule,
    FormsModule,
    DataTableModule,
    SharedModule,
    DialogModule,
    ReactiveFormsModule,
    ReactiveDropDownModule,
    CalendarModule,
    FieldErrorDisplayModule,
    SpinnerModule,
    CheckboxModule,
    AutoCompleteModule,
    NewCustomerContactModule,
    AccordionModule,
    DropdownModule,
    TabViewModule,
    InputTextareaModule,
    ToggleButtonModule,
    PanelModule,
    FileManagerModule,
    LoadingSpinnerModule,
    PickListModule,
    MessageModule,
    MessagesModule,
    EditorModule,
    BlockUIModule,
    MultiSelectModule,
    DragDropModule,
    TooltipModule,
    BreadcurmbBarModule,
    VisibleBusinessUnitDropDownModule,
    ToggleButtonModule,
    PipesModule,
    PanelModule,
  ],
  declarations: [
    QuotesListComponent,
    QuoteMenuComponent,
    QuoteAddnewComponent,
    QuoteSearchComponent,
    QuoteListItemComponent,
    QuoteDetailEditComponent,
    QuoteSearchPaneComponent,
    QuoteAddnewPaneComponent,
    QuoteEditGeneralComponent,
    QuoteEditScoreworkComponent,
    QuoteEditNoteadddressComponent,
    QuoteEditMaintenanceComponent,
    QuoteEditStorageComponent,
    QuoteEditGeneralDetailComponent,
    QuoteEditGeneralCustomerComponent,
    QuoteButtonAddResizeComponent,
    QuoteDetailNewrevisionComponent,
    QuoteEditKillComponent,
    QuoteDetailEditStrategyComponent,
    QuoteDetailEditStrategyItemComponent,
    QuoteDetailEditStrategyScheduleComponent,
    QuoteWorksheetComponent,
    QuoteStage1Component,
    QuoteDetailEditStrategyQuestionComponent,
    QuoteDetailEditStrategyReviewComponent,
    QuoteAddSectionFromQuoteComponent,
    QuoteAddSectionFromSpecificComponent
],
  providers: [

  ],
  exports: [
    QuoteDetailEditComponent,
 ]
})
export class QuotesModule { }
