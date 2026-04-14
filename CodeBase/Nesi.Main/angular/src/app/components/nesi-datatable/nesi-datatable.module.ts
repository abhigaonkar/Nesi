import { NgModule } from '@angular/core';
import { CommonModule, LocationStrategy, HashLocationStrategy, DatePipe, CurrencyPipe, PercentPipe, DecimalPipe } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

// Primeng Modules
import {
  RadioButtonModule, CalendarModule, DialogModule, ButtonModule, InputTextModule,
  ContextMenuModule, GrowlModule, TabViewModule, CodeHighlighterModule, SliderModule,
  CheckboxModule, BlockUIModule,
  PanelModule, DragDropModule,
  OverlayPanelModule, DomHandler,
  TooltipModule, InputTextareaModule, SpinnerModule, ToggleButtonModule, SidebarModule
} from 'primeng/primeng';

// Primeng Customized Modules
import { DataTableModule } from './components/primeng-custom-library/datatable/datatable';
import { MultiSelectModule } from './components/primeng-custom-library/multiselect/multiselect';
import { ListboxModule } from './components/primeng-custom-library/listbox/listbox';
import { DropdownModule } from './components/primeng-custom-library/dropdown/dropdown';

import { DatatableComponent } from './components/datatable/datatable.component';
import { DataService } from './service/dataservice';
import { LocalDbService } from './service/localDbService';
import { UtilityService } from './service/utilityService';
import { ExportComponent } from './components/export/export.component';
import { ThemeComponent } from './components/theme/theme.component';
import { AddEditFormModule } from '../shared/add-edit-form/add-edit-form.module';
import { TextAreaLayover } from './components/textAreaLayover/textAreaLayover.component';
import { BlockableDiv } from './components/blockable-div/blockableDiv.component';
import { PipesModule } from '../../pipes/Pipes.module';
import { LayoutDatatableComponent } from './components/layout-datatable/layout-datatable.component';
import { DateControl } from './components/date-control/date.control.component';
import { LayoutService } from 'app/components/nesi-datatable/service/layoutService';
import { PhoneFormatPipe } from 'app/pipes/phoneFormat.pipe';
import { FilterBuilderComponent } from 'app/components/nesi-datatable/components/filter-builder/filter-builder.component';

import { IndexDirective } from './components/directives/index.directive';

import { RatingModule } from 'primeng/primeng';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MultiSelectModule,
    RadioButtonModule,
    ListboxModule,
    DataTableModule,
    DropdownModule,
    CalendarModule,
    InputTextModule,
    DialogModule,
    CheckboxModule,
    BlockUIModule,
    PanelModule,
    DragDropModule,
    OverlayPanelModule,
    ButtonModule,
    ContextMenuModule,
    SliderModule,
    GrowlModule,
    TabViewModule,
    CodeHighlighterModule,
    AddEditFormModule,
    PipesModule,
    TooltipModule,
    InputTextareaModule,
    RatingModule,
    SpinnerModule,
    SidebarModule
  ],
  declarations: [
    ExportComponent,
    DatatableComponent,
    TextAreaLayover,
    BlockableDiv,
    ThemeComponent,
    LayoutDatatableComponent,
    DateControl,
    FilterBuilderComponent,
    IndexDirective
  ],
  providers: [
    { provide: LocationStrategy, useClass: HashLocationStrategy },
    DataService,
    LocalDbService,
    UtilityService,
    LayoutService,
    DomHandler,
    DatePipe,
    CurrencyPipe,
    PercentPipe,
    DecimalPipe,
    PhoneFormatPipe
  ],
  exports: [
    DatatableComponent,
    TextAreaLayover,
    BlockableDiv,
  ]
})
export class NesiDatatableModule {
}
