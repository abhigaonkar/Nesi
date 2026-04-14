import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthGuard } from '../../services/authentication/authGuard';
import { RootCustomersDetailComponent } from 'app/opens/rootCustomers/RootCustomersDetail/RootCustomersDetail.component';

@NgModule({
  imports: [
    RouterModule.forChild([
      {
        path: ':customer_id', component: RootCustomersDetailComponent,
        canActivate: [AuthGuard], data: { title: 'Customer' }
      },
    ])
  ],
  exports: [
    RouterModule
  ]
})
export class RootCustomersRouterModule { }

