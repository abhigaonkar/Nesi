
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FieldErrorDisplayModule } from '../../components/shared/fieldErrorDisplay/fieldErrorDisplay.module';
import { LoadingSpinnerModule } from '../../components/shared/loading-spinner/loading-spinner.module';
import { PipesModule } from '../../pipes/Pipes.module';
import {
    ButtonModule, DialogModule, DataTableModule, TabViewModule,
    InputTextareaModule, MultiSelectModule,
    DropdownModule, CheckboxModule, InputTextModule,
    FileUploadModule, BlockUIModule, MessageModule,
    MessagesModule, PanelModule, CalendarModule,
    EditorModule, ListboxModule, PickListModule, TreeModule, RadioButtonModule
} from 'primeng/primeng';
import { BreadcurmbBarModule } from 'app/components/shared/breadcrumb/breadcurmb.module';
import { FileManagerModule } from '../../components/shared/fileManager/fileManager.module';
import { NesiDatatableModule } from '../../components/nesi-datatable/nesi-datatable.module';
import { CustomerRequestAdminListComponent } from './customerrequestadminList/customerrequestadminList.component';
import { CustomerRequestAdminNewComponent } from './customerrequestadminNew/customerrequestadminNew.component';
import { CustomerRequestAdminEditComponent } from './customerrequestadminEdit/customerrequestadminEdit.component';
import { CustomerRequestAdminPageRouterModule } from './customerrequestadmin.routing.module';
import { EmployeeService } from '../employees/_base/employeeService';
import { EmployeesPageModule } from '../employees/employees.module';
import { NesiSideBarModule } from '../../components/shared/nesi-sideBar/nesi-sideBar.module';
import { CustomersService } from 'app/services/pages/customer.services';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ButtonModule,
        ReactiveFormsModule,
        LoadingSpinnerModule,
        PipesModule,
        CustomerRequestAdminPageRouterModule,
        DialogModule,
        DataTableModule,
        TabViewModule,
        InputTextareaModule,
        DropdownModule,
        CheckboxModule,
        InputTextModule,
        DialogModule,
        FieldErrorDisplayModule,
        BreadcurmbBarModule,
        FileUploadModule,
        NesiDatatableModule,
        BlockUIModule,
        MessagesModule,
        PanelModule,
        CalendarModule,
        InputTextareaModule,
        EditorModule,
        FileManagerModule,
        RadioButtonModule,
        EmployeesPageModule,
        NesiSideBarModule,
    ],
    declarations: [
      CustomerRequestAdminListComponent,
      CustomerRequestAdminNewComponent,
      CustomerRequestAdminEditComponent
    ],
    providers: [
      EmployeeService,
      CustomersService
    ],
    exports: [
        CustomerRequestAdminListComponent
    ]
})
export class CustomerRequestAdminPageModule { }
