import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthGuard } from '../../services/authentication/authGuard';
import { HomeListComponent } from 'app/pages/homepage/home-list/home-list.component';

@NgModule({
  imports: [
    RouterModule.forChild([
      { path: '', component: HomeListComponent, canActivate: [AuthGuard], data: { title: 'Home Page' } },
    ])
  ],
  exports: [
    RouterModule
  ]
})
export class HomePageRouterModule { }

