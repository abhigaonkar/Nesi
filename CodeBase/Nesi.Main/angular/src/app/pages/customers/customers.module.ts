import { NgModule } from '@angular/core';
import { BusinessUnitService } from '../../services/pages/business_unit.service';
import { FieldErrorDisplayModule } from '../../components/shared/fieldErrorDisplay/fieldErrorDisplay.module';
import {
  InputTextModule, ButtonModule, DataTableModule, TabViewModule,
  MultiSelectModule, FieldsetModule, DropdownModule,
  BlockUIModule, CheckboxModule, AccordionModule,
  PanelModule, InputTextareaModule, CalendarModule,
  SelectButtonModule, DialogModule, MessageModule, MessagesModule
} from 'primeng/primeng';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CustomerListComponent } from 'app/pages/customers/customer-list/customer-list.component';
import { CustomersRoutingModule } from 'app/pages/customers/customers.routing.module';
import { CustomerNewPaneComponent } from './customer-new-pane/customer-new-pane.component';
import { CustomerNewComponent } from './customer-new/customer-new.component';
import { CustomerAddressComponent } from './customer-address/customer-address.component';
import { CustomerSearchResultsComponent } from './customer-search-results/customer-search-results.component';
import { PipesModule } from 'app/pipes/Pipes.module';
import { CommonModule } from '@angular/common';
import { BreadcurmbBarModule } from 'app/components/shared/breadcrumb/breadcurmb.module';
import { PostcodeSearchComponent } from './postcode-search/postcode-search.component';
import { NesiDatatableModule } from 'app/components/nesi-datatable/nesi-datatable.module';
import { CustomersService } from 'app/services/pages/customer.services';
import { CustomerEditComponent } from 'app/pages/customers/customer-edit/customer-edit.component';
import { CustomerEditGeneralComponent } from './customer-edit-general/customer-edit-general.component';
import { CustomerAddressDetailComponent } from './customer-address-detail/customer-address-detail.component';
import { CustomerAddressAccountingComponent } from './customer-address-accounting/customer-address-accounting.component';
import { CustomerAddressFilesComponent } from './customer-address-files/customer-address-files.component';
import { FileManagerModule } from 'app/components/shared/fileManager/fileManager.module';
import { CustomerContactComponent } from './customer-contact/customer-contact.component';
import { CustomerContactListComponent } from './customer-contact-list/customer-contact-list.component';
import { CustomerSalesComponent } from './customer-sales/customer-sales.component';
import { CustomerAssetComponent } from './customer-asset/customer-asset.component';
import { CustomerPhonenumberComponent } from './customer-phonenumber/customer-phonenumber.component';
import { CustomerAccountingSettingsComponent } from './customer-accounting-settings/customer-accounting-settings.component';
import { CustomerWorkordersComponent } from './customer-workorders/customer-workorders.component';
import { CustomerBusinessUnitComponent } from './customer-business-unit/customer-business-unit.component';
import { CustomerQuotesComponent } from './customer-quotes/customer-quotes.component';
import { CustomerActivityComponent } from './customer-activity/customer-activity.component';
import { CustomerAccountingRatesComponent } from './customer-accounting-rates/customer-accounting-rates.component';
import { CustomerAccountingArnotesComponent } from './customer-accounting-arnotes/customer-accounting-arnotes.component';
import { CustomerAccountingSetComponent } from './customer-accounting-set/customer-accounting-set.component';
import { CustomerSalesPropertiesComponent } from './customer-sales-properties/customer-sales-properties.component';
import { CustomerSalesHistoryComponent } from './customer-sales-history/customer-sales-history.component';
import { CustomerSalesStatsComponent } from './customer-sales-stats/customer-sales-stats.component';
import { CustomerEmailComponent } from './customer-email/customer-email.component';
import { CustomerSpecialtiesComponent } from './customer-specialties/customer-specialties.component';
import { CustomerAremailsComponent } from './customer-aremails/customer-aremails.component';
import { CustomerInvoicingnotesComponent } from './customer-invoicingnotes/customer-invoicingnotes.component';
import { CustomerContactPhoneComponent } from './customer-contact-phone/customer-contact-phone.component';
import { CustomerOpensComponent } from './customer-opens/customer-opens.component';
import { ListComponent } from './customer-asset-rate/list/list.component';
import { UpdateComponent } from './customer-asset-rate/update/update.component';
import { DxDataGridModule, DxPopupModule, DxSelectBoxModule, DxListModule, DxButtonModule, DxLoadPanelModule,
  DxResponsiveBoxModule, DxFormModule, DevExtremeModule, DxTextBoxModule } from 'devextreme-angular';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    FieldErrorDisplayModule,
    InputTextModule,
    ButtonModule,
    CustomersRoutingModule,
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
    DxDataGridModule,
    DxPopupModule,
    DxSelectBoxModule,
    DxListModule,
    DxButtonModule,
    DxLoadPanelModule,
    DxResponsiveBoxModule,
    DxFormModule,
    DevExtremeModule,
    DxTextBoxModule,
    MessageModule,
    MessagesModule
  ],
  declarations: [
    CustomerListComponent,
    CustomerNewPaneComponent,
    CustomerNewComponent,
    CustomerAddressComponent,
    CustomerSearchResultsComponent,
    PostcodeSearchComponent,
    CustomerEditComponent,
    CustomerEditGeneralComponent,
    CustomerAddressDetailComponent,
    CustomerAddressAccountingComponent,
    CustomerAddressFilesComponent,
    CustomerContactComponent,
    CustomerContactListComponent,
    CustomerSalesComponent,
    CustomerAssetComponent,
    CustomerPhonenumberComponent,
    CustomerAccountingSettingsComponent,
    CustomerWorkordersComponent,
    CustomerQuotesComponent,
    CustomerActivityComponent,
    CustomerAccountingRatesComponent,
    CustomerAccountingArnotesComponent,
    CustomerAccountingSetComponent,
    CustomerSalesPropertiesComponent,
    CustomerSalesHistoryComponent,
    CustomerSalesStatsComponent,
    CustomerEmailComponent,
    CustomerSpecialtiesComponent,
    CustomerAremailsComponent,
    CustomerInvoicingnotesComponent,
    CustomerContactPhoneComponent,
    CustomerOpensComponent,
    ListComponent,
    UpdateComponent,
    CustomerBusinessUnitComponent
],
  providers: [
    CustomersService,
  ],
  exports: [
    CustomerEditComponent,
    CustomerEditGeneralComponent,
  ]
})
export class CustomersModule { }
