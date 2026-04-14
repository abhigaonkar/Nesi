
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {
  DataGridModule, DropdownModule,
  InputTextModule, DataTableModule, OverlayPanelModule, DialogModule,
  InputTextareaModule, CheckboxModule, TooltipModule, PanelModule,
  BlockUIModule, InplaceModule, AccordionModule, DataListModule, SplitButtonModule, ListboxModule, ToggleButtonModule, MegaMenuModule, MenubarModule, DragDropModule
} from 'primeng/primeng';

import { PicklistSourceComponent } from './picklist-source/picklist-source.component';
import { PicklistItemComponent } from './picklist-item/picklist-item.component';
import { PicklistSelectItemComponent } from './picklist-selectItem/picklist-selectItem.component';
import { PicklistColorComponent } from './picklist-color/picklist-color.component';
import { PicklistNoteComponent } from './picklist-note/picklist-note.component';
import { PicklistMemberTypeLaborComponent } from './picklist-member-type-labor/picklist-member-type-labor.component';
import { PicklistMainComponent } from './picklist-main/picklist-main.component';
import { ReactiveDropDownModule } from '../BindDropDownReactive/reactiveDropDown.module';
import { ButtonModule } from 'primeng/primeng';
import { PicklistAddNoteComponent } from './picklist-add-note/picklist-add-note.component';
import { PicklistEditNoteComponent } from './picklist-edit-note/picklist-edit-note.component';
import { FieldErrorDisplayModule } from '../../../components/shared/fieldErrorDisplay/fieldErrorDisplay.module';
import { PicklistSearchMaterialComponent } from './picklist-search-material/picklist-search-material.component';
import { AutoCompleteModule } from 'primeng/components/autocomplete/autocomplete';
import { PicklistSearchLaborComponent } from './picklist-search-labor/picklist-search-labor.component';
import { PicklistSearchKittedComponent } from './picklist-search-kitted/picklist-search-kitted.component';
import { PicklistSearchGroupComponent } from './picklist-search-group/picklist-search-group.component';
import { PicklistSearchQuoteComponent } from './picklist-search-quote/picklist-search-quote.component';
import { PicklistSearchWorkorderComponent } from './picklist-search-workorder/picklist-search-workorder.component';
import { PicklistSearchPoComponent } from './picklist-search-po/picklist-search-po.component';
import { PicklistSearchRepairComponent } from './picklist-search-repair/picklist-search-repair.component';
import { PicklistSearchRFQComponent } from './picklist-search-RFQ/picklist-search-RFQ.component';
import { PicklistAnlysisComponent } from './picklist-anlysis/picklist-anlysis.component';
import { PicklistSearchInventoryComponent } from './picklist-search-inventory/picklist-search-inventory.component';
import { InventoryImageComponent } from './inventory-image/inventory-image.component';
import { InventoryRowdataComponent } from './inventory-rowdata/inventory-rowdata.component';
import { MultiSelectModule } from 'primeng/components/multiselect/multiselect';
import { PicklistSearchBlankLineComponent } from './picklist-search-blank-line/picklist-search-blank-line.component';
import { PicklistEditSectionsComponent } from './picklist-edit-sections/picklist-edit-sections.component';
import { PicklistCopyMoveItemsComponent } from './picklist-copy-move-items/picklist-copy-move-items.component';
import { PipesModule } from 'app/pipes/Pipes.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    DropdownModule,
    InputTextModule,
    InputTextareaModule,
    DataTableModule,
    DataGridModule,
    OverlayPanelModule,
    ReactiveDropDownModule,
    ButtonModule,
    DialogModule,
    FieldErrorDisplayModule,
    AutoCompleteModule,
    FieldErrorDisplayModule,
    CheckboxModule,
    TooltipModule,
    PanelModule,
    BlockUIModule,
    InplaceModule,
    AccordionModule,
    DataListModule,
    MultiSelectModule,
    SplitButtonModule,
    ListboxModule,
    ToggleButtonModule,
    MenubarModule,
    DragDropModule,
    PipesModule
  ],
  declarations: [
    PicklistSourceComponent,
    PicklistItemComponent,
    PicklistSelectItemComponent,
    PicklistColorComponent,
    PicklistNoteComponent,
    PicklistMemberTypeLaborComponent,
    PicklistMainComponent,
    PicklistAddNoteComponent,
    PicklistEditNoteComponent,
    PicklistSearchMaterialComponent,
    PicklistSearchLaborComponent,
    PicklistSearchKittedComponent,
    PicklistSearchGroupComponent,
    PicklistSearchQuoteComponent,
    PicklistSearchWorkorderComponent,
    PicklistSearchPoComponent,
    PicklistSearchRepairComponent,
    PicklistSearchRFQComponent,
    PicklistAnlysisComponent,
    PicklistSearchInventoryComponent,
    InventoryImageComponent,
    InventoryRowdataComponent,
    PicklistSearchBlankLineComponent
,
    PicklistEditSectionsComponent
,
    PicklistCopyMoveItemsComponent
],
  exports: [
    PicklistMainComponent,
  ],
})
export class PickListModule { }
