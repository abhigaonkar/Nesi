import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthGuard } from '../../services/authentication/authGuard';
import { RootVendorsDetailComponent } from './RootVendorsDetail/RootVendorsDetail.component';

@NgModule({
  imports: [
    RouterModule.forChild([
      {
        path: ':vendor_id', component: RootVendorsDetailComponent,
        canActivate: [AuthGuard], data: { title: 'Vendor' }
      },
    ])
  ],
  exports: [
    RouterModule
  ]
})
export class RootVendorsRouterModule { }

