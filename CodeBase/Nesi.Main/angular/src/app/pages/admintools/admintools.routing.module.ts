import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthGuard } from 'services/authentication/authGuard';
import {IntegrationErrorComponent} from 'pages/admintools/integration-error/integration-error.component';
import {SelectDurationComponent} from '../admintools/select-duration/select-duration.component';


@NgModule({
  imports: [
    RouterModule.forChild([ 
      {
        path: '', component: SelectDurationComponent,
        canActivate: [AuthGuard], data: { title: 'Integration Errors' }
      },
      {
        path: '/integrationerror', component: IntegrationErrorComponent,
        canActivate: [AuthGuard], data: { title: 'Integration Errors' }
      },
     
    ])
  ],
  exports: [
    RouterModule
  ]
})
export class AdmintoolsRoutingModule { }