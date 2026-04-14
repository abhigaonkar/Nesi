import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthGuard } from '../../services/authentication/authGuard';
import { ReleaseComponent } from './release/release.component';
import { PasswordEnhanceComponent } from './passwordEnhance/passwordEnhance.component';

@NgModule({
  imports: [
    RouterModule.forChild([
      { path: '', component: ReleaseComponent, canActivate: [AuthGuard], data: { title: 'Spark Ops - Release Tools' } },
    ])
  ],
  exports: [
    RouterModule
  ]
})
export class ReleaseSystemRoutingModule { }

