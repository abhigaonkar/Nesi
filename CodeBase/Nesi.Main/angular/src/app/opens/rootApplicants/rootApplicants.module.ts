import { NgModule } from '@angular/core';
import { PickListModule } from '../../components/shared/picklist/picklist.module';
import { CustomersModule } from 'app/pages/customers/customers.module';
import { RootApplicantRouterModule } from './rootApplicants.routing.module';
import { ApplicantPageModule } from '../../pages/applicant/applicant.module';
import { RootApplicantDetailComponent } from './rootApplicantDetail/rootApplicantDetail.component';
import { RootApplicantOfferComponent } from './rootApplicantOffer/rootApplicantOffer.component';
import { EmployeesPageModule } from '../../pages/employees/employees.module';
import { CustomerRequestAdminPageModule } from '../../pages/customerrequestadmin/customerrequestadmin.module';

@NgModule({
  imports: [
    RootApplicantRouterModule,
    ApplicantPageModule,
    EmployeesPageModule,
    CustomerRequestAdminPageModule,
   ],
  declarations: [
    RootApplicantDetailComponent
,
    RootApplicantOfferComponent
],
  providers: [
  ]
})
export class RootApplicantsModule { }
