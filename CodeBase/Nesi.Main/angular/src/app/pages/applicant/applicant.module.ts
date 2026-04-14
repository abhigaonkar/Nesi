
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
import { ApplicantListComponent } from './applicantList/applicantList.component';
import { ApplicantPageRouterModule } from './applicant.routing.module';
import { ApplicantEditComponent } from './applicant-edit/applicant-edit.component';
import { EmployeeService } from '../employees/_base/employeeService';
import { ApplicantFileComponent } from './applicant-file/applicant-file.component';
import { ApplicantOffersComponent } from './applicant-offers/applicant-offers.component';
import { ApplicantStartNewComponent } from './applicant-start-new/applicant-start-new.component';
import { ApplicantEditPaneComponent } from './applicant-edit-pane/applicant-edit-pane.component';
import { EmployeesPageModule } from '../employees/employees.module';
import { NesiSideBarModule } from '../../components/shared/nesi-sideBar/nesi-sideBar.module';
@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ButtonModule,
        ReactiveFormsModule,
        LoadingSpinnerModule,
        PipesModule,
        ApplicantPageRouterModule,
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
        ApplicantListComponent,
        ApplicantEditComponent,
        ApplicantFileComponent,
        ApplicantOffersComponent,
        ApplicantStartNewComponent,
        ApplicantEditPaneComponent
    ],
    providers: [
        EmployeeService,
    ],
    exports: [
        ApplicantListComponent,
        ApplicantEditComponent,
        ApplicantEditPaneComponent
    ]
})
export class ApplicantPageModule { }
