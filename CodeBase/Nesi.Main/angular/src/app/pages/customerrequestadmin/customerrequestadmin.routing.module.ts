import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthGuard } from '../../services/authentication/authGuard';
import { CustomerRequestAdminListComponent } from './customerrequestadminList/customerrequestadminList.component';

@NgModule({
  imports: [
    RouterModule.forChild([
      { path: '', component: CustomerRequestAdminListComponent, canActivate: [AuthGuard], data: { title: 'Customer Request Admin' } },
    ])
  ],
  exports: [
    RouterModule
  ]
})
export class CustomerRequestAdminPageRouterModule { }

