import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthGuard } from '../../services/authentication/authGuard';
import { ApplicantListComponent } from './applicantList/applicantList.component';

@NgModule({
  imports: [
    RouterModule.forChild([
      { path: '', component: ApplicantListComponent, canActivate: [AuthGuard], data: { title: 'Applicants' } },
    ])
  ],
  exports: [
    RouterModule
  ]
})
export class ApplicantPageRouterModule { }

