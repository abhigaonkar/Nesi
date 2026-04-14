import { NgModule } from '@angular/core';
import { PickListModule } from '../../components/shared/picklist/picklist.module';
import { CustomersModule } from 'app/pages/customers/customers.module';
import { RootEmployeeDetailComponent } from './rootEmployeeDetail/rootEmployeeDetail.component';
import { RootEmployeeRouterModule } from './rootEmployees.routing.module';
import { EmployeesPageModule } from '../../pages/employees/employees.module';
import { RootEmployeeOfferComponent } from './rootEmployeeOffer/rootEmployeeOffer.component';
import { RootEmployeePrivilegeComponent } from './rootEmployeePrivilege/rootEmployeePrivilege.component';
import { RootEmployeeITComponent } from './rootEmployeeIT/rootEmployeeIT.component';
import { RootEmployeeUserInfoComponent } from './rootEmployeeUserInfo/rootEmployeeUserInfo.component';
import { RootEmployeeDaysOffComponent } from './rootEmployeeDaysOff/rootEmployeeDaysOff.component';
import { RootEmployeeDisciplinaryComponent } from './rootEmployeeDisciplinary/rootEmployeeDisciplinary.component';
import { RootEmployeeWageComponent } from './rootEmployeeWage/rootEmployeeWage.component';
import { RootEmployeeReviewsComponent } from './rootEmployeeReviews/rootEmployeeReviews.component';
import { RootEmployeeFootprintsComponent } from './rootEmployeeFootprints/rootEmployeeFootprints.component';
import { RootEmployeeEmploymentsComponent } from './rootEmployeeEmployments/rootEmployeeEmployments.component';
import { RootEmployeeReviewComponent } from './rootEmployeeReview/rootEmployeeReview.component';
import { RootEmployeeTermiantionComponent } from './rootEmployeeTermiantion/rootEmployeeTermiantion.component';


@NgModule({
  imports: [
    RootEmployeeRouterModule,
    EmployeesPageModule,
   ],
  declarations: [
    RootEmployeeDetailComponent,
    RootEmployeeOfferComponent,
    RootEmployeePrivilegeComponent,
    RootEmployeeITComponent,
    RootEmployeeUserInfoComponent,
    RootEmployeeDaysOffComponent,
    RootEmployeeDisciplinaryComponent,
    RootEmployeeWageComponent,
    RootEmployeeReviewsComponent,
    RootEmployeeFootprintsComponent,
    RootEmployeeEmploymentsComponent,
    RootEmployeeReviewComponent,
    RootEmployeeTermiantionComponent
],
  providers: [
  ]
})
export class RootEmployeesModule { }
