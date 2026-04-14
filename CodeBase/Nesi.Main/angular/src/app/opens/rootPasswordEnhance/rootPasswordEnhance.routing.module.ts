import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ResetPasswordComponent } from '../../pages/releaseSystem/resetpassword/resetpassword.component';

@NgModule({
  imports: [
    RouterModule.forChild([
      {
        path: 'reset/:token', component: ResetPasswordComponent,
       data: { title: 'Reset password' }
      },
    ])
  ],
  exports: [
    RouterModule
  ]
})
export class RootPasswordEnhanceRouterModule { }

