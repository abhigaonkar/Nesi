import { NgModule } from '@angular/core';
import { BusinessUnitService } from '../../services/pages/business_unit.service';
import { FieldErrorDisplayModule } from '../../components/shared/fieldErrorDisplay/fieldErrorDisplay.module';
import {
  InputTextModule, ButtonModule, DataTableModule, TabViewModule,
  MultiSelectModule, FieldsetModule, DropdownModule,
  BlockUIModule, CheckboxModule, AccordionModule,
  PanelModule, InputTextareaModule, CalendarModule,
  SelectButtonModule, DialogModule
} from 'primeng/primeng';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PipesModule } from 'app/pipes/Pipes.module';
import { CommonModule } from '@angular/common';
import { BreadcurmbBarModule } from 'app/components/shared/breadcrumb/breadcurmb.module';
import { NesiDatatableModule } from 'app/components/nesi-datatable/nesi-datatable.module';
import { FileManagerModule } from 'app/components/shared/fileManager/fileManager.module';
import { VendorListComponent } from './vendor-list/vendor-list.component';
import { VendorOpensComponent } from './vendor-opens/vendor-opens.component';
import { VendorsRoutingModule } from './vendors.routing.module';
import { VendorEditComponent } from './vendor-edit/vendor-edit.component';
import { VendorsService } from '../../services/pages/vendor.services';
import { VendorEditGeneralComponent } from './vendor-edit-general/vendor-edit-general.component';
import { VendorNewComponent } from './vendor-new/vendor-new.component';
import { VendorExistsComponent } from './vendor-exists/vendor-exists.component';
import { VendorAddressComponent } from './vendor-address/vendor-address.component';
import { VendorContactsComponent } from './vendor-contacts/vendor-contacts.component';
import { VendorAccountingComponent } from './vendor-accounting/vendor-accounting.component';
import { VendorNotesComponent } from './vendor-notes/vendor-notes.component';
import { VendorPurchasesComponent } from './vendor-purchases/vendor-purchases.component';
import { VendorPhoneNumbersComponent } from './vendor-phone-numbers/vendor-phone-numbers.component';
import { VendorEmailsComponent } from './vendor-emails/vendor-emails.component';
import { VendorPhoneCallsComponent } from './vendor-phone-calls/vendor-phone-calls.component';
import { VendorFilesComponent } from './vendor-files/vendor-files.component';
import { VendorMasrketComponent } from './vendor-masrket/vendor-masrket.component';
import { VendorSpecialtiesComponent } from './vendor-specialties/vendor-specialties.component';
import { VendorContactsNesiComponent } from './vendor-contacts-nesi/vendor-contacts-nesi.component';
import { VendorContactsBvComponent } from './vendor-contacts-bv/vendor-contacts-bv.component';
import { VendorContactsBvItemComponent } from './vendor-contacts-bv-item/vendor-contacts-bv-item.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    FieldErrorDisplayModule,
    InputTextModule,
    ButtonModule,
    DataTableModule,
    PipesModule,
    BreadcurmbBarModule,
    TabViewModule,
    MultiSelectModule,
    NesiDatatableModule,
    FieldsetModule,
    DropdownModule,
    BlockUIModule,
    CheckboxModule,
    AccordionModule,
    PanelModule,
    FileManagerModule,
    InputTextareaModule,
    CalendarModule,
    SelectButtonModule,
    DialogModule,
    CalendarModule,
    VendorsRoutingModule,
    SelectButtonModule,
  ],
  declarations: [
    VendorListComponent,
    VendorOpensComponent,
    VendorEditComponent,
    VendorEditGeneralComponent,
    VendorNewComponent,
    VendorExistsComponent,
    VendorAddressComponent,
    VendorContactsComponent,
    VendorAccountingComponent,
    VendorNotesComponent,
    VendorPurchasesComponent,
    VendorPhoneNumbersComponent,
    VendorEmailsComponent,
    VendorPhoneCallsComponent,
    VendorFilesComponent,
    VendorMasrketComponent,
    VendorSpecialtiesComponent,
    VendorContactsNesiComponent,
    VendorContactsBvComponent,
    VendorContactsBvItemComponent
],
  providers: [
    VendorsService,
  ],
  exports: [
    VendorEditComponent,
  ]
})
export class VendorsModule { }
