import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { LayoutProfileListComponent } from './layoutProfileList/layoutProfileList.component';
import { AuthGuard } from '../../services/authentication/authGuard';

@NgModule({
  imports: [
    RouterModule.forChild([
      { path: '', component: LayoutProfileListComponent, canActivate: [AuthGuard], data: { title: 'Spark Ops - Settings' } },
    ])
  ],
  exports: [
    RouterModule
  ]
})
export class ProfileRoutingModule { }

