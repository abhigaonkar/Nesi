import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthGuard } from '../../services/authentication/authGuard';
import { RootApplicantDetailComponent } from './rootApplicantDetail/rootApplicantDetail.component';
import { RootApplicantOfferComponent } from './rootApplicantOffer/rootApplicantOffer.component';

@NgModule({
  imports: [
    RouterModule.forChild([
      {
        path: ':applicant_id', component: RootApplicantDetailComponent,
        canActivate: [AuthGuard], data: { title: 'Applicant' }
      },
      {
        path: ':applicant_id/offer/:id', component: RootApplicantOfferComponent,
        canActivate: [AuthGuard], data: { title: 'Applicant Offer' }
      },
    ])
  ],
  exports: [
    RouterModule
  ]
})
export class RootApplicantRouterModule { }

