import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthGuard } from '../../services/authentication/authGuard';
import { UsernamePasswordComponent } from './usernamePassword/usernamePassword.component';

@NgModule({
  imports: [
    RouterModule.forChild([
      {
        path: 'usernamepassword', component: UsernamePasswordComponent,
        canActivate: [AuthGuard], data: { title: 'Change Password' }
      }
    ])
  ],
  exports: [
    RouterModule
  ]
})
export class DemoRoutingModule { }

