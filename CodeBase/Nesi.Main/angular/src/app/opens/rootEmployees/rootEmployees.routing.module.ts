import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthGuard } from '../../services/authentication/authGuard';
import { RootEmployeeDetailComponent } from './rootEmployeeDetail/rootEmployeeDetail.component';
import { RootEmployeeOfferComponent } from './rootEmployeeOffer/rootEmployeeOffer.component';
import { RootEmployeePrivilegeComponent } from './rootEmployeePrivilege/rootEmployeePrivilege.component';
import { RootEmployeeUserInfoComponent } from './rootEmployeeUserInfo/rootEmployeeUserInfo.component';
import { RootEmployeeDisciplinaryComponent } from './rootEmployeeDisciplinary/rootEmployeeDisciplinary.component';
import { RootEmployeeDaysOffComponent } from './rootEmployeeDaysOff/rootEmployeeDaysOff.component';
import { RootEmployeeFootprintsComponent } from './rootEmployeeFootprints/rootEmployeeFootprints.component';
import { RootEmployeeReviewsComponent } from './rootEmployeeReviews/rootEmployeeReviews.component';
import { RootEmployeeEmploymentsComponent } from './rootEmployeeEmployments/rootEmployeeEmployments.component';
import { RootEmployeeITComponent } from './rootEmployeeIT/rootEmployeeIT.component';
import { RootEmployeeReviewComponent } from './rootEmployeeReview/rootEmployeeReview.component';
import { RootEmployeeTermiantionComponent } from './rootEmployeeTermiantion/rootEmployeeTermiantion.component';

@NgModule({
  imports: [
    RouterModule.forChild([
      {
        path: ':member_id/employments', component: RootEmployeeEmploymentsComponent,
        canActivate: [AuthGuard], data: { title: 'Employee Employment List' }
      },
      {
        path: ':member_id/reviews', component: RootEmployeeReviewsComponent,
        canActivate: [AuthGuard], data: { title: 'Employee Review List' }
      },
      {
        path: ':member_id/reviews/:id', component: RootEmployeeReviewComponent,
        canActivate: [AuthGuard], data: { title: 'Employee Review Details' }
      },
      {
        path: ':member_id/footprints', component: RootEmployeeFootprintsComponent,
        canActivate: [AuthGuard], data: { title: 'Employee Foot Prints' }
      },
      {
        path: ':member_id/dayoff', component: RootEmployeeDaysOffComponent,
        canActivate: [AuthGuard], data: { title: 'Employee Days Off / Vacation' }
      },
      {
        path: ':member_id/disciplinary', component: RootEmployeeDisciplinaryComponent,
        canActivate: [AuthGuard], data: { title: 'Employee Disciplinary' }
      },
      {
        path: ':member_id/userinfo', component: RootEmployeeUserInfoComponent,
        canActivate: [AuthGuard], data: { title: 'Employee User Info' }
      },
      {
        path: ':member_id/termination', component: RootEmployeeTermiantionComponent,
        canActivate: [AuthGuard], data: { title: 'Employee User Info' }
      },
      {
        path: ':member_id/it', component: RootEmployeeITComponent,
        canActivate: [AuthGuard], data: { title: 'Employee IT' }
      },
      {
        path: ':member_id/privilege', component: RootEmployeePrivilegeComponent,
        canActivate: [AuthGuard], data: { title: 'Employee Privileges' }
      },
      {
        path: ':member_id/offer/:id', component: RootEmployeeOfferComponent,
        canActivate: [AuthGuard], data: { title: 'Employee Offer' }
      },
      {
        path: ':member_id', component: RootEmployeeDetailComponent,
        canActivate: [AuthGuard], data: { title: 'Employee' }
      },

    ])
  ],
  exports: [
    RouterModule
  ]
})
export class RootEmployeeRouterModule { }

