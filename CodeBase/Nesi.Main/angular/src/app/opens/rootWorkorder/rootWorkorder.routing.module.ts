import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthGuard } from '../../services/authentication/authGuard';
import { RootWorkorderWocommentsComponent } from './rootWorkorder-wocomments/rootWorkorder-wocomments.component';

@NgModule({
  imports: [
    RouterModule.forChild([
      {
        path: ':str_wo_id/wocomments', component: RootWorkorderWocommentsComponent,
        canActivate: [AuthGuard], data: { title: 'Work Order' }
      },
    ])
  ],
  exports: [
    RouterModule
  ]
})
export class RootWorkorderRouterModule { }

