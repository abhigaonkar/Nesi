
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
    EditorModule, ListboxModule, PickListModule, TreeModule, RadioButtonModule, SidebarModule, AutoCompleteModule
} from 'primeng/primeng';
import { BreadcurmbBarModule } from 'app/components/shared/breadcrumb/breadcurmb.module';
import { EmployeesListComponent } from './EmployeesList/EmployeesList.component';
import { EmployeesPageRouterModule } from './employees.routing.module';
import { NesiDatatableModule } from '../../components/nesi-datatable/nesi-datatable.module';
import { EmployeeEditComponent } from './employee-edit/employee-edit.component';
import { EmployeeUserInformationComponent } from './employee-user-information/employee-user-information.component';
import { EmployeeItComponent } from './employee-it/employee-it.component';
import { EmployeeWageComponent } from './employee-wage/employee-wage.component';
import { EmployeeDayOffComponent } from './employee-day-off/employee-day-off.component';
import { EmployeeDisciplinaryComponent } from './employee-disciplinary/employee-disciplinary.component';
import { EmployeePrivilegesComponent } from './employee-privileges/employee-privileges.component';
import { EmployeeFilesComponent } from './employee-files/employee-files.component';
import { EmployeeTerminationComponent } from './employee-termination/employee-termination.component';
import { EmployeeEmploymentComponent } from './employee-employment/employee-employment.component';
import { EmployeeReviewsComponent } from './employee-reviews/employee-reviews.component';
import { EmployeeFootprintsComponent } from './employee-footprints/employee-footprints.component';
import { EmployeeService } from './_base/employeeService';
import { EmployeeWageWageComponent } from './_wage/employee-wage-wage/employee-wage-wage.component';
import { EmployeeDaysOffListComponent } from './_day-off/employee-days-off-list/employee-days-off-list.component';
// tslint:disable-next-line:max-line-length
import { EmployeeDaysOffVacationRequestComponent } from './_day-off/employee-days-off-vacation-request/employee-days-off-vacation-request.component';
// tslint:disable-next-line:max-line-length
import { EmployeeDaysOffVacationTrascationsComponent } from './_day-off/employee-days-off-vacation-trascations/employee-days-off-vacation-trascations.component';
import { EmployeeDisiplinaryEditComponent } from './_disciplinary/employee-disiplinary-edit/employee-disiplinary-edit.component';
import { FileManagerModule } from '../../components/shared/fileManager/fileManager.module';
import { EmployeeOfferComponent } from './_employment/employee-offer/employee-offer.component';
import { EmployeeOfferDetailComponent } from './_employment/employee-offer-detail/employee-offer-detail.component';
import { EmployeeOfferExtrasComponent } from './_employment/employee-offer-extras/employee-offer-extras.component';
import { EmployeeOfferMilestonesComponent } from './_employment/employee-offer-milestones/employee-offer-milestones.component';
import { EmployeeOfferNotesComponent } from './_employment/employee-offer-notes/employee-offer-notes.component';
// tslint:disable-next-line:max-line-length
import { EmployeeOfferResponsibilitiesComponent } from './_employment/employee-offer-responsibilities/employee-offer-responsibilities.component';
import { EmployeeOfferSignbackComponent } from './_employment/employee-offer-signback/employee-offer-signback.component';
import { EmployeeOfferWageComponent } from './_employment/employee-offer-wage/employee-offer-wage.component';
// tslint:disable-next-line:max-line-length
import { EmployeePrivilegeUserSwitchingComponent } from './_privileges/employee-privilege-user-switching/employee-privilege-user-switching.component';
import { EmployeePrivilegeTreeComponent } from './_privileges/employee-privilege-tree/employee-privilege-tree.component';
// tslint:disable-next-line:max-line-length
import { EmployeeTerminationChecklistComponent } from './_termination/employee-termination-checklist/employee-termination-checklist.component';
import { EmployeeTerminationStartComponent } from './_termination/employee-termination-start/employee-termination-start.component';
import { EmployeeReviewComponent } from './_reviews/employee-review/employee-review.component';
import { EmployeeReviewitemListComponent } from './_reviews/employee-reviewitem-list/employee-reviewitem-list.component';
import { EmployeeReviewitemMilestoneComponent } from './_reviews/employee-reviewitem-milestone/employee-reviewitem-milestone.component';
import { EmployeeContactComponent } from './employee-contact/employee-contact.component';
import { NesiSideBarModule } from '../../components/shared/nesi-sideBar/nesi-sideBar.module';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ButtonModule,
        ReactiveFormsModule,
        LoadingSpinnerModule,
        PipesModule,
        EmployeesPageRouterModule,
        DialogModule,
        DataTableModule,
        TabViewModule,
        InputTextareaModule,
        MultiSelectModule,
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
        ListboxModule,
        PickListModule,
        TreeModule,
        RadioButtonModule,
        SidebarModule,
        NesiSideBarModule,
        AutoCompleteModule,
    ],
    declarations: [
        EmployeesListComponent,
        EmployeeEditComponent,
        EmployeeUserInformationComponent,
        EmployeeItComponent,
        EmployeeWageComponent,
        EmployeeDayOffComponent,
        EmployeeDisciplinaryComponent,
        EmployeePrivilegesComponent,
        EmployeeFilesComponent,
        EmployeeTerminationComponent,
        EmployeeEmploymentComponent,
        EmployeeReviewsComponent,
        EmployeeFootprintsComponent,
        EmployeeWageWageComponent,
        EmployeeDaysOffListComponent,
        EmployeeDaysOffVacationRequestComponent,
        EmployeeDaysOffVacationTrascationsComponent,
        EmployeeDisiplinaryEditComponent,
        EmployeeOfferComponent,
        EmployeeOfferDetailComponent,
        EmployeeOfferExtrasComponent,
        EmployeeOfferMilestonesComponent,
        EmployeeOfferNotesComponent,
        EmployeeOfferResponsibilitiesComponent,
        EmployeeOfferSignbackComponent,
        EmployeeOfferWageComponent,
        EmployeePrivilegeUserSwitchingComponent,
        EmployeePrivilegeTreeComponent,
        EmployeeTerminationChecklistComponent,
        EmployeeTerminationStartComponent,
        EmployeeReviewComponent,
        EmployeeReviewitemListComponent,
        EmployeeReviewitemMilestoneComponent,
        EmployeeContactComponent,
],
    providers: [
        EmployeeService,
    ],
    exports: [
        EmployeeEditComponent,
        EmployeeOfferComponent,
        EmployeeReviewComponent,
        EmployeeTerminationComponent,
        EmployeeWageComponent,
        EmployeeUserInformationComponent,
        EmployeePrivilegesComponent,
        EmployeeFootprintsComponent,
        EmployeeDisciplinaryComponent,
        EmployeeDayOffComponent,
        EmployeeEmploymentComponent,
        EmployeeFilesComponent,
        EmployeeItComponent,
        EmployeeReviewsComponent,
        EmployeeTerminationComponent,
    ]
})
export class EmployeesPageModule { }
